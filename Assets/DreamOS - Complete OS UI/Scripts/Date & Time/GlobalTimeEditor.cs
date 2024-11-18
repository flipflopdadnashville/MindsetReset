#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(GlobalTime))]
    public class GlobalTimeEditor : Editor
    {
        private GlobalTime timeTarget;
        private int currentTab;

        private void OnEnable()
        {
            timeTarget = (GlobalTime)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Time Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Timed Events");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Time", "Time"), customSkin.FindStyle("Tab Resources")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Timed Events", "Timed Events"), customSkin.FindStyle("Tab Content")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var timedEvents = serializedObject.FindProperty("timedEvents");
            var timeMultiplier = serializedObject.FindProperty("timeMultiplier");
            var currentDay = serializedObject.FindProperty("currentDay");
            var currentMonth = serializedObject.FindProperty("currentMonth");
            var currentYear = serializedObject.FindProperty("currentYear");
            var currentHour = serializedObject.FindProperty("currentHour");
            var currentMinute = serializedObject.FindProperty("currentMinute");
            var currentSecond = serializedObject.FindProperty("currentSecond");
            var saveAndGetValues = serializedObject.FindProperty("saveAndGetValues");
            var enableAmPm = serializedObject.FindProperty("enableAmPm");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Time Multiplier"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(timeMultiplier, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Year"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentYear, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Month"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentMonth, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Day"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentDay, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Hour"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentHour, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Minute"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentMinute, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Second"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(currentSecond, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if(saveAndGetValues.boolValue == true)
                        EditorGUILayout.HelpBox("Save Data is enabled. Some of these variables won't be used if there's a stored data.", MessageType.Info);

                    break;

                case 1:
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(timedEvents, new GUIContent("Event List"), true);
                    timedEvents.isExpanded = true;

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("+  Add a new event", customSkin.button))
                        timeTarget.AddTimedEvent();

                    GUILayout.EndVertical();
                    break;

                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableAmPm.boolValue = GUILayout.Toggle(enableAmPm.boolValue, new GUIContent("Enable AM / PM"), customSkin.FindStyle("Toggle"));
                    enableAmPm.boolValue = GUILayout.Toggle(enableAmPm.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    saveAndGetValues.boolValue = GUILayout.Toggle(saveAndGetValues.boolValue, new GUIContent("Save Values"), customSkin.FindStyle("Toggle"));
                    saveAndGetValues.boolValue = GUILayout.Toggle(saveAndGetValues.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (saveAndGetValues.boolValue == true)
                    {
                        if (GUILayout.Button("Clear Stored Data", customSkin.button))
                        {
                            PlayerPrefs.DeleteKey("CurrentSecond");
                            PlayerPrefs.DeleteKey("CurrentMinute");
                            PlayerPrefs.DeleteKey("CurrentHour");
                            PlayerPrefs.DeleteKey("CurrentMonth");
                            PlayerPrefs.DeleteKey("CurrentYear");
                            PlayerPrefs.DeleteKey("TimeInitalized");
                        }
                    }

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif