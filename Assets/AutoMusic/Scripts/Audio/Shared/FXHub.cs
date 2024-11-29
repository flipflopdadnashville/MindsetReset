using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using AutoMusic;

namespace AutoMusic
{
    [System.Serializable]
    public struct DelayProperties
    {
        public AudioEchoFilter delay;

    }
    public class FXHub : MonoBehaviour
    {
        public AudioMixer mixer;

        [HideInInspector] public bool delayBpmSync;
        [Range(1, 12)]
        public int delayTimeSteps;
        [Range(10f, 1000f)]
        public float delayTimeMS;
        [Range(0.0f, 1.0f)]
        public float delayDecay;
        [Space(10)]
        public bool directSoundSpatialisation;

        //[Header("DebugRef")]
        [HideInInspector] public MasterClock masterClock;

       

        [HideInInspector] public bool initByCompositionHub;




#if UNITY_EDITOR
        private void OnValidate()
        {
            delayBpmSync = true;
        }
#endif
        void Start()
        {
            masterClock = FindObjectOfType<MasterClock>();
            //snapShot.TransitionTo(0);
        }

        //OnAudioFilterRead(float[] data, int channels)
        void Update()
        {



            UpdateRequiresMixer();
        }


        void UpdateRequiresMixer()
        {
            if (mixer == null)
            {
                return;
            }
            float delayTime = delayTimeMS;
            if (delayBpmSync)
            {
                delayTime = (float)(masterClock.beatLength / (double)masterClock.divisionsPerBeat) * delayTimeSteps * 1000;
            }

            if (mixer != null)
            {
                mixer.SetFloat("DelayTime", delayTime);
                mixer.SetFloat("DelayDecay", delayDecay);
            }



        }


        public float GetNoteDelay(float noteOffset)
        {
            return (noteOffset * (float)masterClock.beatLength) + masterClock.globalLatency + (float)masterClock.beatLength;
            //return (noteOffset * (float)masterClock.beatLength) + (float)masterClock.beatLength;
        }

        public float GetVelocity(SoundModule sm)
        {
            float velocity = 1;

            if (sm.velocityInfluence == 0)
            {
                return velocity;
            }

            
            AnimationCurve veloCurve = masterClock.velocityCurve;
            velocity *= Mathf.Lerp(1, veloCurve.Evaluate(masterClock.pointInBar), sm.velocityInfluence);

            return velocity;
        }
    }
}