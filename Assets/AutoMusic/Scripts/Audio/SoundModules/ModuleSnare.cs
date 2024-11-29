using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{

    public class ModuleSnare : SoundModule
    {
        public float beatsUntilStart = 1;
        [Range(0, 100)]
        public float fillIntensity;
        public bool halfTime;
        public int[] snareRepeatStack = new int[] { 8, 6, 4, 10, 3, 2, 5 };
        [Range(0.5f, 8)]
        public float repeatStackPowerFunction = 3.0f;


        void OnValidate()
        {
            
            SanitiseInput();
        }
        public override void SanitiseInput()
        {
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
                init = true;
                GenerateInit();
            }
        }


        public override void GenerateLoop()
        {
            GetBeatsFromTimeSig();
            BeatInstance[] beatInstanceInnerLoop = new BeatInstance[loopBeatCount];
            for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
            {
                beatInstanceInnerLoop[i] = InitBeatInstance();
            }

            int beatInstanceIndex = 0;
            int beatDivisions = masterClock.divisionsPerBeat;
            int stepCount = loopBeatCount * beatDivisions;
            int nextStep = (int)(beatsUntilStart * beatDivisions);
            if (halfTime)
            {
                nextStep *= 2;
            }
            for (int stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                if (stepIndex != nextStep)
                {
                    continue;
                }
                float beatDivisor = (float)stepIndex / (float)beatDivisions;
                beatInstanceIndex = Mathf.FloorToInt(beatDivisor);
                float pointInBeat = beatDivisor - beatInstanceIndex;

                beatInstanceInnerLoop[beatInstanceIndex] = AddNoteToBeatInstance(beatInstanceInnerLoop[beatInstanceIndex], pointInBeat);

                int repeatInSteps = snareRepeatStack[(int)(Mathf.Pow(Random.Range(0.0f, 1.0f), repeatStackPowerFunction) * snareRepeatStack.Length)];
                if (halfTime)
                {
                    repeatInSteps *= 2;
                }
                nextStep += repeatInSteps;
            }

            beatInstanceInnerLoop = TrimBeat(beatInstanceInnerLoop, densityTrimInnerLoop);

            //extend loop to fill sequence
            FullLengthBeatWithFills(beatInstanceInnerLoop, fillIntensity);


            beatInstances = TrimBeat(beatInstances, densityTrimInnerLoop);
        }

        public override void ProcessBeat()
        {
            ProcessBeatCore();
        }
    }
}