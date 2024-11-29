using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;
//using System;


namespace AutoMusic
{

    public enum TonalScale
    {
        Ionian,
        Dorian,
        Phrygian,
        Lydian,
        Myxoldian,
        Aeolian,
        Locrian,
        Diminished,
        MelodicMinor,
        MelodicMinorDorianb2,
        MelodicMinorLydianAug,
        MelodicMinorLydianDom,
        MelodicMinorMyxoldianb6,
        MelodicMinorLocrian,
        MelodicMinorAltered,
        WholeTone
    }
    [System.Serializable]
    public class Chord
    {
        public List<float> chord;
        public int rootIndex;
        public int startStep;
        public int stepDuration;
        public bool init;
    }

    public struct ChordReference
    {
        public int index;
        public int startStep;
        public int stepDuration;
    }

    [System.Serializable]
    public class ChordExtensionStack
    {
        public List<int> chordExtensions = new List<int>();
    }

    public class HarmonicHub : MonoBehaviour
    {
        [Tooltip("Injected Sequences are sequence that are inserted into the object manually (or through MIDI), enabling this box will use that sequence if available")]
        public bool useInjectedSequence;
        [Tooltip("The source Injectable Generator to grab from when useInjectedSequence == true. Can be hot-swapped during playmode to switch between sources")]
        public InjectableGenerator injectableSrc;
        [Space(10)]
        [Tooltip("The type of musical scale (mode) to use when generating sequences")]
        public TonalScale scale;
        [Tooltip("How many semitones of pitch offset to shift the sequence by. Useful for key changes.")]
        [Range(-7, 7)]
        public int transpose;
        [Tooltip("Force the generated chord sequences to begin with the root note of the given scale")]
        public bool startOnRootChord = true;
        [Tooltip("Min & Max number of chords to use in the sequence")]
        public Vector2Int chordCount = new Vector2Int(3, 4);
        public int barsInSequence = 4;
        [Tooltip("Biases chords to change with bars. Chord sequences will become more rhythmically ‘chaotic’ with this set lower. ")]
        [Range(0.0f, 1.0f)]
        public float proportionChordsBeatOne = 0.5f;
        [Tooltip("Min & Max number of notes to add on top of the root note for each chord in the sequence")]
        public Vector2Int extensionCount = new Vector2Int(2, 4);
        [Tooltip("Interacts with the following control. When on, all chords in the sequence will use the same extension stack, when off each chord will independently pick a stack to use")]
        [HideInInspector] public bool homogenousExtensions;
        [Tooltip("Expressed in scale intervals, the notes to add to the generated roots to build full chords. For example, for a typical root-third-fifth chord this stack should have two entries: 3 & 5. ")]
        public List<ChordExtensionStack> chordExtensionStacks = new List<ChordExtensionStack>();


        //[Header("DebugRef")]
        
        [HideInInspector] public MasterClock masterClock;
        [HideInInspector] public bool init;
        [HideInInspector] public float[] pitches;
        [HideInInspector] public Chord[] chordSequence;
        [HideInInspector] public Chord[] chordSequencePerStepRef;
        [HideInInspector] public ChordExtensionStack extensionStackInUse;
        [HideInInspector] public bool runScaleChecks;


        void Awake()
        {
            masterClock = FindObjectOfType<MasterClock>();

            if (runScaleChecks)
            {
                RunScaleChecks();
            }
            homogenousExtensions = false;
        }

        void RunScaleChecks()
        {
            int numScales = System.Enum.GetValues(typeof(TonalScale)).Length;
            for (var i = 0; i < numScales; i++)
            {
                scale = (TonalScale)i;
                List<int> testSteps = ScaleSteps();


                int stepIndex = 0;
                for (var j = 0; j < testSteps.Count; j++)
                {
                    stepIndex += testSteps[j];
                    string currNote = RefScale(stepIndex);

                    if (masterClock.verboseDebug)
                    {
                        Debug.Log("scale = " + scale + " scaleVal = " + j + " note = " + currNote);
                    }
                }
            }
        }

        void OnValidate()
        {
            //chordcount
            chordCount.x = Mathf.Max(Mathf.Min(chordCount.x, 32), 1);
            chordCount.y = Mathf.Max(Mathf.Min(chordCount.y, 32), 1);

            //extensionCount
            extensionCount.x = Mathf.Max(Mathf.Min(extensionCount.x, 7), 0);
            extensionCount.y = Mathf.Max(Mathf.Min(extensionCount.y, 7), 0);


            //extension stack
            if (chordExtensionStacks == null || chordExtensionStacks.Count == 0)
            {
                List<int> chordDefault = new List<int>() { 3, 5, 7 };
                ChordExtensionStack extStack = new ChordExtensionStack();
                extStack.chordExtensions = chordDefault;

                chordExtensionStacks.Add(extStack);
            }
            for (var i = 0; i < chordExtensionStacks.Count; i++)
            {
                for (var j = 0; j < chordExtensionStacks[i].chordExtensions.Count; j++)
                {
                    chordExtensionStacks[i].chordExtensions[j] = Mathf.Max(Mathf.Min(chordExtensionStacks[i].chordExtensions[j], 19), 0);
                }
            }

            //sequence length
            barsInSequence = Mathf.Min(Mathf.Max(1, barsInSequence), 64);
        }

        string RefScale(int scaleVal)
        {
            List<string> notes = new List<string> { "c", "c+", "d", "d+", "e", "f", "f+", "g", "g+", "a", "a+", "b", "C" };

            if (scaleVal < notes.Count)
            {
                return notes[scaleVal];
            }

            return "note too high?";
        }

        void Update()
        {
            if (!init)
            {
                init = true;
                GenerateSequence();
            }
        }

        //contains all steps that are first steps in bar, escape the first bar as that is an auto-include
        List<int> OneDropChordStartSteps()
        {
            int stepsInBar = masterClock.beatsPerBar * masterClock.divisionsPerBeat;
            int stepCount = barsInSequence * masterClock.beatsPerBar * masterClock.divisionsPerBeat;

            List<int> barSteps = new List<int>();
            for (var i = 1; i < stepCount; i++)
            {
                if (i % stepsInBar == 0)
                {
                    barSteps.Add(i);
                }
            }

            return barSteps;
        }

        //contains all steps that are NOT first steps in bar
        List<int> FractalChordStartSteps()
        {
            List<int> fractalChordStartSteps = new List<int>();
            int stepCount = barsInSequence * masterClock.beatsPerBar * masterClock.divisionsPerBeat;

            List<int> stepsUnclaimed = new List<int>();
            for (var i = 1; i < stepCount; i++)
            {
                stepsUnclaimed.Add(i);
            }

            //bars
            int stepsInBar = masterClock.beatsPerBar * masterClock.divisionsPerBeat;
            List<int> barSteps = new List<int>();
            for (var i = 1; i < stepCount; i++)
            {
                if (i % stepsInBar == 0)
                {
                    fractalChordStartSteps.Add(i);
                    barSteps.Add(i);
                    stepsUnclaimed.Remove(i);
                }
            }
            //halfs
            int stepsInHalf = (int)(masterClock.beatsPerBar * masterClock.divisionsPerBeat * 0.5f);
            List<int> halfSteps = new List<int>();
            for (var i = 1; i < stepCount; i++)
            {
                if (i % stepsInHalf == 0 && i % stepsInBar != 0)
                {
                    fractalChordStartSteps.Add(i);
                    halfSteps.Add(i);
                    stepsUnclaimed.Remove(i);
                }
            }
            //beats
            int stepsInBeat = masterClock.divisionsPerBeat;
            for (var i = 1; i < stepCount; i++)
            {
                if (i % stepsInBeat == 0 && i % stepsInHalf != 0 && i % stepsInBar != 0)
                {
                    fractalChordStartSteps.Add(i);
                    stepsUnclaimed.Remove(i);
                }
            }
            //offbeats
            int stepsInOff = (int)(masterClock.divisionsPerBeat * 0.5f);
            for (var i = 1; i < stepCount; i++)
            {
                if (i % stepsInOff == 0 && stepsInBeat != 0 && i % stepsInHalf != 0 && i % stepsInBar != 0)
                {
                    fractalChordStartSteps.Add(i);
                    stepsUnclaimed.Remove(i);
                }
            }
            //everything else
            for (var i = 0; i < stepsUnclaimed.Count; i++)
            {
                fractalChordStartSteps.Add(stepsUnclaimed[i]);
            }

            return fractalChordStartSteps;
        }


        Chord[] SortAndTrimSequence(Chord[] sequence, int stepsToFill)
        {
            //Debug.Log("SortAndTrimSequence " + stepsToFill);

            Chord[] dummySequence = new Chord[stepsToFill];
            for (var i = 0; i < dummySequence.Length;i++)
            {
                dummySequence[i] = new Chord();
            }

            for (var i = 0; i < sequence.Length; i++)
            {
                dummySequence[sequence[i].startStep] = sequence[i];
            }

            List<Chord> chordList = new List<Chord>();

            for (var i = 0; i < stepsToFill; i++)
            {
                if (dummySequence[i].init == true)
                {
                    chordList.Add(dummySequence[i]);
                }
            }

            return chordList.ToArray();
        }

        List<int> RemoveStepFromFractalList(List<int> fractalChordStartSteps, int step)
        {
            for (var i = fractalChordStartSteps.Count - 1; i >= 0; i--)
            {
                if (fractalChordStartSteps[i] == step)
                {
                    fractalChordStartSteps.RemoveAt(i);
                }
            }
            return fractalChordStartSteps;
        }
        
        public void GenerateSequence()
        {
            List<int> scaleSteps = ScaleSteps();
            pitches = StepsToPitches(scaleSteps);

            if (useInjectedSequence && injectableSrc != null && injectableSrc.beatInstances != null && injectableSrc.beatInstances.Length > 0)
            {
                GenerateSequenceInjected();
            }
            else
            {
                GenerateSequenceProcGen(scaleSteps.Count);
            }
        }
        BeatInstance[] ValidateInjectedSequence(BeatInstance[] injectedBeatInstances, int targetLength)
        {
            

            if (injectedBeatInstances.Length == targetLength)
            {
                return injectedBeatInstances;
            }

            BeatInstance[] beatInstancesSeqLength = new BeatInstance[targetLength];
            for (var i = 0; i < beatInstancesSeqLength.Length; i++)
            {
                beatInstancesSeqLength[i] = SoundModule.DuplicateBeatInstance(injectedBeatInstances[i % injectedBeatInstances.Length]);
            }

            return beatInstancesSeqLength;
        }

        void GenerateSequenceInjected()
        {
            int targetLength = barsInSequence * masterClock.beatsPerBar;
            BeatInstance[] seqSrc = ValidateInjectedSequence(injectableSrc.beatInstances, targetLength);

            List<Chord> chordList = new List<Chord>();

            for (var i = 0; i <  seqSrc.Length; i++)
            {
                for (var j = 0; j < seqSrc[i].noteFractionOffsets.Count; j++)
                {
                    //Debug.Log("chord depth = " + seqSrc[i].noteFractionOffsets.Count);
                    int noteFracToStep = (int)((i * masterClock.divisionsPerBeat) + (seqSrc[i].noteFractionOffsets[j] * masterClock.divisionsPerBeat));
                    //does chordList have an entry with this time stamp? 
                    int chordTimeStampIndex = DoChordsHaveTimeStamp(chordList, noteFracToStep);
                    //Debug.Log("chordTimeStampIndex" + chordTimeStampIndex);
                    //if so, add teh note to that entry (if unique pitch
                    if (chordTimeStampIndex >= 0)
                    {
                        //if the chord doesn't have this pitch, slot in as an extension
                        //could sort these by pitch, but not sorting adds free-range inversions
                        if (!chordList[chordTimeStampIndex].chord.Contains(seqSrc[i].pitches[j]))
                        {
                            chordList[chordTimeStampIndex].chord.Add(seqSrc[i].pitches[j]);
                        }
                    }
                    //else, add a new chord
                    {
                        chordList.Add(NewChordFromMIDI(seqSrc[i].pitches[j], noteFracToStep));
                    }
                }
            }
            //loop through the stored pitches and pull them into a tigther range if they're far from zero
            chordList = NormalisePitchRange(chordList);

            chordList = ComputeChordDurations(chordList, targetLength);


            chordSequence = chordList.ToArray();
            chordSequencePerStepRef = ChordSequencePerStepRefList(chordSequence);
        }
        List<Chord> ComputeChordDurations(List<Chord> chordList, int beatCount)
        {
            int targetLength = beatCount * masterClock.divisionsPerBeat;

            for (var i = 0; i < chordList.Count; i++)
            {
                if (i == chordList.Count - 1)
                {
                    //Debug.Log("extending final note to end, startStep = " + chordList[i].startStep + " targetLength = " + targetLength);
                    chordList[i].stepDuration = targetLength - chordList[i].startStep;
                }
                else
                {
                    int nextStep = chordList[i + 1].startStep;
                    chordList[i].stepDuration = nextStep - chordList[i].startStep;
                }
            }

            return chordList;
        }

        List<Chord> NormalisePitchRange(List<Chord> chordList)
        {
            float lowBound = 0.5f;
            float highBound = 1.85f;

            float avg = 0;

            for (var i = 0;i < chordList.Count; i++)
            {
                avg += chordList[i].chord[0];
            }
            avg /= chordList.Count;

            if (avg < lowBound)
            {
                for (var i = 0; i < chordList.Count; i++)
                {
                    for (var j = 0; j < chordList[i].chord.Count; j++)
                    {
                        chordList[i].chord[j] *= 2f;
                    }
                }
            }
            if (avg > highBound)
            {
                for (var i = 0; i < chordList.Count; i++)
                {
                    for (var j = 0; j < chordList[i].chord.Count; j++)
                    {
                        chordList[i].chord[j] *= 0.5f;
                    }
                }
            }

            return chordList;
        }

        int DoChordsHaveTimeStamp(List<Chord> chordList, int timeStamp )
        {
            for (int i = 0;i < chordList.Count; i++)
            {
                if (chordList[i].startStep == timeStamp)
                {
                    return i;
                }
            }

            return -1;
        }

        void GenerateSequenceProcGen(int scaleStepsCount)
        {
            

            chordSequence = new Chord[Random.Range(chordCount.x, chordCount.y)];

            int stepsToFill = barsInSequence * masterClock.beatsPerBar * masterClock.divisionsPerBeat;

            //first chord goes at start of sequence.
            int firstIndex = 0;
            if (!startOnRootChord)
            {
                firstIndex = Random.Range(0, scaleStepsCount);
            }
            chordSequence[0] = NewChord(firstIndex, 0, stepsToFill);

            int numchordsOnBars = (int)Mathf.Min(chordSequence.Length * proportionChordsBeatOne, barsInSequence);

            List<int> fractalChordStartSteps = FractalChordStartSteps();
            List<int> barStartSteps = OneDropChordStartSteps();

            int startStep = 0;
            float chordSwitchOrder = 4;
            for (var i = 1; i < chordSequence.Length; i++)
            {
                if (i < numchordsOnBars)
                {
                    int barIndex = Random.Range(0, barStartSteps.Count);
                    startStep = barStartSteps[barIndex];
                    barStartSteps.RemoveAt(barIndex);
                }
                else
                {
                    int fractalLookUp = (int)(Mathf.Pow(Random.Range(0.0f, 1.0f), chordSwitchOrder) * fractalChordStartSteps.Count);
                    startStep = fractalChordStartSteps[fractalLookUp];
                    fractalChordStartSteps = RemoveStepFromFractalList(fractalChordStartSteps, startStep);
                }
                chordSequence[i] = NewChord(Random.Range(0, scaleStepsCount), startStep, 0);
            }

            //chordSequence needs to be sorted by start step and have dupe triggers removed
            chordSequence = SortAndTrimSequence(chordSequence, stepsToFill);


            //add durations to chords in sequence
            for (var i = 0; i < chordSequence.Length; i++)
            {
                if (i < chordSequence.Length - 1)
                {
                    chordSequence[i].stepDuration = chordSequence[i + 1].startStep - chordSequence[i].startStep;
                }
                else
                {
                    chordSequence[i].stepDuration = stepsToFill - chordSequence[i].startStep;
                }
            }

            //the root chord has to be featured in the sequence
            bool hasRoot = false;
            for (var i = 0; i < chordSequence.Length; i++)
            {
                if (chordSequence[i].chord[0] == 1)
                {
                    hasRoot = true;
                }
            }
            if (!hasRoot)
            {
                int targetIndex = Random.Range(0, chordSequence.Length);
                chordSequence[targetIndex].chord[0] = pitches[targetIndex];
            }

            extensionStackInUse = chordExtensionStacks[Random.Range(0, chordExtensionStacks.Count)];
            //add extenstion to all the chords. Not too worried about doing this 'well' just yet
            for (var i = 0; i < chordSequence.Length; i++)
            {
                int extensionCountThisChord = Random.Range(extensionCount.x, extensionCount.y);

                ChordExtensionStack extensionSet = extensionStackInUse;
                if (!homogenousExtensions)
                {
                    extensionSet = chordExtensionStacks[Random.Range(0, chordExtensionStacks.Count)];
                }


                for (var j = 0; j < extensionCountThisChord && j < extensionSet.chordExtensions.Count; j++)
                {
                    int root = chordSequence[i].rootIndex;
                    //extension has -1 applied as they're stored with array starting 0,
                    // but it's easier to write them in the UI as typical chord tones starting I
                    chordSequence[i].chord.Add(pitches[root + extensionSet.chordExtensions[j] - 1]);
                }
            }

            ///an extra array marking the current chord for every step... easier for instruments to look into
            chordSequencePerStepRef = ChordSequencePerStepRefList(chordSequence);
        }

        Chord[] ChordSequencePerStepRefList(Chord[] chordSequence)
        {
            List<Chord> chordSequencePerStepRefList = new List<Chord>();

            for (var i = 0; i < chordSequence.Length; i++)
            {
                Chord thisChord = chordSequence[i];
                int thisDuration = thisChord.stepDuration;
                for (var j = 0; j < thisDuration; j++)
                {
                    chordSequencePerStepRefList.Add(thisChord);
                }
            }
            return chordSequencePerStepRefList.ToArray();
        }


        Chord NewChordFromMIDI(float pitch, int start)
        {
            Chord chord = new Chord();
            chord.rootIndex = 0;
            chord.chord = new List<float>();
            chord.chord.Add(pitch);
            chord.startStep = start;
            chord.stepDuration = -1;
            chord.init = true;

            return chord;
        }


        Chord NewChord(int root, int start, int duration)
        {
            Chord chord = new Chord();
            chord.rootIndex = root;
            chord.chord = new List<float>();
            chord.chord.Add(pitches[root]);
            chord.startStep = start;
            chord.stepDuration = duration;
            chord.init = true;

            return chord;
        }

        public float[] StepsToPitches(List<int> scaleSteps)
        {
            List<float> pitchList = new List<float>();

            pitchList.Add(1);
            float semiToneMult = 1.059463f;
            for (var i = 1; i < scaleSteps.Count; i++)
            {
                float thisMult = Mathf.Pow(semiToneMult, scaleSteps[i]);

                pitchList.Add(pitchList[i - 1] * thisMult);
            }
            //extend it an octave
            for (var i = 0; i < scaleSteps.Count; i++)
            {
                pitchList.Add(pitchList[i] * 2);
            }
            //ah go on one more
            for (var i = 0; i < scaleSteps.Count; i++)
            {
                pitchList.Add(pitchList[i] * 4);
            }
            return pitchList.ToArray();
        }

        public static float GetTranspose(float transpose)
        {
            float semiToneMult = 1.059463f;
            float offset = Mathf.Pow(semiToneMult, Mathf.Abs(transpose));
            if (transpose < 0)
            {
                //Debug.Log("returning transpose val = " + (2 - offset));
                return 2 - offset;
            }
            else
            {
                //Debug.Log("returning transpose val = " + offset);
                return offset;
            }
        }


        public List<int> ScaleSteps()
        {
            List<int> scaleSteps = new List<int>();

            if (scale == TonalScale.Ionian)
            {
                scaleSteps = ScaleBuilder(0, StepsMaj());
            }
            if (scale == TonalScale.Dorian)
            {
                scaleSteps = ScaleBuilder(1, StepsMaj());
            }
            if (scale == TonalScale.Phrygian)
            {
                scaleSteps = ScaleBuilder(2, StepsMaj());
            }
            if (scale == TonalScale.Lydian)
            {
                scaleSteps = ScaleBuilder(3, StepsMaj());
            }
            if (scale == TonalScale.Myxoldian)
            {
                scaleSteps = ScaleBuilder(4, StepsMaj());
            }
            if (scale == TonalScale.Aeolian)
            {
                scaleSteps = ScaleBuilder(5, StepsMaj());
            }
            if (scale == TonalScale.Locrian)
            {
                scaleSteps = ScaleBuilder(6, StepsMaj());
            }
            if (scale == TonalScale.Diminished)
            {
                scaleSteps.Add(0);
                scaleSteps.Add(2);
                scaleSteps.Add(1);
                scaleSteps.Add(2);
                scaleSteps.Add(1);
                scaleSteps.Add(2);
                scaleSteps.Add(1);
            }
            if (scale == TonalScale.MelodicMinor)
            {
                scaleSteps = ScaleBuilder(0, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorDorianb2)
            {
                scaleSteps = ScaleBuilder(1, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorLydianAug)
            {
                scaleSteps = ScaleBuilder(2, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorLydianDom)
            {
                scaleSteps = ScaleBuilder(3, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorMyxoldianb6)
            {
                scaleSteps = ScaleBuilder(4, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorLocrian)
            {
                scaleSteps = ScaleBuilder(5, StepsMelodMin());
            }
            if (scale == TonalScale.MelodicMinorAltered)
            {
                scaleSteps = ScaleBuilder(6, StepsMelodMin());
            }
            if (scale == TonalScale.WholeTone)
            {
                scaleSteps.Add(0);
                scaleSteps.Add(2);
                scaleSteps.Add(2);
                scaleSteps.Add(2);
                scaleSteps.Add(2);
                scaleSteps.Add(2);
            }

            return scaleSteps;
        }

        public List<int> ScaleBuilder(int startOffset, List<int> steps)
        {
            List<int> scaleSteps = new List<int>();

            scaleSteps.Add(0);
            for (var i = 1; i < steps.Count; i++)
            {
                int addIndex = (int)Mathf.Repeat(i + startOffset, steps.Count);
                scaleSteps.Add(steps[addIndex]);
            }

            for (var i = 0; i < scaleSteps.Count; i++)
            {
                //Debug.Log("Scale Check = " + scaleSteps[i]);
            }
            return scaleSteps;
        }

        public List<int> StepsMaj()
        {
            return new List<int> { 1, 2, 2, 1, 2, 2, 2 };
        }

        public List<int> StepsMelodMin()
        {
            return new List<int> { 1, 2, 1, 2, 2, 2, 2 };
        }
    }
}