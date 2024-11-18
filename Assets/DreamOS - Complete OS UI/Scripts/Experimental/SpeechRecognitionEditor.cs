#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(SpeechRecognition))]
    public class SpeechRecognitionEditor : Editor
    {
        private SpeechRecognition srTarget;
        private int currentTab;
        bool disableGroup = false;

        private void OnEnable()
        {
            srTarget = (SpeechRecognition)target;
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
            GUI.backgroundColor = defaultColor;

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("SR Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Chat List", "Chat List"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

#if UNITY_STANDALONE_WIN || UNITY_WSA
            var keywords = serializedObject.FindProperty("keywords");
            var commands = serializedObject.FindProperty("commands");
            var listeningMessages = serializedObject.FindProperty("listeningMessages");
            var onKeywordCall = serializedObject.FindProperty("onKeywordCall");
            var onDismiss = serializedObject.FindProperty("onDismiss");
            var coriPopup = serializedObject.FindProperty("coriPopup");
            var feedbackSource = serializedObject.FindProperty("feedbackSource");
            var listeningText = serializedObject.FindProperty("listeningText");
            var listeningEffect = serializedObject.FindProperty("listeningEffect");
            var dismissEffect = serializedObject.FindProperty("dismissEffect");
            var enableKeywords = serializedObject.FindProperty("enableKeywords");
            var enableLogs = serializedObject.FindProperty("enableLogs");
            var debugMode = serializedObject.FindProperty("debugMode");
            var dismissAfter = serializedObject.FindProperty("dismissAfter");
            var hypotheses = serializedObject.FindProperty("hypotheses");
            var recognitions = serializedObject.FindProperty("recognitions");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(keywords, new GUIContent("Keywords"), true);
                    keywords.isExpanded = true;

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(commands, new GUIContent("Commands"), true);

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(listeningMessages, new GUIContent("Listening Messages"), true);

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(18);

                    EditorGUILayout.PropertyField(onKeywordCall, new GUIContent("On Keyword Call"));
                    EditorGUILayout.PropertyField(onDismiss, new GUIContent("On Dismiss"));
                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Cori Popup"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(coriPopup, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Feedback Source"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(feedbackSource, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Listening Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(listeningText, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;
                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Dismiss After"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(dismissAfter, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Listening Effect"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(listeningEffect, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Dismiss Effect"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(dismissEffect, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableKeywords.boolValue = GUILayout.Toggle(enableKeywords.boolValue, new GUIContent("Enable Keywords"), customSkin.FindStyle("Toggle"));
                    enableKeywords.boolValue = GUILayout.Toggle(enableKeywords.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableLogs.boolValue = GUILayout.Toggle(enableLogs.boolValue, new GUIContent("Enable Logs"), customSkin.FindStyle("Toggle"));
                    enableLogs.boolValue = GUILayout.Toggle(enableLogs.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    debugMode.boolValue = GUILayout.Toggle(debugMode.boolValue, new GUIContent("Debug Mode"), customSkin.FindStyle("Toggle"));
                    debugMode.boolValue = GUILayout.Toggle(debugMode.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (debugMode.boolValue == true)
                    {
                        GUILayout.Space(8);
                        EditorGUI.BeginDisabledGroup(disableGroup == false);
                        EditorGUILayout.PropertyField(hypotheses, new GUIContent("Hypotheses"));
                        EditorGUILayout.PropertyField(recognitions, new GUIContent("Recognitions"));
                        GUILayout.Space(10f);
                        EditorGUI.indentLevel--;
                    }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
#else
            EditorGUILayout.HelpBox("Speech Recognition is only available for Windows 10 and UWP.", MessageType.Info);
#endif
        }
    }
}
#endif