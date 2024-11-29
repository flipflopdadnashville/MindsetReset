using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;

namespace AutoMusic
{

    public enum ChordsRhythmMode
    {
        MarkChanges,
        StaggerChordUp,
        StaggerChordDown,
        StaggerChordBi,
        FollowOther
    }

    public class ModuleChords : SoundModule
    {
        public HarmonicHub harmonicHub;
        public Vector2Int chordExtensionRange;
        public ChordsRhythmMode rhythmMode = ChordsRhythmMode.MarkChanges;
        public int[] staggerSteps;
        [Range(0, 128)]
        public int staggerRepeats;
        [Header("Rhythm Props : FollowOther")]
        public SoundModule rhythmSource;
        [Range(0, 100)]
        public float deletionsFromRhythmSource;
        public int[] additionalDelaysFromRhythmSource = new int[] { 0, 2, 3, 6, 4 };
        //public float pitchMultiplier = 1;
        public bool loopSound;



        void OnValidate()
        {
            SanitiseInput();
        }
        public override void SanitiseInput()
        {
            staggerSteps = ValidateRepeatStack(staggerSteps);
            additionalDelaysFromRhythmSource = ValidateRepeatStack(additionalDelaysFromRhythmSource, 0);
        }

        void Start()
        {
            if (outputMode == SoundOutputMode.DirectSound)
            {
                player.voiceCount = 4;
            }
        }

        void LateUpdate()
        {
            if (!init)
            {
                if (rhythmMode == ChordsRhythmMode.FollowOther && rhythmSource != null)
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


        public override void GenerateLoop()
        {
            GetBeatsFromTimeSig();
            Chord[] chordSequence = harmonicHub.chordSequence;
            List<int> chordChangeSteps = new List<int>();
            for (var i = 0; i < chordSequence.Length; i++)
            {
                chordChangeSteps.Add(chordSequence[i].startStep);
            }
            int barCount = harmonicHub.barsInSequence;
            int beatCount = barCount * masterClock.beatsPerBar;
            loopBeatCount = beatCount;
            int divisionsPerBeat = masterClock.divisionsPerBeat;
            int stepCount = beatCount * divisionsPerBeat;


            BeatInstance[] beatInstanceInnerLoop = new BeatInstance[beatCount];
            for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
            {
                beatInstanceInnerLoop[i] = InitBeatInstance();
            }
            if (rhythmMode != ChordsRhythmMode.FollowOther)
            {
                int currStep = 0;
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    for (var j = 0; j < divisionsPerBeat; j++)
                    {
                        float thisOffset = (float)j / divisionsPerBeat;

                        if (chordChangeSteps.Contains(currStep))
                        {
                            int chordChangeIndex = chordChangeSteps.IndexOf(currStep);
                            Chord chord = chordSequence[chordChangeIndex];

                            int startIndex = (int)Mathf.Max(0, Mathf.Min(chord.chord.Count - 1, chordExtensionRange.x));
                            int endIndex = (int)Mathf.Min(chordExtensionRange.y + 1, chord.chord.Count);

                            if (rhythmMode == ChordsRhythmMode.MarkChanges)
                            {
                                for (var k = startIndex; k < endIndex; k++)
                                {
                                    float pitch = chord.chord[k];

                                    beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], thisOffset, 0, pitch);
                                }
                            }
                            if (rhythmMode == ChordsRhythmMode.StaggerChordUp || rhythmMode == ChordsRhythmMode.StaggerChordDown || rhythmMode == ChordsRhythmMode.StaggerChordBi)
                            {
                                int chordChangeStep = chord.startStep + chord.stepDuration;
                                int stepAdvance = staggerSteps[Random.Range(0, staggerSteps.Length)];

                                int[] staggerLookUp = StaggerChordLookUpTable(startIndex, endIndex);

                                int staggerLookupIndex = 0;

                                while (staggerLookupIndex < staggerLookUp.Length)
                                {
                                    int targetStep = (i * divisionsPerBeat) + stepAdvance;
                                    int targetBeat = targetStep / divisionsPerBeat;
                                    float targetOffset = Mathf.Repeat((float)targetStep / (float)divisionsPerBeat, 1);

                                    int lookupIndex = staggerLookUp[staggerLookupIndex];
                                    float pitch = chord.chord[lookupIndex];


                                    stepAdvance += staggerSteps[Random.Range(0, staggerSteps.Length)];
                                    staggerLookupIndex += 1;

                                    if (targetStep < chordChangeStep)
                                    {
                                        beatInstanceInnerLoop[targetBeat] = AddNoteToBeatInstance(beatInstanceInnerLoop[targetBeat], targetOffset, 0, pitch);
                                    }
                                }
                            }
                        }
                        currStep += 1;
                    }
                }
            }

            if (rhythmMode == ChordsRhythmMode.FollowOther)
            {
                int repeatIndex = (int)Mathf.Min(rhythmSource.beatInstances.Length, beatInstanceInnerLoop.Length);
                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    int fetchIndex = (int)(i % repeatIndex);
                    beatInstanceInnerLoop[i] = DuplicateBeatInstance(rhythmSource.beatInstances[fetchIndex]);

                    //deletions
                    for (var j = 0; j < beatInstanceInnerLoop[i].noteFractionOffsets.Count; j++)
                    {
                        if (D100(deletionsFromRhythmSource))
                        {
                            RemoveNoteFromBeatInstance(beatInstanceInnerLoop[i], j);
                        }
                    }

                    //add delays
                    int delay = additionalDelaysFromRhythmSource[Random.Range(0, additionalDelaysFromRhythmSource.Length)];
                    if (delay != 0)
                    {
                        float noteOffset = (float)delay / (float)divisionsPerBeat;
                        beatInstanceInnerLoop[i] = AddNoteToBeatInstance(beatInstanceInnerLoop[i], noteOffset);
                    }
                }

                for (var i = 0; i < beatInstanceInnerLoop.Length; i++)
                {
                    for (var j = 0; j < beatInstanceInnerLoop[i].noteFractionOffsets.Count; j++)
                    {
                        float currOffset = beatInstanceInnerLoop[i].noteFractionOffsets[j];
                        int currStep = (int)(i * divisionsPerBeat + (currOffset * divisionsPerBeat));
                        currStep = (int)Mathf.Repeat(currStep, harmonicHub.chordSequencePerStepRef.Length);
                        Chord chordAtStep = harmonicHub.chordSequencePerStepRef[currStep];
                        int startIndex = (int)Mathf.Max(0, Mathf.Min(chordAtStep.chord.Count - 1, chordExtensionRange.x));
                        int endIndex = (int)Mathf.Min(chordExtensionRange.y + 1, chordAtStep.chord.Count);

                        int lookupIndex = Random.Range(startIndex, endIndex);
                        float pitch = chordAtStep.chord[lookupIndex];

                        beatInstanceInnerLoop[i].pitches[j] = pitch;
                    }
                }
            }



            beatInstanceInnerLoop = TrimBeat(beatInstanceInnerLoop, densityTrimInnerLoop);

            FullSeqBeatInstances(beatInstanceInnerLoop, loopBeatCount);
        }

        int[] StaggerChordLookUpTable(int startIndex, int endIndex)
        {
            int count = endIndex - startIndex;
            List<int> lookup = new List<int>();

            for (var i = 0; i < count; i++)
            {
                lookup.Add(startIndex + i);
            }

            if (rhythmMode == ChordsRhythmMode.StaggerChordDown)
            {
                lookup.Reverse();
            }

            if (staggerRepeats > 0)
            {
                for (var i = 0; i < staggerRepeats; i++)
                {
                    lookup.Add(lookup[(int)Mathf.Repeat(i, count)]);
                }
            }

            if (rhythmMode == ChordsRhythmMode.StaggerChordBi)
            {
                List<int> lookupJumble = new List<int>();

                for (var i = 0; i < lookup.Count; i++)
                {
                    int randomIndex = Random.Range(0, lookup.Count);
                    lookupJumble.Add(lookup[randomIndex]);
                    lookup.RemoveAt(randomIndex);
                }

                lookup = lookupJumble;
            }

            return lookup.ToArray();
        }

        public override void ProcessBeat()
        {
            ProcessBeatPitched(false, loopSound, 1);

            if (loopSound && !active)
            {
                StopAudio();
            }
        }
    }
}