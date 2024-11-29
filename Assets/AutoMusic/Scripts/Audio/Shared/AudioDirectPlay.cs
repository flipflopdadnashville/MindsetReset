using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;
using System;


namespace AutoMusic
{
    [System.Serializable]
    public struct AudioClipCache
    {
        public AudioClip audioClip;
        public int clipSampleRate;
        public int clipChannels;
        public int clipSamples;
        public float sampleRateSpeedDiv;
        public float[] clipData;
        public float[] clipDataR;
    }
    [System.Serializable]
    public class DirectNoteData
    {
        public double noteOnTime;
        public double noteOffTime;
        public AudioClip audioClip;
        public float pitch;
        public float entryPitch;
        public float vol;
        public float vel;
        public bool loop;
        //tiestate 0 = not a tie, 1 = tue, not yet triggered, 2 = tie, has been triggered
        public int tieState;
        public bool markForDelete;
        public double currSample;
        public float envAmpVal;
    }


    [System.Serializable]
    public class QueuedClip
    {
        public AudioClip audioClip;
        public bool looping;
    }

    public class AudioDirectPlay : MonoBehaviour
    {
        public AudioClip audioClip;
        public float speed = 1;
        public float volume = 1;
        public bool antiAlias = true;
        public int clipCacheSize = 8;
        [Range(1, 6)]
        public int voiceCount = 1;

        [Header("DebugRef")]
        int deviceSampleRate;
        double bucketStartTime;
        double bucketEndTime;
        int dataLength;
        int dataChannels;
        float sampleFrac;
        int dataStartOffset;
        public float synthLastKnownPitch;

        public List<AudioClipCache> audioClipCache = new List<AudioClipCache>();
        List<DirectNoteData> noteQueue = new List<DirectNoteData>();
        public List<DirectNoteData> noteBucket = new List<DirectNoteData>();
        float[] filterVals;
        [HideInInspector] public SoundModule soundModule;
        public List<QueuedClip> queuedClipsToAdd = new List<QueuedClip>();

        float[] dataPitchVals;
        float[] dataVelVals;
        bool[] usedData;
        float[] dataIn;

        public float prevEnteredNotePitch = 1;
        bool initDSS;

        AudioSource audioSource;

        public void Awake()
        {
            audioSource = this.gameObject.GetComponent<AudioSource>();

            
        }

        void CheckDirectSpace()
        {
            if (soundModule.masterClock.FXHub.directSoundSpatialisation && !initDSS)
            {
                InitDirectSpace(true);
                return;
            }
            if (!soundModule.masterClock.FXHub.directSoundSpatialisation && audioSource.clip && initDSS)
            {
                InitDirectSpace(false);
                return;
            }
        }
        void InitDirectSpace(bool creation)
        {
            if (creation)
            {
                int len = 480;
                AudioClip clip = AudioClip.Create("offsetSilenceDummyClip", 480, 1, 48000, false);
                float[] data = new float[len];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 1;
                }
                clip.SetData(data, 0);
                audioSource.loop = true;
                audioSource.clip = clip;
                audioSource.Play();

                initDSS = true;
            }
            else
            {
                if (audioSource.clip != null)
                {
                    audioSource.clip = null;
                    initDSS = false;
                }
            }
        }
        public DirectNoteData MakeNoteData(double onTime, double offTime, AudioClip clip, float p, float vol, float vel, bool l, bool isTie, double startSampleOffset = 0)
        {
            if (clip == null)
            {
                Debug.Log("trying to build note with missing clip" + this.gameObject.name);
                //return;
            }


            DirectNoteData newNote = new DirectNoteData();
            newNote.noteOnTime = onTime;
            
                AddClipToCache(clip, l);
                newNote.audioClip = clip;
            
            newNote.pitch = p;
            newNote.entryPitch = p;
            newNote.vol = vol;
            newNote.vel = vel;
            newNote.loop = l;

            if (isTie)
            {
                newNote.tieState = 1;
                newNote.entryPitch = prevEnteredNotePitch;
            }

            newNote.markForDelete = false;
            newNote.currSample = startSampleOffset;

            if (offTime == -1)
            {
                float clipOffTimeAdd = 0;
                if (clip != null)
                {
                    clipOffTimeAdd = (float)clip.samples / clip.frequency;
                }
                if (!l)
                {
                    offTime = onTime + (clipOffTimeAdd / Mathf.Abs(p));
                }
                else
                {
                    //this should get clamped down to logical  length during note playback, but approximating end of section as duration to guard against stuck notes
                    //need to check if we're assessing a beat that will land in the next section, and adjust note length accordingly
                    float sectionFracFudge = Mathf.Max(0.1f, 1 - soundModule.masterClock.normalisedPointInSectionAssessing);
                    double maxLength = (soundModule.masterClock.beatLength * soundModule.masterClock.totalBeatsPerSection) * sectionFracFudge;
                    offTime = onTime + maxLength;
                }
            }
            newNote.noteOffTime = offTime;

            prevEnteredNotePitch = p;

            if (soundModule.masterClock.verboseDebug)
            {
                Debug.Log("submitted new direct sound note on instrument " + soundModule.name + ", pitch =  " + p);
            }

            return newNote;
        }


        void Update()
        {
            CheckDirectSpace();
            for (var i = queuedClipsToAdd.Count - 1; i >= 0; i--)
            {
                AddClipToCache(queuedClipsToAdd[i].audioClip, queuedClipsToAdd[i].looping);
                queuedClipsToAdd.RemoveAt(i);
            }
        }
        public void AddNoteToQueue(DirectNoteData note)
        {
            if (noteQueue.Count == 0)
            {
                noteQueue.Add(note);
                return;
            }

            for (var i = 0; i < noteQueue.Count; i++)
            {
                if (noteQueue[i] == null || note.noteOnTime <= noteQueue[i].noteOnTime)
                {
                    noteQueue.Insert(i, note);
                    return;
                }
            }
            noteQueue.Add(note);
        }

        public void AddClipToCache(AudioClip clip, bool loop)
        {
            for (var i = 0; i < audioClipCache.Count; i++)
            {
                if (audioClipCache[i].audioClip == clip)
                {
                    //clip already exists in cache
                    return;
                }
            }

            if (soundModule.masterClock.verboseDebug)
            {
                Debug.Log("Caching audioclip " + clip.name);
            }

            AudioClipCache clipCache = new AudioClipCache();

            clipCache.audioClip = clip;
            clipCache.clipSampleRate = clip.frequency;
            clipCache.clipChannels = clip.channels;
            clipCache.clipSamples = clip.samples;
            clipCache.clipData = new float[clip.samples * clip.channels];
            clipCache.sampleRateSpeedDiv = (float)clip.frequency / AudioSettings.outputSampleRate;

            clip.GetData(clipCache.clipData, 0);

            if (clip.channels == 2)
            {
                float[] dataL = new float[clip.samples];
                float[] dataR = new float[clip.samples];
                for (var i = 0; i < dataL.Length - 1; i++)
                {
                    dataL[i] = clipCache.clipData[i * 2];
                    dataR[i] = clipCache.clipData[(i * 2 + 1)];
                }
                clipCache.clipData = dataL;
                clipCache.clipDataR = dataR;
            }

            //if not a looping sound, taper out the final samples to remove any clicks that unity likes to add
            if (!loop)
            {
                clipCache.clipData = TaperData(clipCache.clipData);
                if (clip.channels == 2)
                {
                    clipCache.clipDataR = TaperData(clipCache.clipDataR);
                }
            }


            audioClipCache.Add(clipCache);


            if (audioClipCache.Count > clipCacheSize)
            {
                audioClipCache.RemoveAt(0);
            }
        }

        float[] TaperData(float[] data) 
        {
            int taperSamples = 48;
            int t = taperSamples;
            for (var i = data.Length - 1; i >= 0 && t > 0; i--, t--)
            {
                float taperVal = 1-((float)t / (float)taperSamples);

                data[i] *= taperVal;
            }

            return data;
        }

        void Start()
        {
            CheckDirectSpace();

            deviceSampleRate = AudioSettings.outputSampleRate;
            synthLastKnownPitch = 1;


            if (audioClip != null)
            {
                AddClipToCache(audioClip, true);
            }
        }

        static int SortByNoteOn(DirectNoteData n1, DirectNoteData n2)
        {
            return n1.noteOnTime.CompareTo(n2.noteOnTime);
        }

        int GetClipCacheIndex(AudioClip clip)
        {
            for (var i = 0; i < audioClipCache.Count; i++)
            {
                if (audioClipCache[i].audioClip == clip)
                {
                    return i;
                }
            }
            return -1;
        }

        public static double LerpDouble(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static double InverseLerpDouble(double a, double b, double value)
        {
            if (a != b)
                return (value - a) / (b - a);
            else
                return 0.0d;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            dataLength = data.Length;

            if (soundModule.masterClock.FXHub.directSoundSpatialisation)
            {
                if (dataIn == null || dataIn.Length != data.Length)
                {
                    dataIn = new float[dataLength];
                }
                for (var i = 0; i < data.Length; i++)
                {
                    dataIn[i] = data[i];
                    data[i] = 0;
                }
            }

            dataChannels = channels;
            bucketStartTime = AudioSettings.dspTime;
            bucketEndTime = bucketStartTime + (((double)data.Length / channels) / deviceSampleRate);
            float channelDiv = 1f / channels;


            


            //pop any expired notes in the bucket
            //check note queue for note entries that land within bucket
            float releasePadding = 0;
            if (soundModule.useAmpEnvelope)
            {
                if (soundModule.outputMode == SoundOutputMode.DirectSound)
                {
                    releasePadding = soundModule.envAmpRelease;
                }
            }
            for (var i = noteBucket.Count - 1; i >= 0; i--)
            {
                //Debug.Log("bucketStartTime " + bucketStartTime);
                if (noteBucket[i].noteOffTime != -1 && noteBucket[i].noteOffTime + releasePadding < bucketStartTime)
                {
                    noteBucket[i].markForDelete = true;
                }
                if (noteBucket[i].markForDelete)
                {
                    noteBucket.RemoveAt(i);
                }
            }


            for (var i = noteQueue.Count - 1; i >= 0; i--)
            {
                if (noteQueue[i] == null)
                {
                    continue;
                }
                if (noteQueue[i].noteOnTime < bucketStartTime)
                {
                    //missed note, remove from queue. this shouldnt generally happen
                    noteQueue.RemoveAt(i);
                    continue;
                }
                if (noteQueue[i].noteOnTime >= bucketStartTime && noteQueue[i].noteOnTime < bucketEndTime)
                {
                    //remove note from queue, add to bucket
                    noteBucket.Add(noteQueue[i]);
                    noteQueue.RemoveAt(i);
                }

            }


            //if the note bucket is bigger than the voice count need to massage the data: 
            //forcing voicecount to 1 for synths, hopefully will get polysynths working correctly in the future
            
            if (noteBucket.Count > voiceCount)
            {
                //sort the notes by start time 
                noteBucket.Sort(SortByNoteOn);

                //trim any overlaps
                for (var i = voiceCount; i < noteBucket.Count; i++)
                {
                    noteBucket[i - voiceCount].noteOffTime = noteBucket[i].noteOnTime;
                }
            }



            if (soundModule.outputMode == SoundOutputMode.DirectSound)
            {
                float filterFreqCurveVal = 1;
                float filterResCurveVal = 1;




                int cacheIndexTracked = 0;
                double currSampleTracked = 0;
                float legatoLength = 0.1f;
                if (noteBucket != null & noteBucket.Count > 0)
                {
                    currSampleTracked = noteBucket[0].currSample;
                }
                data = AudioFilterReadSampleBased(data, channels, releasePadding, currSampleTracked, cacheIndexTracked, legatoLength, filterFreqCurveVal, filterResCurveVal);
            }


            

          


            if (soundModule.masterClock.FXHub.directSoundSpatialisation)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] *= dataIn[i];
                }
            }
        }




        void AddClipToCacheQeue(AudioClip audioClip, bool looping)
        {
            QueuedClip queuedClip = new QueuedClip();
            queuedClip.audioClip = audioClip;
            queuedClip.looping = looping;

            queuedClipsToAdd.Add(queuedClip);
        }
        float[] AudioFilterReadSampleBased(float[] data, int channels, float releasePadding, double currSampleTracked, int cacheIndexTracked, float legatoLength, float filterFreqCurveVal, float filterResCurveVal)
        {
            //run through notes updating 
            // need to be able to mark notes for delete when they've been overriden by another note (voice count enforcing /lifetime stuff)
            for (var bucketIndex = 0; bucketIndex < noteBucket.Count; bucketIndex++)
            {
                //get clip from cache... cache miss at this point could be pretty serious, not sure what to do there
                int clipCacheIndex = GetClipCacheIndex(noteBucket[bucketIndex].audioClip);
                if (clipCacheIndex == -1)
                {
                    //cache miss !
                    //cant directly get teh clip here as it requires checking the frequency and that can only be done from the main thread
                    AddClipToCacheQeue(noteBucket[bucketIndex].audioClip, noteBucket[bucketIndex].loop);
                    continue;
                }



                //pitches the sample & accounts for differences in samplerate between clip & device
                float sampleAdvance = speed * audioClipCache[clipCacheIndex].sampleRateSpeedDiv * noteBucket[bucketIndex].pitch;

                double startSample = noteBucket[bucketIndex].currSample;


                int dataDuration = data.Length / channels;
                //accounting for notes that start part way through this audio bucket
                dataStartOffset = 0;
                if (noteBucket[bucketIndex].noteOnTime > bucketStartTime)
                {
                    float dataLerpVal = (float)InverseLerpDouble(bucketStartTime, bucketEndTime, noteBucket[bucketIndex].noteOnTime);

                    dataStartOffset = (int)Mathf.Lerp(0, dataDuration, dataLerpVal);
                    dataStartOffset *= channels;
                }

                //accounting for notes that conclude part way through this audio bucket
                int dataEndOffset = data.Length;
                double paddedNoteOffTime = noteBucket[bucketIndex].noteOffTime + (double)releasePadding;
                if (paddedNoteOffTime < bucketEndTime)
                {
                    float dataLerpVal = (float)InverseLerpDouble(bucketStartTime, bucketEndTime, paddedNoteOffTime);
                    //Debug.Log("dataLerpVal " + dataLerpVal);
                    dataEndOffset = (int)Mathf.Lerp(0, dataDuration, dataLerpVal);
                    dataEndOffset *= channels;
                }
                //Debug.Log("data end = " + dataEndOffset + " dataLength = " + data.Length);



                for (var i = dataStartOffset; i < dataEndOffset; i += channels)
                {
                    if (noteBucket[bucketIndex].markForDelete)
                    {
                        //// i think this is unneeded / nonsensical ? 
                        //for (var j = 0; j < channels; j++)
                        //{
                        //    data[i + j] = 0;
                        //}
                        //Debug.Log("skipping sample due to deletion");
                        continue;
                    }
                    //this is used for getting the envelope state. doesnt need to be super duper accurate
                    double dataTime = bucketStartTime + (i / channels / (double)deviceSampleRate);

                    //account for ties
                    if (noteBucket[bucketIndex].tieState == 1)
                    {
                        //skip bend, notes are same
                        if (noteBucket[bucketIndex].entryPitch == noteBucket[bucketIndex].pitch)
                        {
                            noteBucket[bucketIndex].tieState = 3;
                        }
                        else
                        {
                            //Debug.Log("processing tie, new pitch = " + noteBucket[bucketIndex].pitch);
                            noteBucket[bucketIndex].currSample = currSampleTracked;
                            clipCacheIndex = cacheIndexTracked;
                            noteBucket[bucketIndex].tieState = 2;

                            sampleAdvance = speed * audioClipCache[clipCacheIndex].sampleRateSpeedDiv * noteBucket[bucketIndex].entryPitch;
                        }
                    }
                    if (noteBucket[bucketIndex].tieState == 2)
                    {
                        float t = (float)(dataTime - noteBucket[bucketIndex].noteOnTime) / legatoLength;
                        float currPitch = Mathf.Lerp(noteBucket[bucketIndex].entryPitch, noteBucket[bucketIndex].pitch, Mathf.Clamp01(t));

                        sampleAdvance = speed * audioClipCache[clipCacheIndex].sampleRateSpeedDiv * currPitch;
                    }
                    ////debug output just ties
                    //if (noteBucket[bucketIndex].tieState == 0)
                    //{
                    //    continue;
                    //}

                    //account for looping or mark note for deletion if a non-looping sound has concluded
                    if (noteBucket[bucketIndex].loop)
                    {
                        noteBucket[bucketIndex].currSample = noteBucket[bucketIndex].currSample % audioClipCache[clipCacheIndex].clipSamples;
                    }
                    else
                    {
                        if (noteBucket[bucketIndex].currSample >= audioClipCache[clipCacheIndex].clipSamples)
                        {
                            noteBucket[bucketIndex].markForDelete = true;
                            continue;
                        }
                    }

                    int sampleIndex = (int)noteBucket[bucketIndex].currSample;
                    int sampleNext = sampleIndex + 1;
                    if (sampleIndex >= audioClipCache[clipCacheIndex].clipData.Length)
                    {
                        sampleIndex = audioClipCache[clipCacheIndex].clipData.Length - 1;
                    }

                    if (sampleNext >= audioClipCache[clipCacheIndex].clipData.Length)
                    {
                        sampleNext = sampleIndex;
                    }
                    sampleFrac = (float)(noteBucket[bucketIndex].currSample - sampleIndex);

                    /*
                    //checking clip cache for future bug fix
                    float testData = 0;
                    try
                    {
                        testData = audioClipCache[clipCacheIndex].clipData[sampleIndex];
                    }
                    catch
                    {
                        Debug.Log("clip cache fail module" + soundModule);
                        if (audioClipCache.Count < clipCacheIndex)
                        {
                            Debug.Log("clip cache fail cache data list too short");
                        }
                        if (audioClipCache[clipCacheIndex].clipData == null || audioClipCache[clipCacheIndex].clipData.Length == 0)
                        {
                            Debug.Log("clip cache fail cache data missing");
                        }
                        {
                            Debug.Log("clip cache fail cache data list too short");
                        }
                        Debug.Log("clip cache fail cache data " + clipCacheIndex + " " + audioClipCache.Count);
                        Debug.Log("clip cache fail sample " + sampleIndex + " " + audioClipCache[clipCacheIndex].clipData.Length);
                    }*/

                    float integratedVolume = volume * noteBucket[bucketIndex].vol * volume;
                    noteBucket[bucketIndex].envAmpVal = GetAmpEnvelopeValue(noteBucket[bucketIndex].envAmpVal, noteBucket[bucketIndex].noteOnTime, noteBucket[bucketIndex].noteOffTime, dataTime);
                    integratedVolume *= noteBucket[bucketIndex].envAmpVal;
                    if (noteBucket[bucketIndex].envAmpVal == 0)
                    {
                        noteBucket[bucketIndex].markForDelete = true;
                        continue;
                    }


                    if (dataEndOffset != data.Length && dataEndOffset - i < 1024 && Mathf.Abs(audioClipCache[clipCacheIndex].clipData[sampleIndex]) < 0.02f)
                    {
                        //Debug.Log("found suitable zero crossing exit");
                        noteBucket[bucketIndex].markForDelete = true;
                        continue;
                    }

                    //multi-channel is not handled super well here
                    // and the pannign control for surround sound is nonsense (stereo should be fine)
                    if (audioClipCache[clipCacheIndex].clipChannels == 1)
                    {
                        float sample = audioClipCache[clipCacheIndex].clipData[sampleIndex];

                        if (antiAlias)
                        {
                            sample = Mathf.Lerp(sample, audioClipCache[clipCacheIndex].clipData[sampleNext], sampleFrac);
                        }

                        float dataVal = sample * integratedVolume;
                        for (var j = 0; j < channels; j++)
                        {
                            data[i + j] += dataVal;
                        }
                    }
                    else
                    {
                        float sampleL = audioClipCache[clipCacheIndex].clipData[sampleIndex];
                        float sampleR = audioClipCache[clipCacheIndex].clipDataR[sampleIndex];

                        if (antiAlias)
                        {
                            sampleL = Mathf.Lerp(sampleL, audioClipCache[clipCacheIndex].clipData[sampleNext], sampleFrac);
                            sampleR = Mathf.Lerp(sampleR, audioClipCache[clipCacheIndex].clipDataR[sampleNext], sampleFrac);
                        }


                        for (var j = 0; j < channels; j++)
                        {
                            float channelMarker = (float)j / (float)channels;
                            if (channelMarker < 0.5f)
                            {
                                data[i + j] += sampleL * integratedVolume;
                            }
                            else
                            {
                                data[i + j] += sampleR * integratedVolume;
                            }
                        }
                    }

                    

                    currSampleTracked += sampleAdvance;
                    cacheIndexTracked = clipCacheIndex;
                    noteBucket[bucketIndex].currSample += sampleAdvance;
                }
            }

            

            return data;
        }

        float EnvToFilter(float envVal)
        {
            return 1;
        }

        float GetAmpEnvelopeValue(float incomingEnvVal, double noteOnTime, double noteOffTime, double dataTime)
        {
            if (!soundModule.useAmpEnvelope)
            {
                return 1;
            }
            //attack phase
            if (dataTime <= noteOnTime + soundModule.envAmpAttack)
            {
                float attackVal = (float)(dataTime - noteOnTime) / soundModule.envAmpAttack;
                //making attack start non-zero to make tracking sounds that have finsihed their envelope and should be marked for delete easier
                return Mathf.Lerp(0.025f, 1, attackVal);
            }
            //decay phase
            if (dataTime <= noteOnTime + soundModule.envAmpAttack + soundModule.envAmpDecay)
            {
                float decayVal = (float)(dataTime - noteOnTime + soundModule.envAmpAttack) / soundModule.envAmpDecay;
                return Mathf.Lerp(1, soundModule.envAmpSustain, decayVal);
            }
            //sustain phase
            if (dataTime <= noteOffTime)
            {
                return soundModule.envAmpSustain;
            }

            //release phase
            float releaseDelta = (1f / soundModule.envAmpRelease) / (float)deviceSampleRate;
            return Mathf.Max(0, incomingEnvVal - releaseDelta);
        }
    }
}