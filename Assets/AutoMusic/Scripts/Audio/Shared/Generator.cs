using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoMusic;


namespace AutoMusic
{

    public class Generator : MonoBehaviour
    {
        public BeatInstance[] beatInstances;
        [HideInInspector] public bool pendingPostProcess;
    }
}