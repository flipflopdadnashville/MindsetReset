#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(HoldKeyEvent))]
    public class HoldKeyEventEditor : Editor
    {
        private HoldKeyEvent hkeTarget;
        private int currentTab;

        private void OnEnable()
        {
            hkeTarget = (HoldKeyEvent)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("HKE Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[1];
            toolbarTabs[0] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 0;

            GUILayout.EndHorizontal();

            var hotkey = serializedObject.FindProperty("hotkey");
            var holdAction = serializedObject.FindProperty("holdAction");
            var releaseAction = serializedObject.FindProperty("releaseAction");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Hotkey"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                    EditorGUILayout.PropertyField(hotkey, new GUIContent(""), true);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);

                    EditorGUILayout.PropertyField(holdAction, new GUIContent("Hold Events"), true);
                    EditorGUILayout.PropertyField(releaseAction, new GUIContent("Release Events"), true);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif