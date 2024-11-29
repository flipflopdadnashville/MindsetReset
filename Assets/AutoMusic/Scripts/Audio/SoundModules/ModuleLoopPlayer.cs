using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using AutoMusic;


namespace AutoMusic
{
    public class ModuleLoopPlayer : SoundModule
    {
        //[Range(1, 16)]
        //public int timeSliceSyncPerSteps = 4;
        [HideInInspector] public List<int> repitchRetrigSteps = new List<int>();
        
        [Header("DebugRef")]
        [HideInInspector] public int clipStepCount;
        [HideInInspector] public double clipLength;
        [HideInInspector] public int lastProcessedStep;
        [HideInInspector] public int processesedSteps;
        [HideInInspector] public float stepProcessTIme;
        [HideInInspector] public int targetStep;
        [HideInInspector] public int repeatStep;
        [HideInInspector] public bool debug;

        int timeSliceSyncPerStepsLocked = 1;

        void Start()
        {
            lastProcessedStep = -1;
        }


        /*
        void Update()
        {
            if (!active)
            {
                return;
            }
            if (repitchRetrigSteps == null || repitchRetrigSteps.Count == 0)
            {
                repitchRetrigSteps = new List<int>{0};
            }
            GetBeatsFromTimeSig();

            clipStepCount = loopBeatCount * masterClock.divisionsPerBeat;
            clipLength = audioClip.length;

            if (outputMode == SoundOutputMode.AudioSources)
            {
                SetPitch(pitchMultiplier);
            }

            if (lastProcessedStep != masterClock.stepCounter)
            {
                if (outputMode == SoundOutputMode.AudioSources)
                {
                    ProcessStep();
                }
                if (outputMode == SoundOutputMode.DirectSound)
                {
                    ProcessStepDirect();
                }
            }
        }
        */

        void LateUpdate()
       {
           if (!init)
           {
               init = true;
               GenerateInit();
           }
       }


       public override void GenerateLoop()
       {
           GetBeatsFromTimeSig();

           BeatInstance[] beatInstanceInnerLoop = new BeatInstance[loopBeatCount];
           int beatDivisions = masterClock.divisionsPerBeat;
           int totalLoopSteps = loopBeatCount * beatDivisions;

           for (var i = 0;  i < loopBeatCount; i++)
           {
               beatInstanceInnerLoop[i] = InitBeatInstance();

               for (var j = 0; j < beatDivisions; j++)
               {
                   float offset = (float)j / (float)beatDivisions;

                   int loopStep = (i * beatDivisions) + j;
                   float t = (float)loopStep / (float)totalLoopSteps;
                   float startOffsetSeconds = Mathf.Lerp(0, audioClip.length, t);


                   beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], offset, 0, 1f, 1f, false, (float)GetStepLength(), startOffsetSeconds);
               }
           }

           FullSeqBeatInstances(beatInstanceInnerLoop, loopBeatCount);
       }

       public override void ProcessBeat()
       {
           ProcessBeatAudioFragment();
       }
       

        void SetPitch(float pitch)
        {
            for (var i = 0; i < roundRobinAudio.Length; i++)
            {
                roundRobinAudio[i].pitch = pitch;
            }
        }

        bool CheckStep()
        {
            if (masterClock.sectionCounterRaw < -1)
            {
                return false;
            }

            if (Mathf.Abs(masterClock.stepCounter - lastProcessedStep) < timeSliceSyncPerStepsLocked)
            {
                return false;
            }
            processesedSteps += 1;

            return true;
        }

        public void ProcessStep()
        {
            if (!CheckStep())
            {
                return;
            }

            SetRepeatStepTargetStep();


            roundRobinAudio[robinIndex].Stop();
            roundRobinAudio[robinIndex].clip = audioClip;
            roundRobinAudio[robinIndex].time = GetTargetStart();
            roundRobinAudio[robinIndex].volume = baseVolume;
            robinIndex = masterClock.PlayScheduledRoundRobin(this, roundRobinAudio, FinaliseOffset(), robinIndex, GetStepLength() * timeSliceSyncPerStepsLocked);


            MarkStepAsProcessed();

            if (masterClock.verboseDebug)
            {
                Debug.Log("loop step offset = " + FinaliseOffset());
            }
        }

        

        public void ProcessStepDirect()
        {
            if (!CheckStep())
            {
                return;
            }

            SetRepeatStepTargetStep();

            int targetStartSamples = (int)(GetTargetStart() * audioClip.frequency);
            masterClock.PlayScheduledDirectSound(this, player, audioClip, FinaliseOffset(), pitchMultiplier, baseVolume, 1,
            false, GetStepLength() * timeSliceSyncPerStepsLocked, true, false, targetStartSamples);


            MarkStepAsProcessed();

            if (masterClock.verboseDebug)
            {
                Debug.Log("loop step offset = " + FinaliseOffset());
            }
        }


        void SetRepeatStepTargetStep()
        {
            repeatStep = (int)Mathf.Min(clipStepCount, masterClock.totalBeatsPerSection * masterClock.divisionsPerBeat);
            targetStep = (int)Mathf.Repeat(masterClock.stepCounter, repeatStep);
        }
        float GetTargetStart()
        {
            return Mathf.Lerp(0, audioClip.length, (float)targetStep / (float)clipStepCount);

        }

        double GetStepLength()
        {
            return Math.Min(masterClock.stepLength, ((double)clipLength / (double)clipStepCount) * pitchMultiplier);
        }
        double FinaliseOffset()
        {
            float baseNoteOffset = (float)(targetStep % masterClock.divisionsPerBeat) / (float)masterClock.divisionsPerBeat;
            double swingMilliseconds = 0;
            return swingMilliseconds + (baseNoteOffset * masterClock.beatLength);
        }

        void MarkStepAsProcessed()
        {
            lastProcessedStep = masterClock.stepCounter;
        }
    }
}