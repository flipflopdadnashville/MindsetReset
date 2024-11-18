#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(WindowManager))]
    public class WindowManagerEditor : Editor
    {
        private WindowManager wmTarget;
        private int currentTab;

        private void OnEnable()
        {
            wmTarget = (WindowManager)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("WM Top Header"));

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

            var fullscreenImage = serializedObject.FindProperty("fullscreenImage");
            var minimizeImage = serializedObject.FindProperty("minimizeImage");
            var imageObject = serializedObject.FindProperty("imageObject");
            var taskBarButton = serializedObject.FindProperty("taskBarButton");
            var windowDragger = serializedObject.FindProperty("windowDragger");
            var windowResize = serializedObject.FindProperty("windowResize");
            var onEnableEvents = serializedObject.FindProperty("onEnableEvents");
            var onLaunchEvents = serializedObject.FindProperty("onLaunchEvents");
            var onQuitEvents = serializedObject.FindProperty("onQuitEvents");
            var enableBackgroundBlur = serializedObject.FindProperty("enableBackgroundBlur");
            var hasNavDrawer = serializedObject.FindProperty("hasNavDrawer");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Fullscreen Image"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(fullscreenImage, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Minimize Image"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(minimizeImage, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(18);

                    EditorGUILayout.PropertyField(onEnableEvents, new GUIContent("On Enable"), true);
                    EditorGUILayout.PropertyField(onLaunchEvents, new GUIContent("On Launch"), true);
                    EditorGUILayout.PropertyField(onQuitEvents, new GUIContent("On Quit"), true);

                    if (wmTarget.GetComponent<CanvasGroup>().alpha == 0)
                    {
                        if (GUILayout.Button("Make It Visible", customSkin.button))
                            wmTarget.GetComponent<CanvasGroup>().alpha = 1;
                    }

                    else
                    {
                        if (GUILayout.Button("Make It Invisible", customSkin.button))
                            wmTarget.GetComponent<CanvasGroup>().alpha = 0;
                    }

                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Nav Image Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(imageObject, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Taskbar Button"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(taskBarButton, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("window Dragger"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(windowDragger, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Window Resize"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(windowResize, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;
                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableBackgroundBlur.boolValue = GUILayout.Toggle(enableBackgroundBlur.boolValue, new GUIContent("Enable Background Blur"), customSkin.FindStyle("Toggle"));
                    enableBackgroundBlur.boolValue = GUILayout.Toggle(enableBackgroundBlur.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    hasNavDrawer.boolValue = GUILayout.Toggle(hasNavDrawer.boolValue, new GUIContent("Has Nav Drawer"), customSkin.FindStyle("Toggle"));
                    hasNavDrawer.boolValue = GUILayout.Toggle(hasNavDrawer.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif