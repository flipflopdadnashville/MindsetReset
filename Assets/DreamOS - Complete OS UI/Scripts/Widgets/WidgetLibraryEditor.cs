#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(WidgetLibrary))]
    public class WidgetLibraryEditor : Editor
    {
        Texture2D tmeLogo;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Widget Library Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Widget Library Light");
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light");

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(tmeLogo, GUILayout.Width(250), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);

            var widgets = serializedObject.FindProperty("widgets");

            GUILayout.Label("WIDGETS", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(widgets, new GUIContent("Widget List"), true);
            widgets.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new widget", customSkin.button))
                widgets.arraySize += 1;

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif