using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AutoMusic;

namespace AutoMusic
{
    [CustomEditor(typeof(Module3x3))]
    [CanEditMultipleObjects]
    public class Module3x3Editor : ModuleEditor 
    {
        override public void OnInspectorGUI()
        {
            var module = target as Module3x3;

            BeginModuleGUI(module);


            CoreModuleGUI(module, false, true);
            EditorGUILayout.Space(10);
            m3x3GUI(module);


            EndModuleGUI(module);
        }
    }
}