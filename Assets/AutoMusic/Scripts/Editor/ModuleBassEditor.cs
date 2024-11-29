using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(ModuleBass))]
    [CanEditMultipleObjects]
    public class ModuleBassEditor : ModuleEditor 
    {
        override public void OnInspectorGUI()
        {
            var module = target as ModuleBass;

            BeginModuleGUI(module);


            CoreModuleGUI(module, true, true, true, true, true, true, true, true);
            EditorGUILayout.Space(10);
            BassGUI(module);


            EndModuleGUI(module);
        }
    }
}