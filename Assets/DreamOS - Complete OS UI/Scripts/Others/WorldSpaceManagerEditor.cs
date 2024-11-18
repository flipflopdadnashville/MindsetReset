#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(WorldSpaceManager))]
    public class WorldSpaceManagerEditor : Editor
    {
        private WorldSpaceManager wsTarget;
        private int currentTab;

        private void OnEnable()
        {
            wsTarget = (WorldSpaceManager)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("WS Top Header"));

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

            var onEnter = serializedObject.FindProperty("onEnter");
            var onEnterEnd = serializedObject.FindProperty("onEnterEnd");
            var onExit = serializedObject.FindProperty("onExit");
            var onExitEnd = serializedObject.FindProperty("onExitEnd");     
            var playerTag = serializedObject.FindProperty("playerTag");
            var getInKey = serializedObject.FindProperty("getInKey");
            var getOutKey = serializedObject.FindProperty("getOutKey");
            var transitionSpeed = serializedObject.FindProperty("transitionSpeed");
            var requiresOpening = serializedObject.FindProperty("requiresOpening");
            var autoGetIn = serializedObject.FindProperty("autoGetIn");
            var lockCursorWhenOut = serializedObject.FindProperty("lockCursorWhenOut");
            var mainCamera = serializedObject.FindProperty("mainCamera");
            var enterMount = serializedObject.FindProperty("enterMount");
            var projectorCam = serializedObject.FindProperty("projectorCam");
            var OSCam = serializedObject.FindProperty("OSCam");
            var osCanvas = serializedObject.FindProperty("osCanvas");
            var useFloatingIcon = serializedObject.FindProperty("useFloatingIcon");

            switch (currentTab)
            {
                case 0:
                    EditorGUILayout.PropertyField(onEnter, new GUIContent("On Enter Events"), true);
                    EditorGUILayout.PropertyField(onEnterEnd, new GUIContent("On Enter End Events"), true);
                    EditorGUILayout.PropertyField(onExit, new GUIContent("On Exit Events"), true);
                    EditorGUILayout.PropertyField(onExitEnd, new GUIContent("On Exit End Events"), true);
                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Main Camera"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(mainCamera, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Enter Mount"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(enterMount, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Projector Cam"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(projectorCam, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("OS Camera"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(OSCam, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("OS Canvas"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(osCanvas, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Floating Icon"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(useFloatingIcon, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Player Tag"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(playerTag, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Get In Key"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(getInKey, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Get Out Key"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(getOutKey, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Transition Speed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(transitionSpeed, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    requiresOpening.boolValue = GUILayout.Toggle(requiresOpening.boolValue, new GUIContent("Requires Opening At First"), customSkin.FindStyle("Toggle"));
                    requiresOpening.boolValue = GUILayout.Toggle(requiresOpening.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    autoGetIn.boolValue = GUILayout.Toggle(autoGetIn.boolValue, new GUIContent("Auto Get In On Trigger"), customSkin.FindStyle("Toggle"));
                    autoGetIn.boolValue = GUILayout.Toggle(autoGetIn.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    lockCursorWhenOut.boolValue = GUILayout.Toggle(lockCursorWhenOut.boolValue, new GUIContent("Lock Cursor On Out"), customSkin.FindStyle("Toggle"));
                    lockCursorWhenOut.boolValue = GUILayout.Toggle(lockCursorWhenOut.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif