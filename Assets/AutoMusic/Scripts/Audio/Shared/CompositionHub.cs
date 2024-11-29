using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;
using UnityEngine.Serialization;

namespace AutoMusic
{
    [System.Serializable]
    public class ModulatableFx
    {
        [Tooltip("Curve(s) to be animated over the duration of a music section. There should be a matching number of curve and target values")]
        public AnimationCurve[] curve;
        [Tooltip("string corresponding to an exposed parameter in the AudioMixer. There should be a matching number of curve and target values")]
        public string[] target;
    }
    [System.Serializable]
    public class CompositionLayout
    {
        public string sectionName;
        [Tooltip("Core modules that will always be made active by this layout")]
        public SoundModule[] primaryModules;
        [Tooltip("Modules that will sometimes be made active by this layout (chance based on 'secondaryStartActiveChance')")]
        public SoundModule[] secondaryModules;
        [Tooltip("These modules will have a chance of being made active (based on 'repeatsStartActiveChance'), but only after this layotu has gone through at least 1 repetition")]
        public SoundModule[] repeatsOnlyModules;
        [Tooltip("When enabled, this layout will only play once before randomly picking another layout")]
        public bool noRepeat;
        [Tooltip("If this layout has 'noRepeat' mode enabled, only layouts corresponding to these indices in the Composition Layouts array will be available as the following layout")]
        [FormerlySerializedAs("noRpeatAllowedFollowers")]
        public int[] noRepeatAllowedFollowers;
        [Tooltip("chance for secondaryModules to be made active")]
        [Range(0, 100)]
        public float secondaryStartActiveChance = 50f;
        [Tooltip("chance fro repeatsOnlyModules modules to be made active (after the layotu has repeated)")]
        [Range(0, 100)]
        public float repeatsStartActiveChance = 50f;

        //[Header("DebugRef")]
        [HideInInspector] public List<ModulatableFx> activeFX;
    }
    [System.Serializable]
    public class MasterFilter
    {
        public AnimationCurve hp;
        public AnimationCurve lp;
    }
    [System.Serializable]
    public class MasterFX
    {
        public AnimationCurve delay;
        public AnimationCurve reverb;
    }

    public class CompositionHub : MonoBehaviour
    {
        [Header("Device Links")]
        public MasterClock masterClock;
        public SoundModule[] soundModules;

        [Space(10)]

        [Header("Composition Properties")]
        [Tooltip("The composition Layout at this index in the CompositionLayouts array will be loaded when the device next switches layout. Set to -1 to keep random")]
        public int forceNextLayoutIndex = -1;

        [Tooltip("Higher value will tend towards sticking to the same sequence for longer durations")]
        [Range(0, 100)]
        public float harmonicStability = 25f;
        [Tooltip("Higher value will cause this component to maintain the current Composition Layout for longer durations")]
        [Range(0, 100)]
        public float layoutStability = 50f;
        [Tooltip("Higher values will cause the Master Clock to hold onto generated patterns (note data / rhythms etc) for longer durations")]
        [Range(0, 100)]
        [HideInInspector] public float seedStability = 50f;
        [Tooltip("The modifier system allows any number of disabled devices to have their settings retriggered, but active devices being switched out is limited by this value : the function of this is to reduce jarring amounts of change. ")]
        public int activeModifiable = 2;
        [Tooltip("An array of Composition Layouts, which are core building blocks defining musical segments of devices enabled/disabled, and fx to assign or modulate on audio mixer channels")]
        public CompositionLayout[] compositionLayouts;


        //[Header("DebugRef")]
        [HideInInspector] public bool init;
        [HideInInspector] public int currLayoutIndex;
        [HideInInspector] public int assessingBeatIndex;
        [HideInInspector] public bool requestHarmonicChange;
        [HideInInspector] public int masterFilterIndex = -1;
        [HideInInspector] public int masterFXIndex = -1;
        [HideInInspector] public int fadeIndex;
        [HideInInspector] public int forceLayout = -1;
        //public int compsTriggered;


        void Awake()
        {
            masterClock = FindObjectOfType<MasterClock>();
            masterClock.processBeat.AddListener(ProcessBeat);

            InactiveAll();
        }


        public void InactiveAll()
        {
            soundModules = ValidateSoundModuleArray(soundModules);

            for (var i = 0; i < soundModules.Length; i++)
            {
                soundModules[i].active = false;
            }
        }


        void OnValidate()
        {


            activeModifiable = Mathf.Max(0, activeModifiable);
        }



        public SoundModule[] ValidateSoundModuleArray(SoundModule[] sm)
        {
            bool trimNeeded = false;
            for (var i = 0; i < sm.Length; i++)
            {
                if (sm[i] == null)
                {

                    trimNeeded = true;
                }
            }
            if (!trimNeeded)
            {
                return sm;
            }
            List<SoundModule> validatedModules = new List<SoundModule>();
            {
                for (var i = 0; i < sm.Length; i++)
                {
                    if (sm[i] != null)
                    {
                        validatedModules.Add(sm[i]);
                    }
                }
            }

            return validatedModules.ToArray();
        }


        public void GenerateComposition()
        {
            init = true;


            //validate some properties to minimse trouble
            soundModules = ValidateSoundModuleArray(soundModules);

            



            //run modifiers
            ProcessModifiers();

            //set layouts
            if (compositionLayouts[currLayoutIndex].noRepeat || !SoundModule.D100(layoutStability))
            {
                int prevLayout = currLayoutIndex;
                //Debug.Log("rerolling layout, choices = " + compositionLayouts.Length);
                currLayoutIndex = Random.Range(0, compositionLayouts.Length);

                compositionLayouts[currLayoutIndex].noRepeatAllowedFollowers = ValidateNoRepeatAllowedFollowers(compositionLayouts[currLayoutIndex].noRepeatAllowedFollowers);
                if (compositionLayouts[currLayoutIndex].noRepeat)
                {
                    int noRepeatTargetIndex = Random.Range(0, compositionLayouts[prevLayout].noRepeatAllowedFollowers.Length);
                    currLayoutIndex = compositionLayouts[prevLayout].noRepeatAllowedFollowers[noRepeatTargetIndex];
                    //Debug.Log("no repeat compLayout, switching to " + currLayoutIndex);
                }

                bool isRepeat = false;
                if (prevLayout == currLayoutIndex)
                {
                    isRepeat = true;
                }

                TriggerLayout(currLayoutIndex, isRepeat);
            }
            else
            {
                TriggerLayout(currLayoutIndex, true);
            }

            if (masterClock.FXHub != null)
            {
                masterClock.FXHub.initByCompositionHub = true;
            }
        }

        int[] ValidateNoRepeatAllowedFollowers(int[] noRepeatAllowedFollowers)
        {
            for (var i = 0; i < noRepeatAllowedFollowers.Length; i++)
            {
                if (noRepeatAllowedFollowers[i] < 0 || noRepeatAllowedFollowers[i] >= compositionLayouts.Length)
                {
                    noRepeatAllowedFollowers[i] = 0;
                }
            }

            return noRepeatAllowedFollowers;
        }

        void ProcessModifiers()
        {
            return;
        }

        


        void ProcessBeat()
        {
            assessingBeatIndex = masterClock.assessingBeatIndex;

            CompositionLayout comp = compositionLayouts[currLayoutIndex];
            comp.primaryModules = ValidateSoundModuleArray(comp.primaryModules);
            comp.secondaryModules = ValidateSoundModuleArray(comp.secondaryModules);
            comp.repeatsOnlyModules = ValidateSoundModuleArray(comp.repeatsOnlyModules);

        }




        public void TriggerLayout(int layout, bool isRepeat)
        {
            //compsTriggered += 1;
            if (forceLayout >= 0)
            {
                layout = forceLayout;
                currLayoutIndex = forceLayout;
            }
            if (forceNextLayoutIndex >= 0)
            {
                if (forceNextLayoutIndex >= compositionLayouts.Length)
                {
                    forceNextLayoutIndex = -1;
                }
                else
                {
                    layout = forceNextLayoutIndex;
                    currLayoutIndex = forceNextLayoutIndex;
                    forceNextLayoutIndex = -1;
                }
            }

            CompositionLayout comp = compositionLayouts[layout];


            InactiveAll();
            SetCompActiveStates(comp, isRepeat);
        }

        void SetCompActiveStates(CompositionLayout comp, bool isRepeat)
        {
            for (var i = 0; i < comp.primaryModules.Length; i++)
            {
                comp.primaryModules[i].active = true;
            }

            for (var i = 0; i < comp.secondaryModules.Length; i++)
            {
                if (SoundModule.D100(comp.secondaryStartActiveChance))
                {
                    comp.secondaryModules[i].active = true;
                }
            }

            if (isRepeat)
            {
                for (var i = 0; i < comp.repeatsOnlyModules.Length; i++)
                {
                    if (SoundModule.D100(comp.repeatsStartActiveChance))
                    {
                        comp.repeatsOnlyModules[i].active = true;
                    }
                }
            }
        }
    }
}