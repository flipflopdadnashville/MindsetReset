#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(DoubleClickEvent))]
    public class DoubleClickEventEditor : Editor
    {
        private DoubleClickEvent dkeTarget;
        private int currentTab;

        private void OnEnable()
        {
            dkeTarget = (DoubleClickEvent)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("DCE Top Header"));

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

            var enableSingleClick = serializedObject.FindProperty("enableSingleClick");
            var timeFactor = serializedObject.FindProperty("timeFactor");
            var doubleClickEvents = serializedObject.FindProperty("doubleClickEvents");
            var singleClickEvents = serializedObject.FindProperty("singleClickEvents");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Time Factor"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                    EditorGUILayout.PropertyField(timeFactor, new GUIContent(""), true);

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableSingleClick.boolValue = GUILayout.Toggle(enableSingleClick.boolValue, new GUIContent("Enable Single Click"), customSkin.FindStyle("Toggle"));
                    enableSingleClick.boolValue = GUILayout.Toggle(enableSingleClick.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);

                    EditorGUILayout.PropertyField(doubleClickEvents, new GUIContent("Double Click Events"), true);

                    if(enableSingleClick.boolValue == true)
                        EditorGUILayout.PropertyField(singleClickEvents, new GUIContent("Single Click Events"), true);

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif