using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{

    public enum BassRhythmLogic
    {
        Percish,
        FollowOther,
        Snarish,
        MarkChanges
    }

    public enum BassPitchLogic
    {
        ChordRoots,
        Spam157,
        PentatonicSpam
    }

    public class ModuleBass : SoundModule
    {


        public HarmonicHub harmonicHub;
        public BassPitchLogic pitchLogic;
        [Range(0, 100)]
        public float arpeggiation;
        public BassRhythmLogic rhythmLogic;
        public bool loopSound;
        [Header("Rhythm Props : FollowOther")]
        public SoundModule rhythmSource;
        [Range(0, 100)]
        public float deletionsFromRhythmSource;
        [Range(0, 100)]
        public float additionalInbetweensFromRhythmSource;
        [Range(0, 100)]
        public float additionalArpOnInbetweens = 25;
        [Header("Rhythm Props : Percish")]
        [Range(0, 100)]
        public float baseRhythmicDistortion;
        public Vector2 baseRhythmicDensity = Vector2.one;

        [Header("Rhythm Props : Snarish")]
        [Range(0, 8)]
        public float beatsUntilStart = 0.5f;
        public int[] snareRepeatStack = new int[] { 8, 6, 2, 10, 3, 4, 5 };
        [Range(0.5f, 8)]
        public float repeatStackPowerFunction = 3.0f;

        //[Header("DebugRef")]
        [HideInInspector] public float[] pitches;



        void OnValidate()
        {
            SanitiseInput();
        }
        public override void SanitiseInput()
        {
            baseRhythmicDistortion = Mathf.Clamp(baseRhythmicDistortion, 0, 100);
            baseRhythmicDensity = ValidateRhythmDensity(baseRhythmicDensity);
            beatsUntilStart = ValidateSnareStart(beatsUntilStart);
            snareRepeatStack = ValidateRepeatStack(snareRepeatStack);
        }

        void Start()
        {
        }

        void LateUpdate()
        {
            if (!init)
            {
                if (rhythmLogic == BassRhythmLogic.FollowOther && rhythmSource != null)
                {
                    if (!rhythmSource.init)
                    {
                        return;
                    }
                }
                init = true;
                GenerateInit();
            }
        }


        //should be different types for this : beat distort, riff, stop...
        public override void GenerateLoop()
        {
            GetBeatsFromTimeSig();
            BeatInstance[] beatInstanceInnerLoop = new BeatInstance[loopBeatCount];

            int beatDivisions = masterClock.divisionsPerBeat;

            if (rhythmLogic == BassRhythmLogic.Snarish)
            {
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = InitBeatInstance();
                }


                int beatInstanceIndex = 0;

                int stepCount = loopBeatCount * beatDivisions;
                int nextStep = (int)(beatsUntilStart * beatDivisions);

                for (int stepIndex = 0; stepIndex < stepCount; stepIndex++)
                {
                    if (stepIndex != nextStep)
                    {
                        continue;
                    }
                    float beatDivisor = (float)stepIndex / (float)beatDivisions;
                    beatInstanceIndex = Mathf.FloorToInt(beatDivisor);
                    float pointInBeat = beatDivisor - beatInstanceIndex;

                    //using output param to mark notes that can take other tones from the chord
                    int output = 0;
                    if (D100(arpeggiation))
                    {
                        output = Random.Range(2, 5);
                    }
                    beatInstanceInnerLoop[beatInstanceIndex] = AddNoteToBeatInstance(beatInstanceInnerLoop[beatInstanceIndex], pointInBeat, output);

                    int repeatInSteps = snareRepeatStack[(int)(Mathf.Pow(Random.Range(0.0f, 1.0f), repeatStackPowerFunction) * snareRepeatStack.Length)];

                    nextStep += repeatInSteps;
                }

            }
            if (rhythmLogic == BassRhythmLogic.Percish)
            {
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = InitBeatInstance();

                    int notesInBeat = (int)Random.Range(baseRhythmicDensity.x, baseRhythmicDensity.y);

                    for (var j = 0; j < notesInBeat; j++)
                    {
                        float currDistortion = baseRhythmicDistortion;
                        float thisDistortion = 0;
                        if (j > 0)
                        {
                            thisDistortion = 1.0f / (beatDivisions / 2);
                        }
                        if (j > 1)
                        {
                            currDistortion += j * 20.0f;
                        }
                        if (D100(currDistortion))
                        {
                            thisDistortion = (float)(Random.Range(0, beatDivisions)) / (float)beatDivisions;
                            //Debug.Log("thisDistortion " + thisDistortion);

                        }
                        float noteOffset = thisDistortion;

                        if (!beatInstanceInnerLoop[i].noteFractionOffsets.Contains(noteOffset))
                        {
                            //using output param to mark notes that can take other tones from the chord
                            int output = 0;
                            if (D100(arpeggiation))
                            {
                                output = Random.Range(2, 5);
                            }
                            beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], noteOffset, output);
                        }
                    }
                }
            }
            if (rhythmLogic == BassRhythmLogic.FollowOther)
            {
                int repeatIndex = (int)Mathf.Min(rhythmSource.beatInstances.Length, beatInstanceInnerLoop.Length);
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    int fetchIndex = (int)(i % repeatIndex);
                    beatInstanceInnerLoop[i] = DuplicateBeatInstance(rhythmSource.beatInstances[fetchIndex]);

                    //deletions
                    for (var j = 0; j < beatInstanceInnerLoop[i].noteFractionOffsets.Count; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        if (D100(deletionsFromRhythmSource))
                        {
                            RemoveNoteFromBeatInstance(beatInstanceInnerLoop[i], j);
                        }
                    }

                    //add inbetweens
                    if (D100(additionalInbetweensFromRhythmSource))
                    {
                        int inbetweens = (int)Random.Range(baseRhythmicDensity.x, baseRhythmicDensity.y);
                        for (var j = 0; j < inbetweens; j++)
                        {
                            float noteOffset = (Random.Range(0, beatDivisions)) / (float)beatDivisions;

                            int output = 0;
                            if (D100((arpeggiation + additionalArpOnInbetweens) * 1.5f))
                            {
                                output = Random.Range(2, 5);
                            }

                            beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], noteOffset, output, 1, 0.5f);
                        }
                    }
                }
            }

            if (rhythmLogic == BassRhythmLogic.MarkChanges)
            {
                Chord[] chordSequence = harmonicHub.chordSequence;
                List<int> chordChangeSteps = new List<int>();
                for (var i = 0; i < chordSequence.Length; i++)
                {
                    chordChangeSteps.Add(chordSequence[i].startStep);
                }

                int currStep = 0;
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = InitBeatInstance();

                    for (var j = 0; j < beatDivisions; j++)
                    {
                        float thisOffset = (float)j / beatDivisions;

                        if (chordChangeSteps.Contains(currStep))
                        {
                            int chordChangeIndex = chordChangeSteps.IndexOf(currStep);
                            Chord chord = chordSequence[chordChangeIndex];

                            float pitch = chord.chord[0];

                            beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], thisOffset, 0);

                        }
                        currStep += 1;
                    }
                }
            }


            beatInstanceInnerLoop = TrimBeat(beatInstanceInnerLoop, densityTrimInnerLoop);

            if (pitchLogic == BassPitchLogic.Spam157 || pitchLogic == BassPitchLogic.PentatonicSpam)
            {
                float[] pitches = GetPitches(pitchLogic, harmonicHub);
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    for (var j = 0; j < beatInstanceInnerLoop[i].noteFractionOffsets.Count; j++)
                    {
                        //Debug.Log("bass output = " + beatInstanceInnerLoop[i].noteOutputs[j]);
                        float pitch = pitches[0];
                        if (beatInstanceInnerLoop[i].noteOutputs[j] != 0)
                        {
                            pitch = pitches[Random.Range(0, pitches.Length)];

                        }
                        beatInstanceInnerLoop[i].pitches[j] = pitch;
                    }
                }
            }


            //match harmonic hub length
            int harmonicLoopLength = harmonicHub.barsInSequence * masterClock.beatsPerBar;
            BeatInstance[] beatInstancesHarmonicLoop = new BeatInstance[harmonicLoopLength];
            for (var i = 0; i < beatInstancesHarmonicLoop.Length; i++)
            {
                beatInstancesHarmonicLoop[i] = DuplicateBeatInstance(beatInstanceInnerLoop[i % beatInstanceInnerLoop.Length]);
            }


            if (pitchLogic == BassPitchLogic.ChordRoots)
            {
                for (var i = 0; i < beatInstancesHarmonicLoop.Length; i++)
                {
                    int harmonicStepRangeStart = (i * beatDivisions);
                    //Debug.Log("step start " + harmonicStepRangeStart);

                    BeatInstance srcBeat = beatInstancesHarmonicLoop[i];
                    BeatInstance newBeat = DuplicateBeatInstance(srcBeat);

                    for (var j = 0; j < beatInstancesHarmonicLoop[i].noteFractionOffsets.Count; j++)
                    {
                        int thisStep = harmonicStepRangeStart + (int)(beatInstancesHarmonicLoop[i].noteFractionOffsets[j] * beatDivisions);

                        thisStep = (int)Mathf.Repeat(thisStep, harmonicHub.chordSequencePerStepRef.Length);
                        int chordDegree = newBeat.noteOutputs[j];
                        //second chance to mod pitch, not bound to inner loop
                        if (D100(arpeggiation * 0.25f))
                        {
                            chordDegree = Random.Range(0, harmonicHub.chordSequencePerStepRef[thisStep].chord.Count + 1);
                        }
                        if (chordDegree >= harmonicHub.chordSequencePerStepRef[thisStep].chord.Count)
                        {
                            chordDegree = 0;
                        }

                        float pitch = FoldHiPitches(harmonicHub.chordSequencePerStepRef[thisStep].chord[chordDegree]);
                        //Debug.Log("pitch = " + pitch + " thisStep = " + thisStep);
                        float thisPitch = pitch;

                        newBeat.pitches[j] = thisPitch;
                    }

                    beatInstancesHarmonicLoop[i] = newBeat;
                }
            }




            //extend loop to fill sequence
            FullSeqBeatInstances(beatInstancesHarmonicLoop, harmonicLoopLength);

            beatInstances = TrimBeat(beatInstances, densityTrimFullSequence);
        }





        public override void ProcessBeat()
        {
            ProcessBeatPitched(true, loopSound, HarmonicHub.GetTranspose(harmonicHub.transpose));

            if (loopSound && !active)
            {
                StopAudio();
            }
        }
    }
}