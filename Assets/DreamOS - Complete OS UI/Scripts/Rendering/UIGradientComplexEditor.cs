#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UnityEngine.UI
{
    namespace Michsky.DreamOS
    {
        [CustomEditor(typeof(UIGradientComplex))]
        public class UIGradientComplexEditor : Editor
        {
            private int currentTab;

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

                GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Gradient Top Header"));

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

                var _effectGradient = serializedObject.FindProperty("_effectGradient");
                var gradientType = serializedObject.FindProperty("gradientType");
                var offset = serializedObject.FindProperty("offset");
                var zoom = serializedObject.FindProperty("zoom");
                var complexGradient = serializedObject.FindProperty("complexGradient");

                switch (currentTab)
                {
                    case 0:
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Gradient"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(_effectGradient, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(gradientType, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Offset"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(offset, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Zoom"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(zoom, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        complexGradient.boolValue = GUILayout.Toggle(complexGradient.boolValue, new GUIContent("Complex Gradient"), customSkin.FindStyle("Toggle"));
                        complexGradient.boolValue = GUILayout.Toggle(complexGradient.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                        break;              
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif