#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(WebBrowserLibrary))]
    public class WebBrowserLibraryEditor : Editor
    {
        Texture2D tmeLogo;
        private WebBrowserLibrary libraryTarget;
        WebBrowserLibrary.DownloadableFiles wbl;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Web Library Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Web Library Light");

            libraryTarget = (WebBrowserLibrary)target;
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

            var webPages = serializedObject.FindProperty("webPages");
            var dlFiles = serializedObject.FindProperty("dlFiles");

            GUILayout.Label("WEB PAGES", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(webPages, new GUIContent("Page List"), true);
            webPages.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new page", customSkin.button))
                webPages.arraySize += 1;

            GUILayout.EndVertical();
            GUILayout.Space(14);
            GUILayout.Label("DOWNLOADABLE FILES", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(dlFiles, new GUIContent("File List"), true);
            dlFiles.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new file", customSkin.button))
                dlFiles.arraySize += 1;

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif