using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(ModuleSnare))]
    [CanEditMultipleObjects]
    public class ModuleSnareEditor : ModuleEditor 
    {
        override public void OnInspectorGUI()
        {
            var module = target as ModuleSnare;

            BeginModuleGUI(module);


            CoreModuleGUI(module, true, false, false, true, true, true, true, true);
            EditorGUILayout.Space(10);
            SnareGUI(module);


            EndModuleGUI(module);
        }
    }
}