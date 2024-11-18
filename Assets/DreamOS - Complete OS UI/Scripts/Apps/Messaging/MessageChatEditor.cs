#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(MessageChat))]
    public class MessageChatEditor : Editor
    {
        Texture2D tmeLogo;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Message Chat Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Message Chat Light");
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

            var useDynamicMessages = serializedObject.FindProperty("useDynamicMessages");
            var useStoryTeller = serializedObject.FindProperty("useStoryTeller");

            GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            useDynamicMessages.boolValue = GUILayout.Toggle(useDynamicMessages.boolValue, new GUIContent("Use Dynamic Messages [Experimental]"), customSkin.FindStyle("Toggle"));
            useDynamicMessages.boolValue = GUILayout.Toggle(useDynamicMessages.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            useStoryTeller.boolValue = GUILayout.Toggle(useStoryTeller.boolValue, new GUIContent("Use Story Teller [Beta]"), customSkin.FindStyle("Toggle"));
            useStoryTeller.boolValue = GUILayout.Toggle(useStoryTeller.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.Space(18);

            var messageList = serializedObject.FindProperty("messageList");
            var dynamicMessages = serializedObject.FindProperty("dynamicMessages");
            var storyTeller = serializedObject.FindProperty("storyTeller");

            GUILayout.Label("MESSAGES", customSkin.FindStyle("Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PropertyField(messageList, new GUIContent("Message List"), true);
            messageList.isExpanded = true;

            EditorGUI.indentLevel = 0;

            if (GUILayout.Button("+  Add a new message", customSkin.button))
                messageList.arraySize += 1;

            GUILayout.EndVertical();

            if (useDynamicMessages.boolValue == true)
            {
                GUILayout.Space(18);
                GUILayout.Label("DYNAMIC MESSAGES", customSkin.FindStyle("Header"));
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel = 1;

                EditorGUILayout.PropertyField(dynamicMessages, new GUIContent("Dynamic Messages"), true);
                dynamicMessages.isExpanded = true;

                EditorGUI.indentLevel = 0;

                if (GUILayout.Button("+  Add a new message", customSkin.button))
                    dynamicMessages.arraySize += 1;

                GUILayout.EndVertical();
            }

            if (useStoryTeller.boolValue == true)
            {
                GUILayout.Space(18);
                GUILayout.Label("STORY TELLER", customSkin.FindStyle("Header"));
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel = 1;

                EditorGUILayout.PropertyField(storyTeller, new GUIContent("StoryTeller"), true);
                storyTeller.isExpanded = true;

                EditorGUI.indentLevel = 0;

                if (GUILayout.Button("+  Add a new item", customSkin.button))
                    storyTeller.arraySize += 1;

                GUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif