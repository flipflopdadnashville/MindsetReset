using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(ModuleChords))]
    [CanEditMultipleObjects]
    public class ModuleChordsEditor : ModuleEditor 
    {
        override public void OnInspectorGUI()
        {
            var module = target as ModuleChords;

            BeginModuleGUI(module);


            CoreModuleGUI(module, true, true, true);
            EditorGUILayout.Space(10);
            ChordsGUI(module);


            EndModuleGUI(module);
        }
    }
}