#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(NotificationCreator))]
    public class NotificationCreatorEditor : Editor
    {
        private NotificationCreator ntfmcTarget;
        private int currentTab;

        private void OnEnable()
        {
            ntfmcTarget = (NotificationCreator)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("NC Top Header"));
         
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

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var notificationIcon = serializedObject.FindProperty("notificationIcon");
            var notificationTitle = serializedObject.FindProperty("notificationTitle");
            var notificationDescription = serializedObject.FindProperty("notificationDescription");
            var popupDescription = serializedObject.FindProperty("popupDescription");
            var notificationButtons = serializedObject.FindProperty("notificationButtons");
            var managerScript = serializedObject.FindProperty("managerScript");
            var enableButtonIcon = serializedObject.FindProperty("enableButtonIcon");
            var showOnlyOnce = serializedObject.FindProperty("showOnlyOnce");
            var enablePopupNotification = serializedObject.FindProperty("enablePopupNotification");
            var enableNotificationSound = serializedObject.FindProperty("enableNotificationSound");
            var notificationSound = serializedObject.FindProperty("notificationSound");
            var notificationColor = serializedObject.FindProperty("notificationColor");
            var notificationType = serializedObject.FindProperty("notificationType");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Icon"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(notificationIcon, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Main Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(notificationColor, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Title"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(notificationTitle, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Description"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
                    EditorGUILayout.PropertyField(notificationDescription, new GUIContent(""), GUILayout.Height(74));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Popup Description"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
                    EditorGUILayout.PropertyField(popupDescription, new GUIContent(""), GUILayout.Height(50));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(notificationButtons, new GUIContent("Notification Buttons"), true);

                    EditorGUI.indentLevel = 0;

                    if (GUILayout.Button("+ Create a new button", customSkin.button))
                        ntfmcTarget.CreateNewButton();

                    GUILayout.EndVertical();
                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Notification Manager"), customSkin.FindStyle("Text"), GUILayout.Width(140));
                    EditorGUILayout.PropertyField(managerScript, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enablePopupNotification.boolValue = GUILayout.Toggle(enablePopupNotification.boolValue, new GUIContent("Enable Popup"), customSkin.FindStyle("Toggle"));
                    enablePopupNotification.boolValue = GUILayout.Toggle(enablePopupNotification.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableButtonIcon.boolValue = GUILayout.Toggle(enableButtonIcon.boolValue, new GUIContent("Enable Button Icon"), customSkin.FindStyle("Toggle"));
                    enableButtonIcon.boolValue = GUILayout.Toggle(enableButtonIcon.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    showOnlyOnce.boolValue = GUILayout.Toggle(showOnlyOnce.boolValue, new GUIContent("Show Only Once"), customSkin.FindStyle("Toggle"));
                    showOnlyOnce.boolValue = GUILayout.Toggle(showOnlyOnce.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableNotificationSound.boolValue = GUILayout.Toggle(enableNotificationSound.boolValue, new GUIContent("Enable Sound"), customSkin.FindStyle("Toggle"));
                    enableNotificationSound.boolValue = GUILayout.Toggle(enableNotificationSound.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableNotificationSound.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Alert Sound"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(notificationSound, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Notification Style"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(notificationType, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;
            }

            if (ntfmcTarget.managerScript == null)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("'Notification Manager' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                GUILayout.EndHorizontal();
                GUILayout.Space(4);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif