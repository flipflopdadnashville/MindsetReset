#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(VideoPlayerLibrary))]
    public class VideoPlayerLibraryEditor : Editor
    {
        Texture2D tmeLogo;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Video Library Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Video Library Light");
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;

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

            var videos = serializedObject.FindProperty("videos");

            GUILayout.Label("VIDEOS", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(videos, new GUIContent("Video List"), true);
            videos.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new video", customSkin.button))
                videos.arraySize += 1;

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif