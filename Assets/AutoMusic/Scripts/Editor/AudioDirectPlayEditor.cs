using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using AutoMusic;

namespace AutoMusic
{
    public class AudioDirectPlayEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var module = target as AudioDirectPlay;

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(module, "Edit " + module);



            EditorUtility.SetDirty(module);
            serializedObject.ApplyModifiedProperties();
        }
    }
}