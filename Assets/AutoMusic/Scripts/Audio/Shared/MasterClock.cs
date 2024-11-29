using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using AutoMusic;

namespace AutoMusic
{
    [System.Serializable]
    public class DSPMusicTime
    {
        public int stepIndex;
        public double stepRemainder;
        public double remainderincrement;
    }
    public class MasterClock : MonoBehaviour
    {
        [Header("System Settings")]
        [Tooltip("See documentation for how this affects performance/output : if in doubt, leave at the default value")]
        [HideInInspector] public float globalLatency = 0.1f;
        [Tooltip("The Audio Mixer Settings Snapshot to load for this systme")]
        public AudioMixerSnapshot audioMixerSnapshot;
        [Tooltip("The FXHub component should be added to your system for full functionality")]
        public FXHub FXHub;
        [Header("Musical Settings")]
        public float bpm = 124;
        [Tooltip("The amount of swing/shuffle feel to add to 1/16th notes. Ranges from completely straight to far too much. Can be modified in real-time")]
        [Range(0, 0.99f)]
        public float swing = 0.0f;
        [Tooltip("How many time divisions between each beat… typical values are 4 or 3.")]
        [Range(3, 4)]
        [HideInInspector] public int divisionsPerBeat = 4;
        [Tooltip("The time signature of the sequence")]
        [Range(3, 7)]
        public int beatsPerBar = 4;
        [Tooltip("Number of bars in each generated sequence.")]
        [Range(2, 16)]
        [HideInInspector] public int barsPerSection = 8;
        [Tooltip("How many times to repeat the sequence before considering regenerating something new")]
        [Range(1, 4)]
        [HideInInspector] public int repeatsPerSection = 2;
        [Tooltip("Many of the Sound Modules implement a ‘fills’ control, where every n number of bars rhythmic & pitch variation is added to the sequence.")]
        [Range(1, 16)]
        [HideInInspector] public int fillsEveryNthBar = 4;

        //public AudioSource metronome;
        //public int metronomeVoices = 8;

        [Tooltip("These curves can be assigned (in variable quantities) to the instruments, modulating the strength of notes depending on where they land in musical time. The curve loops every bar.")]
        public AnimationCurve velocityCurve;

        [Space(10)]
        public bool forceRegenInstant;
        [Tooltip("Allow the system to auto-regen fresh sequences. Disable to lock to current sequence")]
        public bool forceRegenOnLoop = true;

        //[Header("DebugRef")]
        [HideInInspector] public double beatLength;
        [HideInInspector] public double barLength;
        [HideInInspector] public int totalBeatsPerSection;
        [HideInInspector] public double beatTimer;
        [HideInInspector] public int barCounter;
        [HideInInspector] public int sectionCounter;
        [HideInInspector] public int stepCounter;
        [HideInInspector] public double stepLength;
        [HideInInspector] public double stepTimer;
        [HideInInspector] public float normalisedPointInSection;
        [HideInInspector] public float normalisedPointInSectionAssessing;
        [HideInInspector] public AudioSource[] roundRobinAudio;
        [HideInInspector] public int robinIndex;
        [HideInInspector] public double baseSwingOffset;
        [HideInInspector] public float pointInBar;
        [HideInInspector] public float pointInBarNextBeat;
        [HideInInspector] public List<int> fillbeats = new List<int>();

        [HideInInspector] public UnityEvent processBeat;
        [HideInInspector] public int assessingBeatIndex;
        [HideInInspector] public bool sectionCommenced;
        [HideInInspector] public int sectionCounterRaw;
        [HideInInspector] public double prevBeatDspTime;
        [HideInInspector] public double nextBeatStart;
        


        [HideInInspector] public int beatAssessOffset = 1;
        [Space(10)]
        public bool verboseDebug;

        [HideInInspector] public CompositionHub ch;
        HarmonicHub hh;
        [HideInInspector] public int sampleRate;

        private void OnValidate()
        {
            beatLength = 60.0d / bpm;
            totalBeatsPerSection = beatsPerBar * barsPerSection * repeatsPerSection;

            divisionsPerBeat = 4;

            if (velocityCurve == null)
            {
                velocityCurve = new AnimationCurve();
                velocityCurve = new AnimationCurve();
                velocityCurve.AddKey(0, 1);
            }
        }


        void ValidateFXHub()
        {
            if (FXHub == null)
            {
                FXHub = GameObject.FindObjectOfType<FXHub>();

                if (FXHub == null)
                {
                    GameObject fxHubObj = new GameObject("FXHub");
                    fxHubObj.transform.parent = this.transform.parent;
                    fxHubObj.transform.localPosition = Vector3.zero;
                    fxHubObj.transform.localRotation = Quaternion.identity;
                    FXHub = fxHubObj.AddComponent<FXHub>();
                }
            }
        }




        void Update()
        {
            RunClock();
            if (forceRegenInstant)
            {
                forceRegenInstant = false;
                RegenDevices(true);
            }
        }

        private void Awake()
        {
            ValidateFXHub();
        }
        public void Start()
        {
            //if (metronome != null)
            //{
            //    roundRobinAudio = InitRoundRobinAudioSources(metronome, metronomeVoices);
            //}
            

            sampleRate = AudioSettings.outputSampleRate;
            audioMixerSnapshot.TransitionTo(0);


            Init(-3);
        }


        void OnEnable()
        {
            prevBeatDspTime = AudioSettings.dspTime + beatLength;
        }

        public void Init(int offsetCounts = -1)
        {
            sectionCounter = offsetCounts;
            sectionCounterRaw = offsetCounts;
            barCounter = offsetCounts;
            sectionCommenced = false;

            totalBeatsPerSection = beatsPerBar * barsPerSection * repeatsPerSection;
            ConfigFillBeat();
        }

        public AudioSource[] InitRoundRobinAudioSources(AudioSource audioSrc, int voices)
        {
            AudioSource[] rra = new AudioSource[voices];
            for (var i = 0; i < voices; i++)
            {
                rra[i] = audioSrc.gameObject.AddComponent<AudioSource>();
                rra[i].volume = audioSrc.volume;
                rra[i].clip = audioSrc.clip;
                rra[i].outputAudioMixerGroup = audioSrc.outputAudioMixerGroup;
                rra[i].pitch = audioSrc.pitch;
                rra[i].playOnAwake = false;

                rra[i].spatialBlend = audioSrc.spatialBlend;
                rra[i].rolloffMode = audioSrc.rolloffMode;
                rra[i].maxDistance = audioSrc.maxDistance;
            }

            return rra;
        }


        public void RegenDevices(bool fullRegen = false)
        {
            if (verboseDebug)
            {
                Debug.Log("Generating Sequences, full regen = " + fullRegen);
            }

            if (ch == null)
            {
                ch = FindObjectOfType<CompositionHub>();
            }
            float seedStability = 75;
            if (ch != null)
            {
                ch.GenerateComposition();
            }

            if (fullRegen)
            {
                seedStability = 0;
            }

            Init();

            InjectableGenerator[] ig = FindObjectsOfType<InjectableGenerator>();
            for (var i = 0; i < ig.Length; i++)
            {
                if (ig[i].manualRegenOnly)
                {
                    continue;
                }
                ig[i].GenerateInjectable();
            }

            if (ch != null && ch.requestHarmonicChange)
            {
                if (hh == null)
                {
                    hh = FindObjectOfType<HarmonicHub>();
                }
                hh.GenerateSequence();
            }


            SoundModule[] sm = FindObjectsOfType<SoundModule>();
            for (var i = 0; i < sm.Length; i++)
            {
                sm[i].init = false;
                if (!SoundModule.D100(seedStability))
                {
                    sm[i].NewSeed(i);
                }
                sm[i].InitSeed();
            }
        }



        void RunClock()
        {
            bpm = Mathf.Max(bpm, 1);
            beatLength = 60.0d / bpm;
            barLength = beatLength * beatsPerBar;
            stepLength = beatLength / divisionsPerBeat;


            beatTimer += Time.unscaledDeltaTime;
            stepTimer += Time.unscaledDeltaTime;

            baseSwingOffset = (beatLength / divisionsPerBeat) * swing * 0.5f;

            if (stepTimer > stepLength)
            {
                stepTimer = RepeatDouble(stepTimer, stepLength);
            }

            if (beatTimer >= beatLength)
            {
                barCounter += 1;
                sectionCounter += 1;
                sectionCounterRaw += 1;
                beatTimer = RepeatDouble(beatTimer, beatLength);


                Beat();
            }

            barCounter = (int)Mathf.Repeat(barCounter, beatsPerBar);

            sectionCounter = (int)Mathf.Repeat(sectionCounter, totalBeatsPerSection);

            pointInBar = (float)(((barCounter * beatLength) + beatTimer) / barLength);
            pointInBarNextBeat = Mathf.Repeat(pointInBar + 1.0f / beatsPerBar, 1.0f);

            normalisedPointInSection = ((float)sectionCounter / (float)totalBeatsPerSection) + ((float)((beatTimer / beatLength)) / totalBeatsPerSection);
            normalisedPointInSection = Mathf.Repeat(normalisedPointInSection, 1);

            
            normalisedPointInSectionAssessing = ((float)assessingBeatIndex / (float)totalBeatsPerSection) + ((float)((beatTimer / beatLength)) / totalBeatsPerSection);
            normalisedPointInSectionAssessing = Mathf.Repeat(normalisedPointInSectionAssessing, 1);

            stepCounter = (sectionCounter * divisionsPerBeat)
                 + (int)((beatTimer / beatLength) * divisionsPerBeat);
        }


        void ConfigFillBeat()
        {
            //fills can happen for the last 2 beats in every x bar chunk
            //intensity of fills should ramp over the total section length
            int fillBeatNumber = totalBeatsPerSection / beatsPerBar / fillsEveryNthBar;
            fillbeats = new List<int>();
            int fillBeatSpacing = fillsEveryNthBar * beatsPerBar;

            for (var i = 0; i < fillBeatNumber; i++)
            {
                int fillBeat = fillBeatSpacing * (i + 1);
                fillbeats.Add(fillBeat - 2);
                fillbeats.Add(fillBeat - 1);
            }
        }

        public DSPMusicTime GetDSPMusicTime(double inputTime, DSPMusicTime dspMusicTime)
        {
            if (dspMusicTime == null)
            {
                dspMusicTime = new DSPMusicTime();
            }

            double beatsOffsetRaw = inputTime - nextBeatStart;

            double StepLength = beatLength / divisionsPerBeat;

            double stepsOffsetRaw = beatsOffsetRaw / stepLength;

            int stepsOffset = Mathf.FloorToInt((float)stepsOffsetRaw);
            double stepRemainder = stepsOffsetRaw - stepsOffset;

            dspMusicTime.stepIndex = (assessingBeatIndex * divisionsPerBeat) + stepsOffset;
            dspMusicTime.stepIndex = (int)Mathf.Repeat(dspMusicTime.stepIndex, totalBeatsPerSection * divisionsPerBeat);
            dspMusicTime.stepRemainder = stepRemainder;
            dspMusicTime.remainderincrement = StepLength / (double)sampleRate;

            return dspMusicTime;
        }

        public DSPMusicTime IncrementDSPMusicTime(DSPMusicTime dspMusicTime, int downSampleFactor)
        {
            dspMusicTime.stepRemainder += dspMusicTime.remainderincrement * downSampleFactor;

            int newSteps = Mathf.FloorToInt((float)dspMusicTime.stepRemainder);
            dspMusicTime.stepIndex += newSteps;
            dspMusicTime.stepIndex = (int)Mathf.Repeat(dspMusicTime.stepIndex, totalBeatsPerSection * divisionsPerBeat);
            dspMusicTime.stepRemainder -= newSteps;

            return dspMusicTime;
        }

        void Beat()
        {
            //if (metronome == null)
            //{
            //    return;
            //}
            if (verboseDebug)
            {
                Debug.Log("Triggering Beat!");
            }
            double nextBeatIn = (beatLength - beatTimer);

            //maybe the +1 beat is normalisedPointInSection longer needed?
            assessingBeatIndex = sectionCounter + beatAssessOffset;
            //assessingBeatIndex = sectionCounter;



            assessingBeatIndex = (int)Mathf.Repeat(assessingBeatIndex, totalBeatsPerSection);


            nextBeatStart = prevBeatDspTime + beatLength;



            if (forceRegenOnLoop)
            {
                if (sectionCommenced && assessingBeatIndex == 0)
                {
                    //forceRegenOnLoop = false;
                    RegenDevices();
                }
            }
            if (assessingBeatIndex > 1)
            {
                sectionCommenced = true;
            }

            processBeat.Invoke();

            prevBeatDspTime = nextBeatStart;
        }


        public float ScrambleNoteOffset(float offset)
        {
            //scrambling moved to BeatMashers in post process. leaving this stub for compat

            return offset;
        }

        double RepeatDouble(double d, double r)
        {
            while (d > r)
            {
                d -= r;
            }

            return d;
        }


        public int PlayScheduledRoundRobin(SoundModule srcModule, AudioSource[] rra, double delayTime, int rraIndex, double endAfter = -1, bool forceMono = false, float fadeOutDuration = 0.125f)
        {
            if (verboseDebug)
            {
                Debug.Log("Scheduling note output, delayTime = " + delayTime + " instrument = " + rra[0].gameObject.name);
            }
            double startTime = GetScheduledTime(delayTime);


            rra[rraIndex].Stop();
            rra[rraIndex].PlayScheduled(startTime);
            if (forceMono)
            {
                int prevIndex = (int)Mathf.Repeat(rraIndex - 1, rra.Length);
                rra[prevIndex].SetScheduledEndTime(startTime);
            }


            if (endAfter > 0)
            {
                rra[rraIndex].SetScheduledEndTime(startTime + endAfter);
            }

            rraIndex += 1;
            if (rraIndex >= rra.Length)
            {
                rraIndex = 0;
            }

            return rraIndex;
        }


        public void PlayScheduledDirectSound(SoundModule srcModule, AudioDirectPlay player, AudioClip clip, double delayTime, float pitch, float volume,
        float vel, bool loop = false, double endAfter = -1, bool forceMono = true, bool isTie = false, double startSampleOffset = 0)
        {
            double startTime = GetScheduledTime(delayTime);
            double offTime = endAfter;

            if (endAfter != -1)
            {
                offTime = startTime + endAfter;
            }

            DirectNoteData note = player.MakeNoteData(startTime, offTime, clip, pitch, volume, vel, loop, isTie, startSampleOffset);
            
            player.AddNoteToQueue(note);
        }




        public double GetScheduledTime(double delayTime)
        {
            if (nextBeatStart < AudioSettings.dspTime)
            {
                nextBeatStart = AudioSettings.dspTime;
            }

            double sheduledTime = nextBeatStart + delayTime + globalLatency;
            if (verboseDebug)
            {
                Debug.Log("scheduled note input delay = " + delayTime + " scheduled time = " + sheduledTime);
            }
            return sheduledTime;
        }

        public void ScheduleNoteArticulationPitch(AudioSource targetSrc, float targetPitch, float delayTime, float targetDuration = 0.1f)
        {
            StartCoroutine(NoteArticulationPitch(targetSrc, targetPitch, delayTime, targetDuration));
        }


        public void ScheduleNoteArticulationVolume(AudioSource targetSrc, float targetVol, float delayTime, float targetDuration = 0.1f)
        {
            StartCoroutine(NoteArticulationVolume(targetSrc, targetVol, delayTime, targetDuration));
        }

        IEnumerator NoteArticulationPitch(AudioSource targetSrc, float targetPitch, float delayTime, float targetDuration)
        {
            float startPitch = targetSrc.pitch;
            if (startPitch == targetPitch)
            {
                yield break;
            }
            if (targetPitch <= 0)
            {
                if (verboseDebug)
                {
                    Debug.Log("Articulating note Pitch, skipping due to 0 target!");
                }
                yield break;
            }
            yield return new WaitForSecondsRealtime(delayTime + globalLatency);

            float t = 0.0f;
            startPitch = targetSrc.pitch;

            if (verboseDebug)
            {
                Debug.Log("Articulating note Pitch, start = " + startPitch + " target = " + targetPitch);
            }

            while (t < 1)
            {
                targetSrc.pitch = Mathf.Lerp(startPitch, targetPitch, t);
                t += Time.unscaledDeltaTime * (1 / targetDuration);

                yield return null;
            }
        }


        IEnumerator NoteArticulationVolume(AudioSource targetSrc, float targetVol, float delayTime, float targetDuration)
        {
            if (verboseDebug)
            {
                Debug.Log("Articulating note Volume");
            }
            yield return new WaitForSecondsRealtime(delayTime + globalLatency);

            float t = 0.0f;
            float startVol = targetSrc.volume;

            while (t < 1)
            {
                targetSrc.volume = Mathf.Lerp(startVol, targetVol, t);
                t += Time.unscaledDeltaTime * (1 / targetDuration);
                yield return null;
            }
            if (targetVol == 0)
            {
                targetSrc.volume = 0;
                //targetSrc.Stop();
            }
        }

        public static AnimationCurve ClampAnimCurve01(AnimationCurve animationCurve)
        {
            Keyframe[] keys = animationCurve.keys;
            for (var i = 0; i < keys.Length; i++)
            {
                keys[i].time = Mathf.Clamp01(animationCurve[i].time);
                keys[i].value = Mathf.Clamp01(animationCurve[i].value);
            }
            animationCurve.keys = keys;

            return animationCurve;
        }
    }
}