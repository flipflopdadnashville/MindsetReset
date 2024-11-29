using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{
    public enum FillMode3x3
    {
        None,
        Reroll
    }
    public enum RhythmMode3x3
    {
        SixteenthScatters,
        Snareish,
        FollowOther
    }

    public class Module3x3 : SoundModule
    {
        public AudioClip[] clipsBass;
        public AudioClip[] clipsOctave;
        [Header("Rhythm Props : SixteenthScatters")]
        [Range(0, 1)]
        public float density = 0.5f;
        [Range(0, 1)]
        public float accents = 0.15f;
        [Range(0, .75f)]
        public float octaves = 0.25f;
        [Range(0, 0.5f)]
        public float ties = 0.1f;
        [Range(0, 1)]
        public float arpeggiation = 0.25f;
        public FillMode3x3 fillMode = FillMode3x3.None;
        public RhythmMode3x3 rhythmMode = RhythmMode3x3.SixteenthScatters;
        public BassPitchLogic pitchLogic;
        [Space(10)]
        float foldHiPitchesTargetVal = 1.8f;
        [Header("Rhythm Props : Snarish")]
        public int[] snareRepeatStack = new int[] { 6, 4, 2, 3, 1, 5 };
        [Range(0.5f, 8)]
        public float repeatStackPowerFunction = 3.0f;
        public float snareBeatsUntilStart = 1;
        [Header("Rhythm Props : FollowOther")]
        public SoundModule rhythmSource;
        [Range(0, 100)]
        public float deletionsFromRhythmSource;
        [Range(0, 100)]
        public float additionalInbetweensFromRhythmSource;
        [Range(0, 100)]
        public float additionalArpOnInbetweens = 25;

        public HarmonicHub harmonicHub;
        //public float pitchMultiplier = 1;


        int beatDivisions;
        int sequentialOctaves;
        float stepOffset;


        void OnValidate()
        {
            SanitiseInput();
        }
        public override void SanitiseInput()
        {
            clipsBass = Validate3x3Clips(clipsBass);
            clipsOctave = Validate3x3Clips(clipsOctave);

            snareBeatsUntilStart = ValidateSnareStart(snareBeatsUntilStart);
            snareRepeatStack = ValidateRepeatStack(snareRepeatStack);
        }

        AudioClip[] Validate3x3Clips(AudioClip[] clips)
        {
            if (clips.Length == 2)
            {
                return clips;
            }

            AudioClip[] validatedClips = new AudioClip[2];
            for (var i = 0; i < clips.Length && i < validatedClips.Length; i++)
            {
                validatedClips[i] = clips[i];
            }

            return validatedClips;
        }

        void Start()
        {
        }

        void LateUpdate()
        {
            if (!init)
            {
                if (rhythmMode == RhythmMode3x3.FollowOther && rhythmSource != null)
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


        BeatInstance Beat303Rhythm()
        {
            BeatInstance bi = InitBeatInstance();
            for (var j = 0; j < beatDivisions; j++)
            {
                float thisOffset = stepOffset * j;

                if (!D100(density * 100))
                {
                    continue;
                }

                int output = 0;
                if (sequentialOctaves < 2)
                {
                    if (D100(octaves * 100))
                    {
                        output = 1;
                        sequentialOctaves += 1;
                    }
                }
                if (output == 0)
                {
                    sequentialOctaves = 0;
                }

                float pitch = 0;
                if (D100(arpeggiation * 100))
                {
                    pitch = Mathf.Floor(Random.Range(1, 4));
                }

                float velocity = 0.4f;
                if (D100(accents * 100))
                {
                    velocity = 1.0f;
                }
                bool tie = false;
                if (j != 0)
                {
                    if (D100(ties * 100))
                    {
                        tie = true;
                    }
                }

                bi = AddNoteToBeatInstance(bi, thisOffset, output, pitch, velocity, tie);
            }

            return bi;
        }
        BeatInstance[] BeatRhythmSnare(BeatInstance[] beatInstanceInnerLoop)
        {
            for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
            {
                beatInstanceInnerLoop[i] = InitBeatInstance();
            }

            int beatInstanceIndex = 0;
            int beatDivisions = masterClock.divisionsPerBeat;
            int stepCount = loopBeatCount * beatDivisions;
            int nextStep = (int)(snareBeatsUntilStart * beatDivisions);

            for (int stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                if (stepIndex != nextStep)
                {
                    continue;
                }
                float beatDivisor = (float)stepIndex / (float)beatDivisions;
                beatInstanceIndex = Mathf.FloorToInt(beatDivisor);
                float pointInBeat = beatDivisor - beatInstanceIndex;

                int output = 0;
                if (D100(octaves * 100))
                {
                    output = 1;
                }

                float pitch = 0;
                if (D100(arpeggiation * 100))
                {
                    pitch = Mathf.Floor(Random.Range(1, 4)); 
                }

                float velocity = 0.4f;
                if (D100(accents * 100))
                {
                    velocity = 1.0f;
                }
                bool tie = false;
                if (stepIndex != 0)
                {
                    if (D100(ties * 100))
                    {
                        tie = true;
                    }
                }


                beatInstanceInnerLoop[beatInstanceIndex] = AddNoteToBeatInstance(beatInstanceInnerLoop[beatInstanceIndex], pointInBeat, output, pitch, velocity, tie);

                int repeatInSteps = snareRepeatStack[(int)(Mathf.Pow(Random.Range(0.0f, 1.0f), repeatStackPowerFunction) * snareRepeatStack.Length)];

                nextStep += repeatInSteps;
            }

            return beatInstanceInnerLoop;
        }

        BeatInstance[] BeatRhythmFollow(BeatInstance[] beatInstanceInnerLoop)
        {
            int repeatIndex = (int)Mathf.Min(rhythmSource.beatInstances.Length, beatInstanceInnerLoop.Length);
            //rythm data
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
                    int inbetweens = (int)(Random.Range(0f, density) * masterClock.divisionsPerBeat);
                    for (var j = 0; j < inbetweens; j++)
                    {
                        float noteOffset = (Random.Range(0, beatDivisions)) / (float)beatDivisions;

                        int output = 0;
                        if (D100((arpeggiation + additionalArpOnInbetweens) * 1.5f))
                        {
                            output = Random.Range(1, 4);
                        }



                        beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], noteOffset, output, 1, 0.5f);
                    }
                }
            }
            //rhtyhm meta data
            bool firstNoteInit = false;
            for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
            {
                for (var j = 0; j < beatInstanceInnerLoop[i].noteFractionOffsets.Count; j++)
                {
                    beatInstanceInnerLoop[i].noteOutputs[j] = 0;
                    if (D100(octaves * 100))
                    {
                        beatInstanceInnerLoop[i].noteOutputs[j] = 1;
                    }

                    beatInstanceInnerLoop[i].pitches[j] = 0;
                    if (D100(arpeggiation * 100))
                    {
                        beatInstanceInnerLoop[i].pitches[j] = Mathf.Floor(Random.Range(1, 4));
                    }

                    beatInstanceInnerLoop[i].velocities[j] = 0.4f;
                    if (D100(accents * 100))
                    {
                        beatInstanceInnerLoop[i].velocities[j] = 1.0f;
                    }
                    beatInstanceInnerLoop[i].ties[j] = false;
                    if (firstNoteInit)
                    {
                        if (D100(ties * 100))
                        {
                            beatInstanceInnerLoop[i].ties[j] = true;
                        }
                    }
                    firstNoteInit = true;
                }
            }

            return beatInstanceInnerLoop;
        }

        //Beat303Pitch(srcBeat)
        BeatInstance Beat303Pitch(BeatInstance srcBeat, int harmonicStepRangeStart)
        {
            BeatInstance newBeat = DuplicateBeatInstance(srcBeat);

            for (var j = 0; j < srcBeat.noteFractionOffsets.Count; j++)
            {
                int thisStep = harmonicStepRangeStart + (int)(srcBeat.noteFractionOffsets[j] * beatDivisions);
                int chordDegree = (int)newBeat.pitches[j];
                float pitch = 1;
                if (pitchLogic == BassPitchLogic.ChordRoots)
                {
                    //second chance to mod pitch, not bound to inner loop
                    if (D100(arpeggiation * 0.25f))
                    {
                        chordDegree = Random.Range(0, harmonicHub.chordSequencePerStepRef[thisStep].chord.Count);
                    }

                    if (chordDegree >= harmonicHub.chordSequencePerStepRef[thisStep].chord.Count)
                    {
                        chordDegree = Random.Range(0, harmonicHub.chordSequencePerStepRef[thisStep].chord.Count);
                    }
                    
                    pitch = harmonicHub.chordSequencePerStepRef[thisStep].chord[chordDegree];
                }
                if (pitchLogic == BassPitchLogic.Spam157 || pitchLogic == BassPitchLogic.PentatonicSpam)
                {
                    float[] pitches = GetPitches(pitchLogic, harmonicHub);
                    int pitchIndex = (int)Mathf.Repeat(chordDegree, pitches.Length);
                    pitch = pitches[pitchIndex];
                }

                pitch = FoldHiPitches(pitch, foldHiPitchesTargetVal);

                //Debug.Log("pitch = " + pitch + " thisStep = " + thisStep);
                float thisPitch = pitch;

                newBeat.pitches[j] = thisPitch;
            }

            return newBeat;
        }

        public override void GenerateLoop()
        {
            GetBeatsFromTimeSig();
            BeatInstance[] beatInstanceInnerLoop = new BeatInstance[loopBeatCount];

            beatDivisions = masterClock.divisionsPerBeat;
            sequentialOctaves = 0;
            stepOffset = 1.0f / beatDivisions;

            if (rhythmMode == RhythmMode3x3.SixteenthScatters)
            {
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    beatInstanceInnerLoop[i] = Beat303Rhythm();
                }
            }
            if (rhythmMode == RhythmMode3x3.Snareish)
            {
                beatInstanceInnerLoop = BeatRhythmSnare(beatInstanceInnerLoop);
            }
            if (rhythmMode == RhythmMode3x3.FollowOther)
            {
                beatInstanceInnerLoop = BeatRhythmFollow(beatInstanceInnerLoop);
                //match loop to target length (instead o finstrument being followed)
                if (beatInstances == null || beatInstances.Length != masterClock.totalBeatsPerSection)
                {
                    beatInstances = new BeatInstance[masterClock.totalBeatsPerSection];
                }
                for (var i = 0; i < beatInstances.Length; i++)
                {
                    beatInstances[i] = DuplicateBeatInstance(beatInstanceInnerLoop[i % loopBeatCount]);
                }
            }

            beatInstanceInnerLoop = TrimBeat(beatInstanceInnerLoop, densityTrimInnerLoop);

            //match harmonic hub length
            int harmonicBeatLength = harmonicHub.barsInSequence * masterClock.beatsPerBar;
            BeatInstance[] beatInstancesHarmonicLoop = new BeatInstance[harmonicBeatLength];
            for (var i = 0; i < beatInstancesHarmonicLoop.Length; i++)
            {
                beatInstancesHarmonicLoop[i] = DuplicateBeatInstance(beatInstanceInnerLoop[i % beatInstanceInnerLoop.Length]);
            }

            for (var i = 0; i < beatInstancesHarmonicLoop.Length; i++)
            {
                int harmonicStepRangeStart = i * beatDivisions;

                BeatInstance srcBeat = beatInstancesHarmonicLoop[i];


                beatInstancesHarmonicLoop[i] = Beat303Pitch(srcBeat, harmonicStepRangeStart);
            }

            //extend loop to fill sequence
            beatInstances = new BeatInstance[masterClock.totalBeatsPerSection];
            for (var i = 0; i < beatInstances.Length; i++)
            {
                beatInstances[i] = DuplicateBeatInstance(beatInstancesHarmonicLoop[i % beatInstancesHarmonicLoop.Length]);
            }



            //add fills
            if (fillMode == FillMode3x3.Reroll)
            {
                for (var i = 0; i < beatInstances.Length; i++)
                {
                    if (!masterClock.fillbeats.Contains(i))
                    {
                        continue;
                    }
                    //regen rhythm
                    beatInstances[i] = Beat303Rhythm();

                    //regen pitch
                    int harmonicStepRangeStart = (int)Mathf.Repeat(i * beatDivisions, harmonicBeatLength);
                    beatInstances[i] = Beat303Pitch(beatInstances[i], harmonicStepRangeStart);
                }
            }

            //set durations
            for (var i = 0; i < beatInstances.Length; i++)
            {
                BeatInstance bi = beatInstances[i];
                for (var j = 0; j < bi.noteFractionOffsets.Count; j++)
                {
                    float thisDuration = 0.85f;
                    float thisOffset = bi.noteFractionOffsets[j];
                    if (j < bi.noteFractionOffsets.Count - 1)
                    {
                        if (bi.ties[j + 1])
                        {
                            //Debug.Log("Generating 3x3 note duration, slewing to tied note eithin same beat");
                            thisDuration = bi.noteFractionOffsets[j + 1] - thisOffset * masterClock.divisionsPerBeat;
                            thisDuration += 0.35f;
                        }
                    }
                    else
                    {
                        int nextBeatIndex = (int)Mathf.Repeat(i + 1, beatInstances.Length);
                        if (beatInstances[nextBeatIndex].ties.Count > 0 && beatInstances[nextBeatIndex].ties[0])
                        {
                            //Debug.Log("Generating 3x3 note duration, slewing to tied note in next beat");
                            thisDuration = (1 + bi.noteFractionOffsets[0]) - thisOffset * masterClock.divisionsPerBeat;
                            thisDuration += 0.35f;
                        }
                        else
                        {
                            //Debug.Log("Generating 3x3 note duration, extending to fill void");
                            float nextNote = FindNextNoteAddress(beatInstances, i, thisOffset);
                            thisDuration = GetStepsCountBetweenNotes(BeatNoteOffsetToFloat(i, thisOffset), nextNote, beatInstances.Length);
                            thisDuration = Mathf.Max(0.85f, thisDuration * bi.velocities[j]);
                        }
                    }
                    //thisDuration *= 0.85f;
                    //thisDuration = 0.85f;
                    if (masterClock.verboseDebug)
                    {
                        Debug.Log("Generating 3x3 note duration, steps = " + thisDuration);
                    }
                    bi.stepsDuration[j] = thisDuration;
                }
            }
        }
        

        public override void ProcessBeat()
        {
            ProcessBeat3x3(clipsBass, clipsOctave, 1);
        }
    }
}