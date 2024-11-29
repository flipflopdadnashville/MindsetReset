using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{

    [System.Serializable]
    public struct BeatInstance
    {
        public List<float> noteFractionOffsets;
        public List<int> noteOutputs;
        public List<float> pitches;
        public List<float> velocities;
        public List<float> stepsDuration;
        public List<bool> ties;
        public List<float> clipStartOffsets;
    }

    [SerializeField]
    public enum SoundOutputMode
    {
        AudioSources,
        DirectSound,
        Synth
    }

    [SerializeField]
    public enum ClipVariationMode
    {
        None,
        RandomInnerLoop,
        RandomFullSequence,
        VelocityStack
    }


    public class SoundModule : Generator
    {
        public bool active = true;
        [HideInInspector] public AudioSource audioSource;
        public bool useInjectedSequence;
        public InjectableGenerator injectableSrc;
        [Space(10)]
        public SoundOutputMode outputMode = SoundOutputMode.DirectSound;
        [Space(10)]
        [Header("Core Properties")]
        public AudioClip audioClip;
        public AudioClip[] audioClipVariations;
        public ClipVariationMode clipVariationMode;
        public AnimationCurve clipVariationChanceCurve;
        public float pitchMultiplier = 1;
        public int pitchOffsetSemitones = 0;
        [Range(1, 24)]
        public int voiceCount = 12;
        [Range(0.1f, 16)]
        public float loopBarCount = 2;
        [Range(0, 1)]
        public float swingMultiplier = 1;
        public int velocityCurve;
        [Range(0, 1)]
        public float velocityInfluence;
        public Vector2 humaniseOffsetMilliseconds = Vector2.zero;
        public AnimationCurve densityTrimInnerLoop;
        public AnimationCurve densityTrimFullSequence;


        [Space(10)]

        //[Header("DebugRef")]
        [HideInInspector] public MasterClock masterClock;
        [HideInInspector] public float baseVolume;
        [HideInInspector] public AudioSource[] roundRobinAudio;
        [HideInInspector] public AudioDirectPlay player;
        [HideInInspector] public int robinIndex;
        [HideInInspector] public BeatInstance[] injectedBeatInstances;
        [HideInInspector] public bool init;
        [HideInInspector] public int loopBeatCount;
        [HideInInspector] public int seed;
        [HideInInspector] public float compositionMixVol = 1;
        //[HideInInspector] public float masterMixVol = 1;
        //[HideInInspector] public float masterMixPan = 0;
        

        //synthish props
        public bool useAmpEnvelope;
        [Range(0, 3)]
        public float envAmpAttack = 0;
        [Range(0, 3)]
        public float envAmpDecay = 0;
        [Range(0, 1)]
        public float envAmpSustain = 1;
        [Range(0, 3)]
        public float envAmpRelease = 0;
        [Range(0, 1)]
        public float ampEnvToFilter = 0.35f;


        void OnValidate()
        {

            SanitiseInputCore();
        }


        void Start()
        {
            SanitiseInputCore();
            SanitiseInput();
        }


        public void GenerateInit()
        {
            SanitiseInputCore();
            SanitiseInput();
            GenerateLoop();

            pendingPostProcess = true;
        }


        public virtual void GenerateLoop()
        {

        }

        AnimationCurve InitAnimCurve(AnimationCurve ac, float val, bool forceInit = false)
        {
            if (forceInit || ac == null || ac.keys.Length == 0)
            {
                ac = new AnimationCurve();
                ac.AddKey(0, val);
            }

            return ac;
        }
        public void SanitiseInputCore()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = this.gameObject.AddComponent<AudioSource>();
                }
            }


            //masterMixVol = 1;
            //masterMixPan = 0;

            loopBarCount = Mathf.Clamp(loopBarCount, 0.1f, 16);
            swingMultiplier = Mathf.Clamp01(swingMultiplier);
            pitchMultiplier = Mathf.Clamp(pitchMultiplier, -8, 8);
            pitchOffsetSemitones = Mathf.Clamp(pitchOffsetSemitones, -12, 12);


            densityTrimInnerLoop = InitAnimCurve(densityTrimInnerLoop, 1);
            densityTrimFullSequence = InitAnimCurve(densityTrimFullSequence, 1, true);


            clipVariationChanceCurve = InitAnimCurve(clipVariationChanceCurve, 1);
            clipVariationChanceCurve = MasterClock.ClampAnimCurve01(clipVariationChanceCurve);

            humaniseOffsetMilliseconds.x = Mathf.Clamp(humaniseOffsetMilliseconds.x, -50, 50);
            humaniseOffsetMilliseconds.y = Mathf.Clamp(humaniseOffsetMilliseconds.y, -50, 50);

            if (masterClock == null)
            {
                masterClock = FindObjectOfType<MasterClock>();
            }
        }

        public virtual void SanitiseInput()
        {

        }

        public float GetVolume()
        {
            return audioSource.volume;
        }

        void Awake()
        {
            masterClock = FindObjectOfType<MasterClock>();
            masterClock.processBeat.AddListener(ProcessBeat);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            if (audioSource == null)
            {
                if (masterClock.verboseDebug)
                {
                    Debug.LogWarning("Sound Module requires AudioSource component!");
                }
                this.enabled = false;
            }
            baseVolume = GetVolume();

            if (outputMode == SoundOutputMode.AudioSources)
            {
                roundRobinAudio = masterClock.InitRoundRobinAudioSources(audioSource, voiceCount);
            }
            if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
            {
                audioSource.clip = null;
                audioSource.playOnAwake = true;
                player = gameObject.AddComponent<AudioDirectPlay>();
                player.soundModule = this;
            }

            NewSeed();
        }

        void OnEnable()
        {
            if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
            {
                audioSource.enabled = false;
                audioSource.enabled = true;
            }
        }

        public static BeatInstance[] InitBeatInstances(int count)
        {
            BeatInstance[] beats = new BeatInstance[count];
            for (var i = 0; i < count; i++)
            {
                beats[i] = InitBeatInstance();
            }

            return beats;
        }
        public static BeatInstance InitBeatInstance()
        {
            BeatInstance bi = new BeatInstance();
            bi.noteFractionOffsets = new List<float>();
            bi.noteOutputs = new List<int>();
            bi.pitches = new List<float>();
            bi.velocities = new List<float>();
            bi.stepsDuration = new List<float>();
            bi.ties = new List<bool>();
            bi.clipStartOffsets = new List<float>();
            return bi;
        }

        public static BeatInstance DuplicateBeatInstance(BeatInstance src)
        {
            BeatInstance bi = new BeatInstance();

            bi.noteFractionOffsets = new List<float>(src.noteFractionOffsets);
            bi.noteOutputs = new List<int>(src.noteOutputs);
            bi.pitches = new List<float>(src.pitches);
            bi.velocities = new List<float>(src.velocities);
            bi.stepsDuration = new List<float>(src.stepsDuration);
            bi.ties = new List<bool>(src.ties);
            bi.clipStartOffsets = new List<float>(src.clipStartOffsets);

            return bi;
        }


        public static BeatInstance AddNoteToBeatInstance(BeatInstance bi, float offset, int output = 0, float pitch = 1, float velocity = 1, bool tie = false, float stepsDuration = -1.0f, float clipStartOffset = 0)
        {
            bi.noteFractionOffsets.Add(offset);
            bi.noteOutputs.Add(output);
            bi.pitches.Add(pitch);
            bi.velocities.Add(velocity);
            bi.ties.Add(tie);
            bi.stepsDuration.Add(stepsDuration);
            bi.clipStartOffsets.Add(clipStartOffset);
            return bi;
        }
        public static BeatInstance AddNoteToBeatInstanceForceMono(BeatInstance bi, float offset, int output = 0, float pitch = 1, float velocity = 1, bool tie = false, float stepsDuration = -1.0f, float clipStartOffset = 0)
        {
            if (bi.noteFractionOffsets.Contains(offset))
            {
                return bi;
            }
            bi.noteFractionOffsets.Add(offset);
            bi.noteOutputs.Add(output);
            bi.pitches.Add(pitch);
            bi.velocities.Add(velocity);
            bi.ties.Add(tie);
            bi.stepsDuration.Add(stepsDuration);
            bi.clipStartOffsets.Add(clipStartOffset);
            return bi;
        }

        public static BeatInstance AddNoteToBeatInstanceForceMonoKeepLouder(BeatInstance bi0, BeatInstance bi1, int index)
        {
            float offset = bi0.noteFractionOffsets[index];
            int output = bi0.noteOutputs[index];
            float pitch = bi0.pitches[index];
            float velocity = bi0.velocities[index];
            bool tie = bi0.ties[index];
            float stepsDuration = bi0.stepsDuration[index];
            float clipStartOffset = bi0.clipStartOffsets[index];

            return AddNoteToBeatInstanceForceMonoKeepLouder(bi1, offset, output, pitch, velocity, tie, stepsDuration, clipStartOffset);
        }
        public static BeatInstance AddNoteToBeatInstanceForceMonoKeepLouder(BeatInstance bi, float offset, int output = 0, float pitch = 1, float velocity = 1, bool tie = false, float stepsDuration = -1.0f, float clipStartOffset = 0)
        {
            if (bi.noteFractionOffsets.Contains(offset))
            {
                int index = bi.noteFractionOffsets.IndexOf(offset);
                if (bi.velocities[index] > velocity)
                {
                    return bi;
                }
                else
                {
                    RemoveNoteFromBeatInstance(bi, index);
                }
            }
            bi.noteFractionOffsets.Add(offset);
            bi.noteOutputs.Add(output);
            bi.pitches.Add(pitch);
            bi.velocities.Add(velocity);
            bi.ties.Add(tie);
            bi.stepsDuration.Add(stepsDuration);
            bi.clipStartOffsets.Add(clipStartOffset);

            return bi;
        }


        public static BeatInstance DuplicateModifyNote(BeatInstance beatSrc, int srcNoteIndex, BeatInstance beatDest, float offset = -1, float velocity = -1, float pitch = -1)
        {
            if (offset < 0)
            {
                offset = beatSrc.noteFractionOffsets[srcNoteIndex];
            }
            if (velocity < 0)
            {
                velocity = beatSrc.velocities[srcNoteIndex];
            }
            if (pitch <= 0)
            {
                pitch = beatSrc.pitches[srcNoteIndex];
            }

            AddNoteToBeatInstance(beatDest, offset, beatSrc.noteOutputs[srcNoteIndex], pitch, velocity, beatSrc.ties[srcNoteIndex], beatSrc.stepsDuration[srcNoteIndex]);

            return beatDest;
        }
        public static BeatInstance RemoveNoteFromBeatInstance(BeatInstance bi, int i)
        {
            bi.noteFractionOffsets.RemoveAt(i);
            bi.noteOutputs.RemoveAt(i);
            bi.pitches.RemoveAt(i);
            bi.velocities.RemoveAt(i);
            bi.ties.RemoveAt(i);
            bi.stepsDuration.RemoveAt(i);
            bi.clipStartOffsets.RemoveAt(i);
            return bi;
        }
        public virtual void ProcessBeat()
        {

        }

        public static bool D100(float f)
        {
            if (Random.Range(0, 100) < f)
            {
                return true;
            }
            return false;
        }



        float PitchOffsetSemitones()
        {
            return HarmonicHub.GetTranspose(pitchOffsetSemitones);
        }

        public void GetBeatsFromTimeSig()
        {
            loopBeatCount = (int)(loopBarCount * masterClock.beatsPerBar);
        }


        public BeatInstance[] TrimBeat(BeatInstance[] src, AnimationCurve trimCurve)
        {
            for (var i = 0; i < src.Length; i++)
            {
                float trimFactorLookup = (float)i / (float)src.Length;
                float trimFactor = trimCurve.Evaluate(trimFactorLookup);

                if (trimFactor < 1)
                {
                    List<int> trimIndices = new List<int>();
                    for (var j = src[i].noteOutputs.Count - 1; j >= 0; j--)
                    {
                        if (Random.Range(0.0f, 1.0f) > trimFactor)
                        {
                            trimIndices.Add(j);
                        }
                    }

                    for (var j = 0; j < trimIndices.Count; j++)
                    {
                        RemoveNoteFromBeatInstance(src[i], trimIndices[j]);
                    }
                }
            }
            return src;
        }

        public void ProcessBeatCore()
        {
            if (!ValidateBeat())
            {
                return;
            }

            BeatInstance[] beatInstancesInUse = GetSequenceSource();

            int assessingBeatIndex = masterClock.assessingBeatIndex;
            if (assessingBeatIndex < 0 || assessingBeatIndex >= beatInstancesInUse.Length)
            {
                return;
            }


            if (masterClock.verboseDebug)
            {
                Debug.Log("Processing Percussion Beat, instrument = " + this.gameObject.name + " note count = " + beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count);
            }

            double baseOffset = (masterClock.beatLength - masterClock.beatTimer);
            double fullBeatOffset = masterClock.beatLength;

            double swingMilliseconds = masterClock.baseSwingOffset * swingMultiplier;
            float halfBeatOffset = 1.0f / (masterClock.divisionsPerBeat / 2);

            for (var i = 0; i < beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count; i++)
            {
                float scrambledOffset = masterClock.ScrambleNoteOffset(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i]);
                double thisSwing = swingMilliseconds;
                if (scrambledOffset == 0 || scrambledOffset == halfBeatOffset)
                {
                    thisSwing = 0;
                }
                double thisOffset = baseOffset + thisSwing + HumaniseTiming() + (beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i] * fullBeatOffset);

                float vel = GetVelocity(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i], false, beatInstancesInUse[assessingBeatIndex].velocities[i]);
                float vol = vel * baseVolume * compositionMixVol;

                
                AudioClip currClip = GetVariationClip(beatInstancesInUse[assessingBeatIndex].noteOutputs[i], audioClipVariations, audioClip);
                

                if (currClip != null || outputMode == SoundOutputMode.Synth)
                {
                    if (outputMode == SoundOutputMode.AudioSources)
                    {
                        roundRobinAudio[robinIndex].clip = currClip;

                        roundRobinAudio[robinIndex].volume = vol;
                        robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, thisOffset, robinIndex);
                    }
                    if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
                    {
                        if (masterClock.verboseDebug && beatInstancesInUse[assessingBeatIndex].pitches[i] != 1)
                        {
                            Debug.Log("Playing pitchModed percussion note, pitch = " + beatInstancesInUse[assessingBeatIndex].pitches[i]);
                        }
                        masterClock.PlayScheduledDirectSound(this, player, currClip, thisOffset, pitchMultiplier * beatInstancesInUse[assessingBeatIndex].pitches[i], vol, vel);
                    }
                }

            }
        }

        public void ProcessBeatAudioFragment()
        {
            if (!ValidateBeat())
            {
                return;
            }

            BeatInstance[] beatInstancesInUse = GetSequenceSource();

            int assessingBeatIndex = masterClock.assessingBeatIndex;
            if (assessingBeatIndex < 0 || assessingBeatIndex >= beatInstancesInUse.Length)
            {
                return;
            }


            if (masterClock.verboseDebug)
            {
                Debug.Log("Processing Percussion Beat, instrument = " + this.gameObject.name + " note count = " + beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count);
            }

            double baseOffset = (masterClock.beatLength - masterClock.beatTimer);
            double fullBeatOffset = masterClock.beatLength;

            double swingMilliseconds = masterClock.baseSwingOffset * swingMultiplier;
            float halfBeatOffset = 1.0f / (masterClock.divisionsPerBeat / 2);

            for (var i = 0; i < beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count; i++)
            {
                float scrambledOffset = masterClock.ScrambleNoteOffset(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i]);
                double thisSwing = swingMilliseconds;
                if (scrambledOffset == 0 || scrambledOffset == halfBeatOffset)
                {
                    thisSwing = 0;
                }

                double thisOffset = baseOffset + thisSwing + HumaniseTiming() + (beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i] * fullBeatOffset);
                float vel = GetVelocity(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i], false, beatInstancesInUse[assessingBeatIndex].velocities[i]);
                float vol = vel * baseVolume * compositionMixVol;


                double endAfter = beatInstancesInUse[assessingBeatIndex].stepsDuration[i];


                if (audioClip != null)
                {
                    if (outputMode == SoundOutputMode.AudioSources)
                    {
                        roundRobinAudio[robinIndex].clip = audioClip;
                        roundRobinAudio[robinIndex].time = beatInstancesInUse[assessingBeatIndex].clipStartOffsets[i];
                        roundRobinAudio[robinIndex].volume = vol;

                        robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, thisOffset, robinIndex, endAfter);
                    }
                    if (outputMode == SoundOutputMode.DirectSound)
                    {
                        double clipStartOffsetSamples = (beatInstancesInUse[assessingBeatIndex].clipStartOffsets[i]) * audioClip.frequency;

                        masterClock.PlayScheduledDirectSound(this, player, audioClip, thisOffset, pitchMultiplier * PitchOffsetSemitones(), vol, vel, false, endAfter, false, false, clipStartOffsetSamples);
                    }
                }
            }
        }

        BeatInstance[] GetSequenceSource()
        {
            if (injectableSrc != null)
            {
                injectedBeatInstances = injectableSrc.beatInstances;
            }
            if (useInjectedSequence && injectedBeatInstances != null && injectedBeatInstances.Length != 0)
            {
                ValidateInjectedSequence();
                return injectedBeatInstances;
            }

            return beatInstances;
        }

        bool ValidateBeat()
        {
            if (!active)
            {
                return false;
            }
            if (!gameObject.activeSelf)
            {
                return false;
            }

            if (beatInstances == null || beatInstances.Length == 0)
            {
                return false;
            }

            return true;
        }

        void ValidateInjectedSequence()
        {
            if (injectedBeatInstances == null)
            {
                return;
            }

            if (injectedBeatInstances.Length == 0)
            {
                return;
            }

            if (injectedBeatInstances.Length == masterClock.totalBeatsPerSection)
            {
                return;
            }

            BeatInstance[] beatInstancesSeqLength = new BeatInstance[masterClock.totalBeatsPerSection];
            for (var i = 0; i < beatInstancesSeqLength.Length; i++)
            {
                beatInstancesSeqLength[i] = SoundModule.DuplicateBeatInstance(injectedBeatInstances[i % injectedBeatInstances.Length]);
            }

            injectedBeatInstances = beatInstancesSeqLength;
        }

        public void ProcessBeatPitched(bool forceMono = false, bool loopSound = false, float transpose = 1)
        {
            if (!ValidateBeat())
            {
                return;
            }

            BeatInstance[] beatInstancesInUse = GetSequenceSource();

            if (loopSound == true)
            {
                forceMono = true;
            }
            int assessingBeatIndex = masterClock.assessingBeatIndex;


            if (masterClock.verboseDebug)
            {
                Debug.Log("Processing Pitched Beat, instrument = " + this.gameObject.name + " note count = " + beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count);
            }

            double baseOffset = (masterClock.beatLength - masterClock.beatTimer);
            double fullBeatOffset = masterClock.beatLength;

            double swingMilliseconds = masterClock.baseSwingOffset * swingMultiplier;
            float halfBeatOffset = 1.0f / (masterClock.divisionsPerBeat / 2);

            for (var i = 0; i < beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count; i++)
            {
                float scrambledOffset = masterClock.ScrambleNoteOffset(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i]);

                double thisSwing = swingMilliseconds;
                if (scrambledOffset == 0 || scrambledOffset == halfBeatOffset)
                {
                    thisSwing = 0;
                }


                if (audioClip != null || outputMode == SoundOutputMode.Synth)
                {
                    double thisOffset = baseOffset + thisSwing + HumaniseTiming() + (scrambledOffset * fullBeatOffset);

                    float vel = GetVelocity(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i], false, beatInstancesInUse[assessingBeatIndex].velocities[i]);
                    float vol = vel * baseVolume * compositionMixVol;

                    float pitch = beatInstancesInUse[assessingBeatIndex].pitches[i] * transpose;


                    if (audioClip != null)
                    {
                        AudioClip currClip = GetVariationClip(beatInstancesInUse[assessingBeatIndex].noteOutputs[i], audioClipVariations, audioClip);

                        if (outputMode == SoundOutputMode.AudioSources)
                        {
                            if (masterClock.verboseDebug)
                            {
                                Debug.Log("setting instrument pitch, " + roundRobinAudio[robinIndex].pitch);
                            }

                            roundRobinAudio[robinIndex].clip = currClip;

                            roundRobinAudio[robinIndex].volume = vol;
                            roundRobinAudio[robinIndex].pitch = pitch * pitchMultiplier;
                            roundRobinAudio[robinIndex].loop = loopSound;

                            robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, thisOffset, robinIndex, -1, forceMono);
                        }
                        if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
                        {
                            double endAfter = -1;
                            if (loopSound && player.voiceCount > 1)
                            {
                                endAfter = GetNoteLength(beatInstancesInUse, i, beatInstancesInUse[assessingBeatIndex].noteFractionOffsets);
                            }
                            masterClock.PlayScheduledDirectSound(this, player, currClip, thisOffset, pitch * pitchMultiplier, vol, vel, loopSound, endAfter);
                        }
                    }
                }
            }
        }

        //need to figure otut an end time for this sound, that allows poly voices but doesnt let notes drone on afte they should
        //check future beats if required
        double GetNoteLength(BeatInstance[] beatInstancesInUse, int offsetIndex, List<float> noteOffsets)
        {
            //check local beat for future notes as quick out
            if (noteOffsets.Count > offsetIndex + 1)
            {
                for (var i = offsetIndex + 1; i < noteOffsets.Count; i++)
                {
                    if (noteOffsets[i] > noteOffsets[offsetIndex])
                    {
                        float offsetDiff = noteOffsets[i] - noteOffsets[offsetIndex];
                        return offsetDiff * masterClock.beatLength;
                    }
                }
            }
            for (var i = masterClock.assessingBeatIndex + 1; i < beatInstancesInUse.Length; i++)
            {
                if (beatInstancesInUse[i].noteFractionOffsets == null || beatInstancesInUse[i].noteFractionOffsets.Count == 0)
                {
                    continue;
                }
                int beatDiff = i - masterClock.assessingBeatIndex;
                float offsetDiff = beatInstancesInUse[i].noteFractionOffsets[0] - noteOffsets[offsetIndex];

                return (beatDiff + offsetDiff) * masterClock.beatLength;
            }

            //if no cut off note found, snip at end of sequence
            return (masterClock.totalBeatsPerSection - masterClock.sectionCounter) * masterClock.beatLength;
        }


        public void ProcessBeatClipSelect(AudioClip[] clips)
        {
            if (!ValidateBeat())
            {
                return;
            }

            BeatInstance[] beatInstancesInUse = GetSequenceSource();

            if (clips.Length == 0 && outputMode != SoundOutputMode.Synth)
            {
                return;
            }

            int assessingBeatIndex = masterClock.assessingBeatIndex;


            double baseOffset = (masterClock.beatLength - masterClock.beatTimer);
            double fullBeatOffset = masterClock.beatLength;

            double swingMilliseconds = masterClock.baseSwingOffset * swingMultiplier;
            float halfBeatOffset = 1.0f / (masterClock.divisionsPerBeat / 2);

            if (masterClock.verboseDebug)
            {
                Debug.Log("Processing multi=clip beat, instrument = " + this.gameObject.name + " note count = " + beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count + " clip count = " + clips.Length);
            }

            for (var i = 0; i < beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count; i++)
            {
                int clipIndex = 0;
                if (outputMode != SoundOutputMode.Synth)
                {
                    clipIndex = (int)Mathf.Repeat(beatInstancesInUse[assessingBeatIndex].noteOutputs[i], clips.Length);
                    if (clips[clipIndex] == null)
                    {
                        if (masterClock.verboseDebug)
                        {
                            Debug.Log("Skipping trigger due to empty clip in multi clip instrument");
                        }
                        continue;
                    }
                }
                float scrambledOffset = masterClock.ScrambleNoteOffset(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i]);

                double thisSwing = swingMilliseconds;
                if (scrambledOffset == 0 || scrambledOffset == halfBeatOffset)
                {
                    thisSwing = 0;
                }

                double thisOffset = baseOffset + thisSwing + HumaniseTiming() + (scrambledOffset * fullBeatOffset);
                float vel = GetVelocity(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i], false, beatInstancesInUse[assessingBeatIndex].velocities[i]);
                float vol = vel * baseVolume * compositionMixVol;

                if (outputMode == SoundOutputMode.AudioSources)
                {
                    roundRobinAudio[robinIndex].clip = clips[clipIndex];
                    roundRobinAudio[robinIndex].pitch = pitchMultiplier * PitchOffsetSemitones();

                    roundRobinAudio[robinIndex].volume = vol;

                    robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, thisOffset, robinIndex);
                }
                if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
                {
                    masterClock.PlayScheduledDirectSound(this, player, clips[clipIndex], thisOffset, beatInstancesInUse[assessingBeatIndex].pitches[i], vol, vel, false, -1);
                }
            }
        }

        public float GetVelocity(float beatOffset, bool includeBaseVolume, float noteVelocity)
        {
            float velocity = 1;
            if (includeBaseVolume)
            {
                velocity = baseVolume;
            }

            if (velocityInfluence == 0)
            {
                return velocity;
            }

            AnimationCurve veloCurve = masterClock.velocityCurve;
            velocity *= Mathf.Lerp(1, veloCurve.Evaluate(masterClock.pointInBarNextBeat + (beatOffset / masterClock.divisionsPerBeat)), velocityInfluence);

            if (masterClock.verboseDebug)
            {
                Debug.Log("outputting velocity " + velocity);
            }
            velocity *= noteVelocity;

            //applying pow curve to velocity output to make effect stronger
            velocity = Mathf.Pow(velocity, 1.5f);
            return velocity;
        }

        public float HumaniseTiming()
        {
            return Random.Range(humaniseOffsetMilliseconds.x, humaniseOffsetMilliseconds.y) / 1000;
        }

        public void FullLengthBeatWithFills(BeatInstance[] srcBeat, float fillIntensity)
        {
            if (beatInstances == null || beatInstances.Length != masterClock.totalBeatsPerSection)
            {
                beatInstances = new BeatInstance[masterClock.totalBeatsPerSection];
            }
            int beatDivisions = masterClock.divisionsPerBeat;
            for (var i = 0; i < beatInstances.Length; i++)
            {
                beatInstances[i] = DuplicateBeatInstance(srcBeat[i % loopBeatCount]);

                //fills
                if (masterClock.fillbeats.Contains(i))
                {
                    float globalFillIntensity = (float)i / (float)beatInstances.Length;
                    float thisBeatFillIntensity = fillIntensity * globalFillIntensity;
                    if (thisBeatFillIntensity > 0)
                    {
                        int lowNoteAdds = 0;
                        int highNoteAdds = 2;
                        if (globalFillIntensity > 0.75f)
                        {
                            lowNoteAdds += 1;
                            highNoteAdds += 1;
                        }
                        if (masterClock.fillbeats.Contains(i - 1))
                        {
                            lowNoteAdds += 1;
                            highNoteAdds += 1;
                        }


                        int newNoteCount = Random.Range(lowNoteAdds, highNoteAdds);

                        for (var j = 0; j < newNoteCount; j++)
                        {
                            float randomOffset = (float)(Random.Range(0, beatDivisions)) / beatDivisions;
                            beatInstances[i] = AddNoteToBeatInstance(beatInstances[i], randomOffset);
                        }
                    }
                }
            }

            if (masterClock.verboseDebug)
            {
                Debug.Log("Generated Full Length Beat w/ fills, instrument = " + this.gameObject.name);
            }
        }



        public void ProcessBeat3x3(AudioClip[] bass, AudioClip[] octave, float transpose = 0)
        {
            if (!ValidateBeat())
            {
                return;
            }

            BeatInstance[] beatInstancesInUse = GetSequenceSource();


            int assessingBeatIndex = masterClock.assessingBeatIndex;


            //List<float> durations = beatInstancesInUse[assessingBeatIndex].stepDurations;

            if (masterClock.verboseDebug)
            {
                Debug.Log("Processing 3x3 Beat, instrument = " + this.gameObject.name + " note count = " + beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count
                + " ties = " + beatInstancesInUse[assessingBeatIndex].ties.Count);
            }

            double baseOffset = (masterClock.beatLength - masterClock.beatTimer);
            double fullBeatOffset = masterClock.beatLength;

            double swingMilliseconds = masterClock.baseSwingOffset * swingMultiplier;
            float halfBeatOffset = 1.0f / (masterClock.divisionsPerBeat / 2);

            int lastOctave = 0;
            for (var i = 0; i < beatInstancesInUse[assessingBeatIndex].noteFractionOffsets.Count; i++)
            {
                float scrambledOffset = masterClock.ScrambleNoteOffset(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i]);

                double thisSwing = swingMilliseconds;
                if (scrambledOffset == 0 || scrambledOffset == halfBeatOffset)
                {
                    thisSwing = 0;
                }


                double thisOffset = baseOffset + thisSwing + HumaniseTiming() + (scrambledOffset * fullBeatOffset);
                float vel = GetVelocity(beatInstancesInUse[assessingBeatIndex].noteFractionOffsets[i], false, beatInstancesInUse[assessingBeatIndex].velocities[i]);
                float vol = baseVolume * compositionMixVol;


                if (masterClock.verboseDebug)
                {
                    Debug.Log("303 pitch = " + beatInstancesInUse[assessingBeatIndex].pitches[i]);
                }
                AudioClip[] clipSet = bass;
                if (beatInstancesInUse[assessingBeatIndex].noteOutputs[i] > 0)
                {
                    clipSet = octave;
                }
                int clipIndex = 0;
                if (vel >= 0.5f)
                {
                    clipIndex = 1;
                }

                float targetPitch = beatInstancesInUse[assessingBeatIndex].pitches[i] * transpose * pitchMultiplier;

                double endAfter = beatInstancesInUse[assessingBeatIndex].stepsDuration[i] * masterClock.stepLength;

                if (outputMode == SoundOutputMode.AudioSources)
                {
                    roundRobinAudio[robinIndex].clip = clipSet[clipIndex];
                    roundRobinAudio[robinIndex].volume = vol;
                    if (!beatInstancesInUse[assessingBeatIndex].ties[i])
                    {
                        roundRobinAudio[robinIndex].pitch = targetPitch;
                        if (masterClock.verboseDebug)
                        {
                            Debug.Log("setting 3x3 pitch, " + targetPitch);
                        }

                        robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, thisOffset, robinIndex, endAfter);
                    }
                    else
                    {
                        int targetAudioIndex = (int)Mathf.Repeat(robinIndex - 1, roundRobinAudio.Length);

                        if (masterClock.verboseDebug)
                        {
                            Debug.Log("setting 3x3 TIED pitch, " + roundRobinAudio[robinIndex].pitch);
                        }

                        float octaveDiff = 0;
                        if (beatInstancesInUse[assessingBeatIndex].noteOutputs[i] < lastOctave)
                        {
                            targetPitch *= 0.5f;
                        }
                        targetPitch = octaveDiff + targetPitch;
                        masterClock.ScheduleNoteArticulationPitch(roundRobinAudio[targetAudioIndex], targetPitch, (float)thisOffset);
                    }
                }

                
                if (outputMode == SoundOutputMode.DirectSound || outputMode == SoundOutputMode.Synth)
                {
                    if (outputMode == SoundOutputMode.Synth)
                    {
                        vel *= clipIndex * 0.5f;
                        targetPitch *= beatInstancesInUse[assessingBeatIndex].noteOutputs[i] + 1;
                    }
                    if (!beatInstancesInUse[assessingBeatIndex].ties[i])
                    {
                        if (masterClock.verboseDebug)
                        {
                            Debug.Log("setting 3x3 pitch, " + targetPitch);
                        }

                        masterClock.PlayScheduledDirectSound(this, player, clipSet[clipIndex], thisOffset, targetPitch, vol, vel, false, endAfter);
                    }
                    else
                    {
                        if (masterClock.verboseDebug)
                        {
                            Debug.Log("setting 3x3 TIED pitch, " + targetPitch);
                        }

                        //float octaveDiff = 0;
                        if (outputMode != SoundOutputMode.Synth && beatInstancesInUse[assessingBeatIndex].noteOutputs[i] < lastOctave)
                        {
                            targetPitch *= 0.5f;
                        }

                        masterClock.PlayScheduledDirectSound(this, player, clipSet[clipIndex], thisOffset, targetPitch, vol, vel, false, endAfter, true, true);
                    }
                }

                lastOctave = beatInstancesInUse[assessingBeatIndex].noteOutputs[i];
            }
        }

        public float[] GetPitches(BassPitchLogic logic, HarmonicHub hh)
        {
            float thisPitchOffset = PitchOffsetSemitones();
            if (logic == BassPitchLogic.Spam157)
            {
                float[] pitches = new float[3];

                pitches[0] = FoldHiPitches(hh.pitches[0]) * thisPitchOffset;
                pitches[1] = FoldHiPitches(hh.pitches[4]) * thisPitchOffset;
                pitches[2] = FoldHiPitches(hh.pitches[6]) * thisPitchOffset;

                return pitches;
            }

            if (logic == BassPitchLogic.PentatonicSpam)
            {
                float[] pitches = new float[5];

                pitches[0] = FoldHiPitches(hh.pitches[0]) * thisPitchOffset;
                pitches[1] = FoldHiPitches(hh.pitches[1]) * thisPitchOffset;
                pitches[2] = FoldHiPitches(hh.pitches[2]) * thisPitchOffset;
                pitches[3] = FoldHiPitches(hh.pitches[4]) * thisPitchOffset;
                pitches[4] = FoldHiPitches(hh.pitches[5]) * thisPitchOffset;

                return pitches;
            }
            if (masterClock.verboseDebug)
            {
                Debug.Log("pitches array will be empty");
            }
            return new float[0];
        }

        public float FoldHiPitches(float pitch, float targetVal = 1.5f)
        {
            if (pitch > targetVal)
            {
                if (masterClock.verboseDebug)
                {
                    Debug.Log("Folding over pitched note");
                }
                pitch *= 0.5f;
            }
            return pitch;
        }

        public void NewSeed(int i = 0)
        {
            seed = (int)((Time.realtimeSinceStartup * 107013) + AudioSettings.dspTime + (i * 13));
            if (masterClock.verboseDebug)
            {
                Debug.Log("Fetching new random seed, seed = " + seed);
            }
            Random.InitState(seed);
        }
        public void InitSeed(int s = -1)
        {
            if (s > 0)
            {
                Random.InitState(s);
            }
            else
            {
                Random.InitState(seed);
            }
        }

        public void StopAudio()
        {
            for (var i = 0; i < roundRobinAudio.Length; i++)
            {
                roundRobinAudio[i].Stop();
            }
        }


        public Vector2 ValidateRhythmDensity(Vector2 rhythDensity)
        {
            rhythDensity.x = Mathf.Min(Mathf.Max(-16, rhythDensity.x), 16);
            rhythDensity.y = Mathf.Min(Mathf.Max(-16, rhythDensity.y), 16);

            return rhythDensity;
        }
        public float ValidateSnareStart(float snareStart)
        {
            snareStart = Mathf.Round(snareStart * 2) / 2;
            snareStart = Mathf.Clamp(snareStart, 0, 16);
            return snareStart;
        }
        public int[] ValidateRepeatStack(int[] repeatStack, int baseVal = 8)
        {
            if (repeatStack == null || repeatStack.Length == 0)
            {
                repeatStack = new int[1];
                repeatStack[0] = baseVal;
            }

            for (var i = 0; i < repeatStack.Length; i++)
            {
                repeatStack[i] = Mathf.Min(Mathf.Max(repeatStack[i], 1), 32);
            }

            return repeatStack;
        }

        //extend loop to fill sequence
        public void FullSeqBeatInstances(BeatInstance[] InnerLoop, int srcLoopLengthBeats)
        {
            if (beatInstances == null || beatInstances.Length != masterClock.totalBeatsPerSection)
            {
                beatInstances = new BeatInstance[masterClock.totalBeatsPerSection];
            }
            for (var i = 0; i < beatInstances.Length; i++)
            {
                beatInstances[i] = DuplicateBeatInstance(InnerLoop[i % srcLoopLengthBeats]);
            }

            beatInstances = TrimBeat(beatInstances, densityTrimFullSequence);
        }

        bool UsingClipVariations()
        {
            if (clipVariationMode == ClipVariationMode.None || audioClipVariations == null || audioClipVariations.Length == 0)
            {
                return false;
            }
            return true;
        }
        public BeatInstance[] IntegrateClipVariations(BeatInstance[] beats, float fillIntensity, bool isInnerLoop)
        {
            if (!UsingClipVariations())
            {
                return beats;
            }

            if (masterClock.verboseDebug)
            {
                Debug.Log("Asessing ClipVariations, " + this.gameObject.name + ", isInnerLoop = " + isInnerLoop + ", clipVariationMode = " + clipVariationMode + ", beat count = " + beats.Length);
            }
            bool applyLoopRandom = false;
            if (clipVariationMode == ClipVariationMode.RandomInnerLoop && isInnerLoop)
            {
                applyLoopRandom = true;
            }
            if (clipVariationMode == ClipVariationMode.RandomFullSequence && !isInnerLoop)
            {
                applyLoopRandom = true;
            }
            if (applyLoopRandom)
            {
                for (var i = 0; i < beats.Length; i++)
                {
                    float pointInCurve = (float)i/(float)beats.Length;
                    float varChance = clipVariationChanceCurve.Evaluate(pointInCurve) * 100;

                    if (masterClock.verboseDebug)
                    {
                        Debug.Log("ClipVariation chance = " + varChance);
                    }

                    beats[i] = RandomOutput(beats[i], varChance);
                }
                return beats;
            }

            if (!isInnerLoop && fillIntensity > 0 && clipVariationMode != ClipVariationMode.VelocityStack)
            {
                for (var i = 0; i < beats.Length; i++)
                {
                    if (masterClock.fillbeats.Contains(i))
                    {
                        beats[i] = RandomOutput(beats[i], fillIntensity);
                    }
                }
            }

            if (!isInnerLoop && clipVariationMode == ClipVariationMode.VelocityStack) 
            { 
                for (var i = 0; i < beats.Length; i++)
                {
                    for (var j = 0; j < beats[i].velocities.Count; i++)
                    {
                        int velocityToIndex = (int)((1-beats[i].velocities[j]) * (audioClipVariations.Length - 1));
                        beats[i].noteOutputs[j] = velocityToIndex;
                    }
                }
            }

            return beats;
        }
        BeatInstance RandomOutput(BeatInstance beat, float chancePercent)
        {
            for (var j = 0; j < beat.noteOutputs.Count; j++)
            {
                if (D100(chancePercent))
                {
                    int maxOutput = audioClipVariations.Length;
                    int outputIndex = Random.Range(0, maxOutput) + 1;
                    beat.noteOutputs[j] = outputIndex;

                    if (masterClock.verboseDebug)
                    {
                        Debug.Log("Creating ClipVariation, output = " + outputIndex + " clips available = " + maxOutput);
                    }
                }
            }

            return beat;
        }

        public float FindNextNoteAddress(BeatInstance[] beats, int index, float offset)
        {
            for (var i = index; i < beats.Length; i++)
            {
                for (var j = 0; j < beats[i].noteFractionOffsets.Count; j++)
                {
                    if (i == index && beats[i].noteFractionOffsets[j] <= offset)
                    {
                        continue;
                    }

                    return i + beats[i].noteFractionOffsets[j];
                }
            }

            return -1;
        }
        public float GetStepsCountBetweenNotes(float note0, float note1, float beatCount)
        {
            //get the step count to end of sequence, return as fail safe
            if (note1 < 0)
            {
                return (note0 * masterClock.divisionsPerBeat) / beatCount;
            }

            return (note1-note0) * masterClock.divisionsPerBeat;
        }

        public float BeatNoteOffsetToFloat(int index, float offset)
        {
            return index + offset;
        }

        AudioClip GetVariationClip(int output, AudioClip[] clipSet, AudioClip fallbackClip)
        {
            if (output == 0 || !UsingClipVariations())
            {
                return fallbackClip;
            }

            int varIndex = output - 1;
            if (clipSet.Length > varIndex && clipSet[varIndex] != null)
            {
                return clipSet[varIndex];
            }

            return fallbackClip;
        }
    }
}