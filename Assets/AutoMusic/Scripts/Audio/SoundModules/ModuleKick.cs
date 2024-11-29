using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{
    public enum KickRhythmLogic
    {
        DensityDisorder,
        EuclidVariation
    }
    public class ModuleKick : SoundModule
    {
        public KickRhythmLogic rythmLogic = KickRhythmLogic.DensityDisorder;
        [Range(0, 100)]
        public float fillIntensity;
        //[Header("Rhythm Props : DensityDisorder")]
        [Range(0, 100)]
        public float baseRhythmicDistortion;
        public Vector2 baseRhythmicDensity = Vector2.one;
        //[Header("Rhythm Props : EuclidVariation")]
        public int[] repeatStack = new int[] { 8, 6, 2, 10, 3, 4, 5 };
        [Range(0.5f, 8)]
        public float repeatStackPowerFunction = 3.0f;


        void OnValidate()
        {
            SanitiseInput();
        }
        public override void SanitiseInput()
        {
            fillIntensity = Mathf.Clamp(fillIntensity, 0, 100);
            baseRhythmicDistortion = Mathf.Clamp(baseRhythmicDistortion, 0, 100);
            baseRhythmicDensity = ValidateRhythmDensity(baseRhythmicDensity);
            repeatStack = ValidateRepeatStack(repeatStack);
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

        //should be different types for this : beat distort, riff, stop...
        public override void GenerateLoop()
        {
            GetBeatsFromTimeSig();

            BeatInstance[] beatInstanceInnerLoop = new BeatInstance[loopBeatCount];
            int beatDivisions = masterClock.divisionsPerBeat;

            if (rythmLogic == KickRhythmLogic.DensityDisorder)
            {
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = InitBeatInstance();

                    int notesInBeat = (int)Random.Range(baseRhythmicDensity.x, baseRhythmicDensity.y);

                    for (var j = 0; j < notesInBeat; j++)
                    {
                        float thisDistortion = 0;
                        if (D100(baseRhythmicDistortion))
                        {
                            thisDistortion = (float)(Random.Range(0, beatDivisions)) / (float)beatDivisions;
                            //Debug.Log("thisDistortion " + thisDistortion);

                        }
                        float noteOffset = thisDistortion;
                        if (!beatInstanceInnerLoop[i].noteFractionOffsets.Contains(noteOffset))
                        {
                            beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], noteOffset);
                        }
                    }

                    //pop a beat at kick at loop starts
                    if (i == 0 && !beatInstanceInnerLoop[i].noteFractionOffsets.Contains(0))
                    {
                        beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], 0);
                    }
                }
            }
            if (rythmLogic == KickRhythmLogic.EuclidVariation)
            {
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = InitBeatInstance();
                }

                int beatInstanceIndex = 0;

                int stepCount = loopBeatCount * beatDivisions;
                int nextStep = 0;

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

                    int repeatInSteps = repeatStack[(int)(Mathf.Pow(Random.Range(0.0f, 1.0f), repeatStackPowerFunction) * repeatStack.Length)];

                    nextStep += repeatInSteps;
                }
            }


            //extend loop to fill sequence
            FullLengthBeatWithFills(beatInstanceInnerLoop, fillIntensity);

        }

        public override void ProcessBeat()
        {
            ProcessBeatCore();
        }
    }
}