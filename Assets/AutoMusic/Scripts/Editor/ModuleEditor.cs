using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AutoMusic;

namespace AutoMusic
{
    public class ModuleEditor : Editor
    {
        GUIContent active = new GUIContent("Active", "Is this module active? note : this value is typically controlled by a CompositionHub. You can also disable the modules gameObject to mute an instrument");
        GUIContent useInjectedSequence = new GUIContent("Use Injected Sequence", "Injected Sequences are sequence that are inserted into the object manually (or through MIDI), enabling this box will use that sequence if available");
        GUIContent injectableSrc = new GUIContent("Injectable Source", "The source Injectable Generator to grab from when useInjectedSequence == true. Can be hot-swapped during playmode to switch between sources");
        GUIContent outputMode = new GUIContent("Output Mode", "You're probably best off with DirectSound. Please refer to documentation as this control affects a lot of possibilities");
        GUIContent voiceCount = new GUIContent("Voice Count", "Number of Audio Source components to instantiate for this device to function");
        GUIContent useFilter = new GUIContent("Use Filter", "Add a resonant low-pass filter to this module. Also forces the sound to be mono (performance reasons)");
        GUIContent useFilterCurve = new GUIContent("Use Filter Curve", "Active curves for filter freq & resonance that are repeated x times over the duration of each music section");
        GUIContent useAmpEnvelope = new GUIContent("Use Amp Envelope", "Enable an Attack-Decay-Sustain-Release envelope to control the volume curve of each triggered note. Durations are in seconds");
        GUIContent sidechainCompression = new GUIContent("Sidechain Compression", "Amount of sidechain compression effect to apply to this deive. Master setting sfor the sidechain compressor are in the FXHub component");
        GUIContent audioClip = new GUIContent("Audio Clip", "Primary audioclip to play for notes triggered by this module");
        GUIContent clipVariationMode = new GUIContent("Clip Variation Mode", "Clip Variation modes allow for triggering different clips when certain conditions are met. See documtation for full details");
        GUIContent varClipsGUI = new GUIContent("AudioClip Variations", "Array of AudioClips to select from when rnadomly choosing a variant clip to play");
        GUIContent clipVariationChanceCurve = new GUIContent("Clip Variation Chance Curve", "Curve that controls likelihood of choosing variant clips. TIme along the x axis, chnace on y. Evaluated along the duration of a music section");
        GUIContent pitchMultiplier = new GUIContent("Pitch Multiplier", "Raw pitch multiplier to apply to clips (alongisde their musical tuning). 1 is default speed, 0.5 is half speed, etc");
        GUIContent pitchOffsetSemitones = new GUIContent("Pitch transpose Semitones", "Transpose the clips pitch by a precise musical semi-tone value");
        GUIContent loopBarCount = new GUIContent("Loop Bar Count", "Number of bars of music to generate. This will then be repeated (or truncated) to form a full music section. Some properties (fills, chord pitching, clip variations) can modify beyond a single loop duration");
        GUIContent swingMultiplier = new GUIContent("Swing Multiplier", "How much of the swing parameter (as set in the Master Clock) to apply to this module");
        GUIContent velocityCurve = new GUIContent("Velocity Curve", "Veclocity curves are set in the Master Clock, this value selects which velocity curve index to use");
        GUIContent velocityInfluence = new GUIContent("Velocity Influence", "AMount of the velocity curve to apply to note output volume (and filtering, is use Filter is enabled)");
        GUIContent humaniseOffsetMilliseconds = new GUIContent("Humanise Offset Milliseconds", "Amount of 'humanisation' to apply to note timings: rnadom offset in millseconds");
        GUIContent densityTrimInnerLoop = new GUIContent("Density Trim Inner Loop", "This loop runs along the duration of each loop : for points in time where the curve is >= 1, all notes will be let through. Where the curve is <= 0, no notes are let through. Between those values, a corresponding random percentage of the notes will play. \r\nAn example use case for this is having a 2 bar loop where you assign a curve that only lets notes through for the 1st bar in each loop");
        GUIContent densityTrimFullSequence = new GUIContent("Density Trim Full Sequence", "As above, but instead of per-loop, this curve spans the duration of the entire master section. \r\nAn example for this is a curve that slopes up from 0 to 1, making for an increasingly dense output sequence over the duration of a 16 bar section");
        GUIContent ampEnvToFilter = new GUIContent("Amp Env To Filter", "Amount of the amp envelope output to also send to the filter frequency value");

        public void CoreModuleGUI(SoundModule module, bool displayClip = true, bool displayPitch = true,
        bool displayEnvelope = false, bool displayTrims = true, bool displayOutModeMode = true,
        bool displayHumanise = true, bool displaySequenceInjection = true, bool displayVariationClips = false)
        {
            

            module.active = EditorGUILayout.Toggle(active, module.active);
            EditorGUILayout.Space(5);
            module.useInjectedSequence = EditorGUILayout.Toggle(useInjectedSequence, module.useInjectedSequence);
            if (module.useInjectedSequence)
            {
                if (module.injectableSrc == null)
                {
                    EditorGUILayout.HelpBox(
                                    "'Use Injected Sequence' requires an input added to 'Injectable Source': Add an InjectableGenerator component (MIDIToBeatInstance)",
                                    MessageType.Info);
                }
                module.injectableSrc = (InjectableGenerator)EditorGUILayout.ObjectField(injectableSrc, module.injectableSrc, typeof(InjectableGenerator), true);
            }

            EditorGUILayout.Space(5);

            GUILayout.Label("Core Properties", EditorStyles.boldLabel);
            
            EditorGUI.indentLevel++;
            if (module.outputMode == SoundOutputMode.AudioSources)
            {
                module.voiceCount = EditorGUILayout.IntSlider(voiceCount, module.voiceCount, 1, 24);
                EditorGUILayout.HelpBox(
                    "This output mode will be deprecated, consider switching to DirectSound output mode",
                    MessageType.Info);
            }
            if (module.outputMode == SoundOutputMode.DirectSound)
            {
                
                if (displayEnvelope)
                {
                    EditorGUILayout.Space(10);
                    module.useAmpEnvelope = EditorGUILayout.Toggle(useAmpEnvelope, module.useAmpEnvelope);
                    SerializedProperty envState = serializedObject.FindProperty("useAmpEnvelope");
                    if (envState.boolValue)
                    {
                        EditorGUI.indentLevel++;
                            module.envAmpAttack = EditorGUILayout.Slider("Env Amp Attack", module.envAmpAttack, 0f, 3f);
                            module.envAmpDecay = EditorGUILayout.Slider("Env Amp Decay", module.envAmpDecay, 0f, 3f);
                            module.envAmpSustain = EditorGUILayout.Slider("Env Amp Sustain", module.envAmpSustain, 0f, 1f);
                            module.envAmpRelease = EditorGUILayout.Slider("Env Amp Release", module.envAmpRelease, 0f, 3f);
                        EditorGUI.indentLevel--;
                    }
                }
                
            }
            EditorGUILayout.Space(10);


                if (displayClip)
                {
                    GUILayout.Label("AudioClip Properties", EditorStyles.boldLabel);
                    module.audioClip = (AudioClip)EditorGUILayout.ObjectField(audioClip, module.audioClip, typeof(AudioClip), true);

                }
                if (displayPitch)
                {
                    module.pitchMultiplier = EditorGUILayout.FloatField(pitchMultiplier, module.pitchMultiplier);
                    module.pitchOffsetSemitones = EditorGUILayout.IntField(pitchOffsetSemitones, module.pitchOffsetSemitones);
                }

            EditorGUILayout.Space(5);


            if (!UsingInjection(module))
            {
                GUILayout.Label("Timing Properties", EditorStyles.boldLabel);
                module.velocityInfluence = EditorGUILayout.Slider(velocityInfluence, module.velocityInfluence, 0.0f, 1f);
                module.loopBarCount = EditorGUILayout.Slider(loopBarCount, module.loopBarCount, 0.5f, 16f);
                if (module.loopBarCount >= 1)
                {
                    module.loopBarCount = Mathf.FloorToInt(module.loopBarCount);
                }
                if (module.loopBarCount < 1)
                {
                    module.loopBarCount = 0.5f;
                }
               

                if (displayTrims)
                {
                    EditorGUILayout.Space(10);

                    module.densityTrimInnerLoop = EditorGUILayout.CurveField(densityTrimInnerLoop, module.densityTrimInnerLoop);
                }
            }


            module.SanitiseInputCore();
        }

        GUIContent fillIntensity = new GUIContent("Fill Intensity", "How much generated patterns will be modified by the 'fill' funcitionality, FIlls are triggered every n bars, as set in the Master Clock");
        GUIContent rythmLogic = new GUIContent("Rhythm Logic", "Algorithm used to generate rhythms. See documentation for full details");
        GUIContent baseRhythmicDistortion = new GUIContent("Base Rhythmic Disorder", "How likely generated notes are to land off beat (notes are still quantized to the step grid)");
        GUIContent baseRhythmicDensity = new GUIContent("Base Rhythmic Density", "Min and Max number of notes to generate in each music beat");
        GUIContent repeatStackGUI = new GUIContent("Repeat Stack", "Each time a note is generated, the system counts ahead a number of steps corresponding to one of these entries then places another note (repeats process until loop duration is reached)");
        GUIContent repeatStackPowerFunction = new GUIContent("Repeat Stack Power Function", "Higher values make it increasingly likely for earlier entries in the repeatStack array to be chosen over later entries");


        bool UsingInjection(SoundModule module)
        {
            if (!module.useInjectedSequence)
            {
                return false;
            }
            if (module.injectableSrc == null)
            {
                return false;
            }

            return true;
        }
        public void KickGUI(ModuleKick module)
        {
            if (!UsingInjection(module))
            {
                GUILayout.Label("Kick Properties", EditorStyles.boldLabel);
                module.fillIntensity = EditorGUILayout.Slider(fillIntensity, module.fillIntensity, 0.0f, 100f);

                module.rythmLogic = (KickRhythmLogic)EditorGUILayout.EnumPopup(rythmLogic, module.rythmLogic);

                EditorGUI.indentLevel++;
                if (module.rythmLogic == KickRhythmLogic.DensityDisorder)
                {
                    module.baseRhythmicDistortion = EditorGUILayout.Slider(baseRhythmicDistortion, module.baseRhythmicDistortion, 0.0f, 100f);
                    module.baseRhythmicDensity = EditorGUILayout.Vector2Field(baseRhythmicDensity, module.baseRhythmicDensity);
                }
                if (module.rythmLogic == KickRhythmLogic.EuclidVariation)
                {
                    SerializedProperty repeatStack = serializedObject.FindProperty("repeatStack");
                    EditorGUILayout.PropertyField(repeatStack, repeatStackGUI);
                    module.repeatStackPowerFunction = EditorGUILayout.Slider(repeatStackPowerFunction, module.repeatStackPowerFunction, 0.5f, 8f);
                }
                EditorGUI.indentLevel--;
            }
            module.SanitiseInput();
        }

        GUIContent timeSliceSyncPerSteps = new GUIContent("Time Slice Sync Per Steps", "The number of steps to wait between re-syncing the audioclip. \r\nThis is a trade off between accurately conforming to BPM changes, and potential for small ‘clicks’ to appear between slices. If you are planning to play your loop back at a tempo different from what it was recorded at, this value should be set to the smallest unit of time represented in the clip.");
        public void LoopGUI(ModuleLoopPlayer module)
        {
            

            module.SanitiseInput();
        }

        GUIContent beatsUntilStart = new GUIContent("Beats Until Start", "Number of music beats to wait before playing the first note");
        GUIContent halfTime = new GUIContent("Half Time", "Generate sequences at half the speed of the values entered in the repeat stack/ beats until start values");
        public void SnareGUI(ModuleSnare module)
        {
            if (!UsingInjection(module))
            {
                GUILayout.Label("Snare Properties", EditorStyles.boldLabel);

                module.beatsUntilStart = EditorGUILayout.FloatField(beatsUntilStart, module.beatsUntilStart);
                module.fillIntensity = EditorGUILayout.Slider(fillIntensity, module.fillIntensity, 0.0f, 100f);
                module.halfTime = EditorGUILayout.Toggle(halfTime, module.halfTime);

                SerializedProperty repeatStack = serializedObject.FindProperty("snareRepeatStack");
                EditorGUILayout.PropertyField(repeatStack, repeatStackGUI);
                module.repeatStackPowerFunction = EditorGUILayout.Slider(repeatStackPowerFunction, module.repeatStackPowerFunction, 0.5f, 8f);
            }
            module.SanitiseInput();
        }

        GUIContent harmonicHub = new GUIContent("Harmonic Hub", "Harmonic Hub must be connected for pitch-based modules to operate correctly");
        GUIContent loopSound = new GUIContent("Loop Sound", "Enable to make the sound loop for the duration of the tirggered notes. For long, sustained chords. The audio clip in use should be created with this in mind. Combines well with envelope & filter modes");
        GUIContent chordExtensionRange = new GUIContent("Chord Extension Range", "Combines with the chord system defined in the Harmonic Hub : How high up the chord extension array to read when playing notes");
        GUIContent rhythmMode = new GUIContent("Rhythm Mode", "Algorithm used to generate rhythms. See documentation for full details");
        GUIContent staggerStepsGUI = new GUIContent("Stagger Steps", "Each staggered note output will be delayed by one of these numbers of steps with each retrigger");
        GUIContent staggerRepeats = new GUIContent("Stagger Repeats", "How many staggered notes to play after each chord change");
        GUIContent rhythmSource = new GUIContent("Rhythm Source", "Sound Module to use as the rhythm source : This module will follow the timing of that module, with changes based on teh following two parameters");
        GUIContent deletionsFromRhythmSource = new GUIContent("Deletions From Rhythm Source", "Random percentage of notes to remove from the incoming rhythm");
        GUIContent addDelaysGUI = new GUIContent("Additional Delays From Rhythm Source", "Each incoming note will be delayed by one of these numbers of steps with each retrigger");
        public void ChordsGUI(ModuleChords module)
        {
            GUILayout.Label("Chord Properties", EditorStyles.boldLabel);
            module.harmonicHub = (HarmonicHub)EditorGUILayout.ObjectField(harmonicHub, module.harmonicHub, typeof(HarmonicHub), true);
            module.loopSound = EditorGUILayout.Toggle(loopSound, module.loopSound);

            if (!UsingInjection(module))
            {
                module.chordExtensionRange = EditorGUILayout.Vector2IntField(chordExtensionRange, module.chordExtensionRange);

                module.rhythmMode = (ChordsRhythmMode)EditorGUILayout.EnumPopup(rhythmMode, module.rhythmMode);

                if (module.rhythmMode == ChordsRhythmMode.StaggerChordUp || module.rhythmMode == ChordsRhythmMode.StaggerChordDown || module.rhythmMode == ChordsRhythmMode.StaggerChordBi)
                {
                    EditorGUI.indentLevel++;
                    SerializedProperty staggerSteps = serializedObject.FindProperty("staggerSteps");
                    EditorGUILayout.PropertyField(staggerSteps, staggerStepsGUI);
                    module.staggerRepeats = EditorGUILayout.IntSlider(staggerRepeats, module.staggerRepeats, 0, 128);
                    EditorGUI.indentLevel--;
                }
                if (module.rhythmMode == ChordsRhythmMode.FollowOther)
                {
                    EditorGUI.indentLevel++;
                    module.rhythmSource = (SoundModule)EditorGUILayout.ObjectField(rhythmSource, module.rhythmSource, typeof(SoundModule), true);
                    module.deletionsFromRhythmSource = EditorGUILayout.Slider(deletionsFromRhythmSource, module.deletionsFromRhythmSource, 0.0f, 100f);
                    SerializedProperty addDelays = serializedObject.FindProperty("additionalDelaysFromRhythmSource");
                    EditorGUILayout.PropertyField(addDelays, addDelaysGUI);
                    EditorGUI.indentLevel--;
                }
            }
            module.SanitiseInput();
        }

        GUIContent backGhostsGUI = new GUIContent("Back Ghosts", "Array of step values to determine how far back in time (in musical steps) each generated ghost note may be");
        GUIContent forwardGhostsGUI = new GUIContent("Forward Ghosts", "Array of step values to determine how far forward in time (in musical steps) each generated ghost note may be");
        GUIContent arpeggiation = new GUIContent("Arpeggiation", "Ratio of notes that will play pitches other than the current root note established in the Harmonic Hub. Un-arped notes will play back as either the root note of the current chord or the root note of the key, depending on pitch logic settings ");
        GUIContent rhythmicIntensityBack = new GUIContent("Rhythmic Intensity Back", "Ratio of incoming notes to 'delay' backwards in time, creating reduced-velocity notes earlier in time than the source note");
        GUIContent rhythmicIntensityForward = new GUIContent("Rhythmic Intensity Forward", "Ratio of incoming notes to delay forwards in time, creating reduced-velocity notes later in time than the source note");
        GUIContent iterations = new GUIContent("Ghost Iterations", "Amount of times to iterate/loop throuhg each generation of ghost notes, generating repeats of repeats");
        GUIContent rollChance = new GUIContent("Roll Chance", "Chance for each generated ghost note to create a rapid roll, dividing each note into a greater number of fast notes by a factor the following control");
        GUIContent rollStepDivisorMax = new GUIContent("Roll Step Divisor Max", "The maximum number of faster notes that each generated ghost note can be divided into");


        GUIContent closedHat = new GUIContent("Closed Hat Clip", "Audio Clip representing the CLOSED hi-hat sound. The closed clip is the default clip played");
        GUIContent openHat = new GUIContent("Open Hat Clip", "Audio Clip representing the OPEN hi-hat sound. Open hat sounds are played on off-beats randomly selected by a factor of 'Offs Are Open Chance'");
        GUIContent offsAreOpenChance = new GUIContent("Offs Are Open Chance", "The ratio of off-beat notes that will use the open-hat audio clip (instead of staying with the default closed clip");
       

        GUIContent randClipGUI = new GUIContent("Audio Clips", "A random selection from this AudioClip array will be selected each time this module is triggered");
       

        
        GUIContent firstStep = new GUIContent("First Step", "The first step in each music section to play a sound. Typically left at 0 to play a high-impact sound at the onset of each section repeat");
      

        GUIContent additionalInbetweensFromRhythmSource = new GUIContent("Additional Inbetweens From Rhythm Source", "A number of additional notes will be inserted rhythmically between incoming notes based on this value");
        GUIContent pitchLogic = new GUIContent("Pitch Logic", "Algorithm used to determine pitch of generated notes. Linked to settings in the Harmonic Hub");
        GUIContent additionalArpOnInbetweens = new GUIContent("Additional Arp On Inbetweens", "Additional inbetween notes will have a boost to their arpeggiation chnace as a factor of this control");
        public void BassGUI(ModuleBass module)
        {
            GUILayout.Label("Bass Properties", EditorStyles.boldLabel);

            module.harmonicHub = (HarmonicHub)EditorGUILayout.ObjectField(harmonicHub, module.harmonicHub, typeof(HarmonicHub), true);
            module.loopSound = EditorGUILayout.Toggle(loopSound, module.loopSound);

            if (!UsingInjection(module))
            {
                module.pitchLogic = (BassPitchLogic)EditorGUILayout.EnumPopup(pitchLogic, module.pitchLogic);
                module.arpeggiation = EditorGUILayout.Slider(arpeggiation, module.arpeggiation, 0.0f, 100f);

                module.rhythmLogic = (BassRhythmLogic)EditorGUILayout.EnumPopup(rythmLogic, module.rhythmLogic);

                if (module.rhythmLogic == BassRhythmLogic.FollowOther)
                {
                    EditorGUI.indentLevel++;
                    module.rhythmSource = (SoundModule)EditorGUILayout.ObjectField(rhythmSource, module.rhythmSource, typeof(SoundModule), true);
                    module.deletionsFromRhythmSource = EditorGUILayout.Slider(deletionsFromRhythmSource, module.deletionsFromRhythmSource, 0.0f, 100f);
                    module.additionalInbetweensFromRhythmSource = EditorGUILayout.Slider(additionalInbetweensFromRhythmSource, module.additionalInbetweensFromRhythmSource, 0.0f, 100f);
                    module.additionalArpOnInbetweens = EditorGUILayout.Slider(additionalArpOnInbetweens, module.additionalArpOnInbetweens, 0.0f, 100f);

                    EditorGUI.indentLevel--;
                }
                if (module.rhythmLogic == BassRhythmLogic.Percish)
                {
                    EditorGUI.indentLevel++;
                    module.baseRhythmicDistortion = EditorGUILayout.Slider(baseRhythmicDistortion, module.baseRhythmicDistortion, 0.0f, 100f);
                    module.baseRhythmicDensity = EditorGUILayout.Vector2Field(baseRhythmicDensity, module.baseRhythmicDensity);
                    EditorGUI.indentLevel--;
                }
                if (module.rhythmLogic == BassRhythmLogic.Snarish)
                {
                    EditorGUI.indentLevel++;
                    module.beatsUntilStart = EditorGUILayout.FloatField(beatsUntilStart, module.beatsUntilStart);
                    SerializedProperty repeatStack = serializedObject.FindProperty("snareRepeatStack");
                    EditorGUILayout.PropertyField(repeatStack, repeatStackGUI);
                    module.repeatStackPowerFunction = EditorGUILayout.Slider(repeatStackPowerFunction, module.repeatStackPowerFunction, 0.5f, 8f);
                    EditorGUI.indentLevel--;
                }
            }

            module.SanitiseInput();
        }

        GUIContent clipsBassGUI = new GUIContent("AudioClips Bass", "The audio clips to play on bass notes. This array should contain exactly 2 entries : the first clip represents 'normal' notes, the second represents 'accented' notes");
        GUIContent clipsOctaveGUI = new GUIContent("AudioClips Octave", "The audio clips to play on upper-octave notes. This array should contain exactly 2 entries : the first clip represents 'normal' notes, the second represents 'accented' notes");
        GUIContent density = new GUIContent("Density", "Random steps are marked as active as a ratio of this parameter");
        GUIContent accents = new GUIContent("Accents", "Random notes are marked as accent (read from the second clip in the bass/octave arrays as a ratio of this parameter");
        GUIContent octaves = new GUIContent("Octaves", "Random notes are marked as octave (read from the Audio Clips Octave array) as a ratio of this parameter. All other notes will use the Bass array");
        GUIContent ties = new GUIContent("Ties", "Random notes are slewed together instead of retriggering hte clip/envelope as a ratio of this parameter");
        GUIContent foldHiPitchesTargetVal = new GUIContent("Fold Hi Pitches Target Val", "Notes that are given pitches that have pitch(speed) multiplier values above this value will instead be 'folded' down to play the same note at the octave below. Prevents chipmunking");
        public void m3x3GUI(Module3x3 module)
        {
            GUILayout.Label("3x3 Properties", EditorStyles.boldLabel);
            module.harmonicHub = (HarmonicHub)EditorGUILayout.ObjectField(harmonicHub, module.harmonicHub, typeof(HarmonicHub), true);

            SerializedProperty clipsBass = serializedObject.FindProperty("clipsBass");
            EditorGUILayout.PropertyField(clipsBass, clipsBassGUI);
            SerializedProperty clipsOctave = serializedObject.FindProperty("clipsOctave");
            EditorGUILayout.PropertyField(clipsOctave, clipsOctaveGUI);

            if (!UsingInjection(module))
            {
                module.rhythmMode = (RhythmMode3x3)EditorGUILayout.EnumPopup(rhythmMode, module.rhythmMode);
                EditorGUI.indentLevel++;
                if (module.rhythmMode == RhythmMode3x3.FollowOther)
                {
                    module.rhythmSource = (SoundModule)EditorGUILayout.ObjectField(rhythmSource, module.rhythmSource, typeof(SoundModule), true);
                    module.deletionsFromRhythmSource = EditorGUILayout.Slider(deletionsFromRhythmSource, module.deletionsFromRhythmSource, 0.0f, 100f);
                    module.additionalInbetweensFromRhythmSource = EditorGUILayout.Slider(additionalInbetweensFromRhythmSource, module.additionalInbetweensFromRhythmSource, 0.0f, 100f);
                    module.additionalArpOnInbetweens = EditorGUILayout.Slider(additionalArpOnInbetweens, module.additionalArpOnInbetweens, 0.0f, 100f);
                }
                if (module.rhythmMode == RhythmMode3x3.Snareish)
                {
                    module.snareBeatsUntilStart = EditorGUILayout.FloatField(beatsUntilStart, module.snareBeatsUntilStart);
                    SerializedProperty repeatStack = serializedObject.FindProperty("snareRepeatStack");
                    EditorGUILayout.PropertyField(repeatStack, repeatStackGUI);
                    module.repeatStackPowerFunction = EditorGUILayout.Slider(repeatStackPowerFunction, module.repeatStackPowerFunction, 0.5f, 8f);
                }
                if (module.rhythmMode == RhythmMode3x3.SixteenthScatters)
                {
                    module.density = EditorGUILayout.Slider(density, module.density, 0.0f, 1.0f);
                    module.accents = EditorGUILayout.Slider(accents, module.accents, 0.0f, 1.0f);
                    module.octaves = EditorGUILayout.Slider(octaves, module.octaves, 0.0f, 0.75f);
                    module.ties = EditorGUILayout.Slider(ties, module.ties, 0.0f, 0.5f);
                }
                EditorGUI.indentLevel--;

                module.pitchLogic = (BassPitchLogic)EditorGUILayout.EnumPopup(pitchLogic, module.pitchLogic);
                module.arpeggiation = EditorGUILayout.Slider(arpeggiation, module.arpeggiation, 0.0f, 1.0f);
                //module.foldHiPitchesTargetVal = EditorGUILayout.Slider(foldHiPitchesTargetVal, module.foldHiPitchesTargetVal, 1.0f, 3.0f);
            }
            module.SanitiseInput();
        }

        public void BeginModuleGUI(SoundModule module)
        {
            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(module, "Edit " + module);
        }

        public void EndModuleGUI(SoundModule module)
        {
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(module);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}