#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(ThemeManager))]
    [System.Serializable]
    public class ThemeManagerEditor : Editor
    {
        Texture2D tmeLogo;
        protected static bool showGeneralSettings = false;
        protected static bool showColors = false;
        protected static bool showFonts = false;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                tmeLogo = Resources.Load<Texture2D>("Editor\\Theme Manager Dark");
            else
                tmeLogo = Resources.Load<Texture2D>("Editor\\Theme Manager Light");
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light");

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.font = customSkin.font;
            foldoutStyle.fontStyle = FontStyle.Normal;
            foldoutStyle.fontSize = 15;
            foldoutStyle.margin = new RectOffset(11, 55, 6, 6);
            Vector2 contentOffset = foldoutStyle.contentOffset;
            contentOffset.x = 5;
            contentOffset.y = -1;
            foldoutStyle.contentOffset = contentOffset;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(tmeLogo, GUILayout.Width(250), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Colors
            var windowBGColorDark = serializedObject.FindProperty("windowBGColorDark");
            var backgroundColorDark = serializedObject.FindProperty("backgroundColorDark");
            var primaryColorDark = serializedObject.FindProperty("primaryColorDark");
            var secondaryColorDark = serializedObject.FindProperty("secondaryColorDark");
            var highlightedColorDark = serializedObject.FindProperty("highlightedColorDark");
            var highlightedColorSecondaryDark = serializedObject.FindProperty("highlightedColorSecondaryDark");
            var taskBarColorDark = serializedObject.FindProperty("taskBarColorDark");
            var highlightedColorCustom = serializedObject.FindProperty("highlightedColorCustom");
            var highlightedColorSecondaryCustom = serializedObject.FindProperty("highlightedColorSecondaryCustom");
            showColors = EditorGUILayout.Foldout(showColors, "Colors", true, foldoutStyle);

            if (showColors)
            {
                GUILayout.Label("SYSTEM THEME", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Reversed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorSecondaryDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(primaryColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(secondaryColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Window BG Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(windowBGColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(backgroundColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Task Bar Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(taskBarColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.Space(12);
                GUILayout.Label("CUSTOM THEME", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorCustom, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Reversed Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorSecondaryCustom, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Fonts
            var systemFontThin = serializedObject.FindProperty("systemFontThin");
            var systemFontLight = serializedObject.FindProperty("systemFontLight");
            var systemFontRegular = serializedObject.FindProperty("systemFontRegular");
            var systemFontSemiBold = serializedObject.FindProperty("systemFontSemiBold");
            var systemFontBold = serializedObject.FindProperty("systemFontBold");
            showFonts = EditorGUILayout.Foldout(showFonts, "Fonts", true, foldoutStyle);

            if (showFonts)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Thin"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontThin, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Light"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontLight, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Regular"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontRegular, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Semibold"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontSemiBold, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Bold"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontBold, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(7);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(6);

            var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent("Update Values"), customSkin.FindStyle("Toggle"));
            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            var enableExtendedColorPicker = serializedObject.FindProperty("enableExtendedColorPicker");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent("Extended Color Picker"), customSkin.FindStyle("Toggle"));
            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            if (enableExtendedColorPicker.boolValue == true)
                EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 1);
            else
                EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 0);

            var editorHints = serializedObject.FindProperty("editorHints");

            GUILayout.BeginVertical(EditorStyles.helpBox);

            editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent("Theme Manager Hints"), customSkin.FindStyle("Toggle"));

            if (editorHints.boolValue == true)
            {
                EditorGUILayout.HelpBox("These values are universal and will affect any object that contains 'Theme Manager' component.", MessageType.Info);
                EditorGUILayout.HelpBox("Remove 'Theme Manager' component from the object in order to get unique values.", MessageType.Info);
            }

            GUILayout.EndVertical();

            var rootFolder = serializedObject.FindProperty("rootFolder");
            var changeRootFolder = serializedObject.FindProperty("changeRootFolder");

            GUILayout.BeginVertical(EditorStyles.helpBox);

            changeRootFolder.boolValue = GUILayout.Toggle(changeRootFolder.boolValue, new GUIContent("Change Root Folder"), customSkin.FindStyle("Toggle"), GUILayout.Width(500));

            if (changeRootFolder.boolValue == true)
            {
                EditorGUI.indentLevel = 2;
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(new GUIContent("Root Folder:"), customSkin.FindStyle("Text"), GUILayout.Width(76));
                EditorGUILayout.PropertyField(rootFolder, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.Space(2);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Apply", customSkin.button, GUILayout.Width(120)))
                    EditorPrefs.SetString("DreamOS.ObjectCreator.RootFolder", rootFolder.stringValue);

                GUILayout.EndHorizontal();
                GUILayout.Space(2);
                EditorGUI.indentLevel = 0;
                EditorGUILayout.HelpBox("Only use this option if you're moving DreamOS folder. " +
                    "Make sure to hit apply after changing the root.", MessageType.Warning);
            }

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset to defaults", customSkin.button))
                ResetToDefaults();

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Apply & Update button
            GUILayout.Space(4);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            GUILayout.Label("Need help? Contact me via:", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();

            if (GUILayout.Button("Discord", customSkin.button))
                Discord();

            if (GUILayout.Button("E-mail", customSkin.button))
                Email();

            if (GUILayout.Button("Twitter", customSkin.button))
                Twitter();

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Website", customSkin.button))
                Website();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(12);
        }

        void Discord()
        {
            Application.OpenURL("https://discord.gg/VXpHyUt");
        }

        void Email()
        {
            Application.OpenURL("mailto:isa.steam@outlook.com?subject=Contact");
        }

        void Twitter()
        {
            Application.OpenURL("https://twitter.com/michskyHQ");
        }

        void Website()
        {
            Application.OpenURL("https://www.michsky.com/");
        }

        void ResetToDefaults()
        {
            if (EditorUtility.DisplayDialog("Reset to defaults", "Are you sure you want to reset Theme Manager values to default?", "Yes", "Cancel"))
            {
                try
                {
                    if (EditorPrefs.HasKey("DreamOS.PipelineUpgrader"))
                    {
                        Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/SRP Default");
                        defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
                    }

                    else
                    {
                        Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/Default");
                        defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
                    }

                    Selection.activeObject = null;
                    Debug.Log("<b>[Theme Manager]</b> Resetting successful.");
                }

                catch
                {
                    Debug.LogWarning("<b>[Theme Manager]</b> Resetting failed.");
                }
            }
        }
    }
}
#endif