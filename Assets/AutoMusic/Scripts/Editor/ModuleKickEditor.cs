using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(ModuleKick))]
    [CanEditMultipleObjects]
    public class ModuleKickEditor : ModuleEditor 
    {
        override public void OnInspectorGUI()
        {
            var module = target as ModuleKick;

            BeginModuleGUI(module);


            CoreModuleGUI(module, true, false, false, true, true, true, true, true);
            EditorGUILayout.Space(10);
            KickGUI(module);


            EndModuleGUI(module);
        }
    }
}