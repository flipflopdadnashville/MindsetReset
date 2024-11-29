using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;


namespace AutoMusic
{
    public class InjectableGenerator : Generator
    {
        [Header("Injectable Generator Settings")]
        public bool active = true;
        [Tooltip("Set to -1 to randomly pick a stored sequence, else will play the sequence at the given index (when next regenerating")]
        public int sequenceIndex = -1;
        [Tooltip("Enable to lock this device to a given sequenceIndex & omit from being shuffled by the MasterClock regeneration process")]
        public bool manualRegenOnly;
        public MasterClock masterClock;



        public virtual void GenerateInjectable()
        {

        }
    }
}