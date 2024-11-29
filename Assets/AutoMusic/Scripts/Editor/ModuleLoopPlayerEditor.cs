using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(ModuleLoopPlayer))]
    [CanEditMultipleObjects]
    public class ModuleLoopPlayerEditor : ModuleEditor
    {
        override public void OnInspectorGUI()
        {
            var module = target as ModuleLoopPlayer;

            BeginModuleGUI(module);


            CoreModuleGUI(module, true, true, false, false, true, false, false);
            EditorGUILayout.Space(10);
            LoopGUI(module);


            EndModuleGUI(module);
        }
    }
}