using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
    using System.IO;
#endif

using AutoMusic;


namespace AutoMusic
{
    [System.Serializable]
public class MIDINote
{
    public float velocity;
    public int noteNumber;
    public int beatIndex;
    public float noteOffset;
}
[System.Serializable]
public class MIDISource
{
    [Tooltip("Source .mid file. Files must be added tot eh system in editor, beatInstance based sequences are then stored for in-game playback")]
    public UnityEngine.Object midiFileInputObj;
    [Tooltip("Only notes played on this MIDI channel will be converted into AUtoMusic notes. Channel numbers may be off by 1 depend on export platform. -1 will use notes on all channels")]
    [Range(-1, 16)]
    public int channelFilter = 1;
    [Tooltip("Number of bars in the clip to process. Notes outside of this range will be skipped. If set to a longer duration than data contained in the .mid file, there will be a period of silence at the end")]
    [Range(1, 32)]
    public int loopBarCount = 8;
    [Tooltip("Ignore data in the .mid file prior to this beat index, and start counting the loopBarCount from here")]
    public int assessClipFromBeat = 0;
    [Tooltip("Enable to use the pitch data from the .mid file, when disabled notes will be set to pitch = 1 and play back at the base rate of the clip")]
    public bool applyPitch = true;
    [Tooltip("Amount of semitones to transpose the .mid file during processing, for moving to different keys")]
    [Range(-48, 48)]
    public int semitoneTranspose;
    [HideInInspector] public bool init;
}
[System.Serializable]
public class MIDISrcedBeatInstance
{
    public string filePath;
    public BeatInstance[] beatInstances;
}

    [ExecuteInEditMode]
    public class MidiToBeatInstance : InjectableGenerator
    {
        [Header("MIDI to Beat Settings")]
        [Tooltip("each MIDI source contains a source MIDI clip that can be converted into AutoMusic format")]
        public MIDISource[] srcMIDI;
        


        [Header("Debug Ref")]
        public int enteredNotes;
        public int foundNotes;
        public int dataFoundStartingFromBeatNumber;
        public MIDISrcedBeatInstance[] MIDISrcedBeatInstances;
        //[HideInInspector] public string[] midiFilePathStrings;
        //[HideInInspector] public BeatInstance[][] midiBeatInstances;
        [HideInInspector] public List<MIDINote> noteOns = new List<MIDINote>();
        [HideInInspector] public List<MIDINote> noteOffs = new List<MIDINote>();


        public override void GenerateInjectable()
        {
            int index = sequenceIndex;

            //pick random index if input index < 0
            if (index < 0 && MIDISrcedBeatInstances != null && MIDISrcedBeatInstances.Length > 0)
            {
                index = UnityEngine.Random.Range(0, MIDISrcedBeatInstances.Length);
            }

            if (MIDISrcedBeatInstances == null || index < 0 || index >= MIDISrcedBeatInstances.Length)
            {
                Debug.LogError("MIDI file not found at index " + index);
                return;
            }

            if (MIDISrcedBeatInstances[index].beatInstances != null && MIDISrcedBeatInstances[index].beatInstances.Length > index)
            {
                //beatInstances = ParseMidiFile(midiFilePathStrings[index], index);
                beatInstances = MIDISrcedBeatInstances[index].beatInstances;
            }
            else
            {
                Debug.LogError("MIDI file not found: " + MIDISrcedBeatInstances[index].filePath);
            }
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            if (srcMIDI != null)
            {
                MIDISrcedBeatInstances = new MIDISrcedBeatInstance[srcMIDI.Length];

                for (var i = 0; i < MIDISrcedBeatInstances.Length; i++)
                {
                    string inputPath = AssetDatabase.GetAssetPath(srcMIDI[i].midiFileInputObj);
                    string inputExtension = Path.GetExtension(inputPath);

                    if (srcMIDI[i].midiFileInputObj != null && inputExtension != ".mid")
                    {
                        Debug.Log("Input is not a valid a .mid file");
                        srcMIDI[i].midiFileInputObj = null;
                    }
                    else
                    {
                        MIDISrcedBeatInstances[i] = new MIDISrcedBeatInstance();

                        MIDISrcedBeatInstances[i].filePath = inputPath;
                        MIDISrcedBeatInstances[i].beatInstances = ParseMidiFile(inputPath, i);
                    }
                }

                if (srcMIDI.Length > 0)
                {
                    sequenceIndex = (int)Mathf.Clamp(sequenceIndex, -1, srcMIDI.Length);
                }
                else
                {
                    sequenceIndex = -1;
                }
            }

            if (srcMIDI != null)
            {
                for (var i = 0; i < srcMIDI.Length; i++)
                {
                    if (!srcMIDI[i].init)
                    {
                        srcMIDI[i].channelFilter = -1;
                        srcMIDI[i].loopBarCount = 2;
                        srcMIDI[i].applyPitch = true;
                        srcMIDI[i].init = true;
                    }
                }
            }
        }


       

        BeatInstance[] ParseMidiFile(string path, int index)
        {
            BeatInstance[] beatInstances = new BeatInstance[srcMIDI[index].loopBarCount * masterClock.beatsPerBar];
            for (var i = 0; i < beatInstances.Length; i++)
            {
                beatInstances[i] = SoundModule.InitBeatInstance();
            }

            noteOns = new List<MIDINote>();
            noteOffs = new List<MIDINote>();

            enteredNotes = 0;
            foundNotes = 0;
            dataFoundStartingFromBeatNumber = 999999999;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // Read header chunk
                    string headerChunkID = new string(reader.ReadChars(4));
                    if (headerChunkID != "MThd")
                    {
                        Debug.Log("Invalid MIDI file");
                        return null;
                    }

                    int headerChunkSize = ReadInt32BigEndian(reader);
                    int formatType = ReadInt16BigEndian(reader);
                    int numberOfTracks = ReadInt16BigEndian(reader);
                    int timeDivision = ReadInt16BigEndian(reader);

                    //Debug.Log($"Header Chunk: Size={headerChunkSize}, Format={formatType}, Tracks={numberOfTracks}, Division={timeDivision}");

                    int currentTime = 0;

                    // Read track chunks
                    //valid midi notes get saved to lists for noteOn and noteOff values, these will be combined to form notes in the following stage
                    for (int i = 0; i < numberOfTracks; i++)
                    {
                        string trackChunkID = new string(reader.ReadChars(4));
                        if (trackChunkID != "MTrk")
                        {
                            if (masterClock.verboseDebug)
                            {
                                Debug.Log("Invalid MIDI track chunk");
                            }
                            return null;
                        }

                        int trackChunkSize = ReadInt32BigEndian(reader);
                        long trackEnd = fs.Position + trackChunkSize;
                        //Debug.Log($"Track {i + 1}: Size={trackChunkSize}");

                        while (fs.Position < trackEnd)
                        {
                            int deltaTime = ReadVariableLengthQuantity(reader);
                            //Debug.Log("summing currentTime, deltaTime = " + deltaTime + ", currentTime = " + currentTime);
                            
                            byte eventType = reader.ReadByte();

                            if (eventType >= 0x80 && eventType <= 0xEF)
                            {
                                currentTime += deltaTime;
                                foundNotes += 1;

                                byte eventTypeHighNibble = (byte)(eventType & 0xF0);
                                byte channel = (byte)(eventType & 0x0F);

                                if (srcMIDI[index].channelFilter != -1)
                                {
                                    if (channel != srcMIDI[index].channelFilter)
                                    {
                                        if (masterClock.verboseDebug)
                                        {
                                            Debug.Log("Skipping MIDI note : mismatch channel");
                                        }
                                        continue;
                                    }
                                }

                                if (eventTypeHighNibble == 0x90 || eventTypeHighNibble == 0x80)
                                {
                                    byte noteNumber = reader.ReadByte();
                                    byte velocity = reader.ReadByte();

                                    MIDINote newNote = new MIDINote();
                                    newNote.noteNumber = (int)noteNumber;
                                    newNote.velocity = (int)velocity / 127f;
                                    float beatFractional = GetMusicalTime(currentTime, timeDivision);

                                    int beatIndex = Mathf.FloorToInt(beatFractional);
                                    dataFoundStartingFromBeatNumber = (int)Mathf.Min(beatIndex, dataFoundStartingFromBeatNumber);
                                    newNote.beatIndex = beatIndex - srcMIDI[index].assessClipFromBeat;

                                    if (newNote.beatIndex >= beatInstances.Length)
                                    {
                                        if (masterClock.verboseDebug)
                                        {
                                            Debug.Log("Skipping MIDI note : outside of loop range, beatIndex = " + newNote.beatIndex);
                                        }
                                        continue;
                                    }

                                    newNote.noteOffset = beatFractional - newNote.beatIndex;
                                    //newNote.musicalTime = GetMusicalTime(currentTime, timeDivision);

                                    if (eventTypeHighNibble == 0x90 && velocity > 0)
                                    {
                                        noteOns.Add(newNote);
                                        enteredNotes += 1;
                                        if (masterClock.verboseDebug)
                                        {
                                            Debug.Log($"Note On: Channel={channel}, Note={noteNumber}, Velocity={velocity}, DeltaTime={deltaTime}");
                                        }
                                    }
                                    else
                                    {
                                        noteOffs.Add(newNote);
                                        if (masterClock.verboseDebug)
                                        {
                                            Debug.Log($"Note Off: Channel={channel}, Note={noteNumber}, Velocity={velocity}, DeltaTime={deltaTime}");
                                        }
                                    }
                                }
                                else
                                {
                                    if (masterClock.verboseDebug)
                                    {
                                        Debug.Log("Skipping MIDI note : misc");
                                    }
                                    // Skip other events
                                    reader.BaseStream.Seek(1, SeekOrigin.Current);
                                }
                            }
                            else if (eventType == 0xFF)
                            {
                                byte metaType = reader.ReadByte();
                                int length = ReadVariableLengthQuantity(reader);
                                reader.BaseStream.Seek(length, SeekOrigin.Current);
                            }
                            else if (eventType == 0xF0 || eventType == 0xF7)
                            {
                                int length = ReadVariableLengthQuantity(reader);
                                reader.BaseStream.Seek(length, SeekOrigin.Current);
                            }
                        }
                    }

                    //loop from the on/off arrays generating notes. the offs are used to determine note lengths
                    for (var i = 0; i < noteOns.Count; i++)
                    {
                        float pitch = 1;
                        if (srcMIDI[index].applyPitch)
                        {
                            //60 is middle C
                            pitch = Mathf.Pow(1.059463f, (noteOns[i].noteNumber - 60 + srcMIDI[index].semitoneTranspose));
                        }
                        
                        float velocity = noteOns[i].velocity;
                        int beatIndex = noteOns[i].beatIndex;
                        if (beatIndex < 0 || beatIndex >= beatInstances.Length)
                        {
                            continue;
                        }

                        float noteOffset = noteOns[i].noteOffset;
                        float stepDurations = GetNoteLengthInSteps(i);
                        //Debug.Log("note endAfter = " + stepDurations);

                        beatInstances[beatIndex] = SoundModule.AddNoteToBeatInstance(beatInstances[beatIndex], noteOffset, 0, pitch, velocity, false, stepDurations);
                    }
                }
            }

            pendingPostProcess = true;

            return beatInstances;
        }

        float GetNoteLengthInSteps(int noteOnIndex)
        {
            int beatIndex = noteOns[noteOnIndex].beatIndex;
            float noteOffset = noteOns[noteOnIndex].noteOffset;
            int noteNumber = noteOns[noteOnIndex].noteNumber;

            for (var i = 0; i < noteOns.Count && i < noteOffs.Count; i++)
            {
                if (noteOffs[i].noteNumber != noteNumber)
                {
                    continue;
                }
                if (noteOffs[i].beatIndex < beatIndex)
                {
                    continue;
                }
                if (noteOffs[i].noteOffset <= noteOffset)
                {
                    continue;
                }
                int beats = noteOffs[i].beatIndex - beatIndex;
                float frac = noteOffs[i].noteOffset - noteOffset;
                if (beats > 0)
                {
                    frac = (1 - noteOffset) + noteOffs[i].noteOffset;
                }
                return (beats + frac) * masterClock.divisionsPerBeat;
            }
            return -1;
        }

        float GetMusicalTime(int currentTime, int timeDivision)
        {
            //Debug.Log("getting musical time, " +  currentTime + " / " + timeDivision);
            return (float)currentTime / (float)timeDivision;
        }

        int ReadInt32BigEndian(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        int ReadInt16BigEndian(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(2);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        int ReadVariableLengthQuantity(BinaryReader reader)
        {
            int value = 0;
            byte byteRead;
            do
            {
                byteRead = reader.ReadByte();
                value = (value << 7) | (byteRead & 0x7F);
            } while ((byteRead & 0x80) != 0);
            return value;
        }
#endif
    }
}