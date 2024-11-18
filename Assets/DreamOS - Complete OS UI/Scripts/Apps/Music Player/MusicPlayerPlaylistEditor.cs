#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(MusicPlayerPlaylist))]
    public class MusicPlayerPlaylistEditor : Editor
    {
        Texture2D tmeLogo;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Music Playlist Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Music Playlist Light");
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

            var coverImage = serializedObject.FindProperty("coverImage");
            var playlistName = serializedObject.FindProperty("playlistName");

            GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent("Cover Image"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(coverImage, new GUIContent(""));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent("Playlist Name"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(playlistName, new GUIContent(""));

            GUILayout.EndHorizontal();
            GUILayout.Space(18);

            var playlist = serializedObject.FindProperty("playlist");

            GUILayout.Label("PLAYLIST", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(playlist, new GUIContent("Playlist Content"), true);
            playlist.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new music", customSkin.button))
                playlist.arraySize += 1;

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif