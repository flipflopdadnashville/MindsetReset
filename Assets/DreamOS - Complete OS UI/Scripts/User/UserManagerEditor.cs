#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(UserManager))]
    public class UserManagerEditor : Editor
    {
        private UserManager userTarget;
        private int currentTab;
        bool disableGroup = false;

        private void OnEnable()
        {
            userTarget = (UserManager)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("UM Top Header"));

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

            var errorMessage = serializedObject.FindProperty("errorMessage");
            var minNameCharacter = serializedObject.FindProperty("minNameCharacter");
            var maxNameCharacter = serializedObject.FindProperty("maxNameCharacter");
            var minPasswordCharacter = serializedObject.FindProperty("minPasswordCharacter");
            var maxPasswordCharacter = serializedObject.FindProperty("maxPasswordCharacter");
            var minSecurityCharacter = serializedObject.FindProperty("minSecurityCharacter");
            var maxSecurityCharacter = serializedObject.FindProperty("maxSecurityCharacter");
            var systemUsername = serializedObject.FindProperty("systemUsername");
            var systemLastname = serializedObject.FindProperty("systemLastname");
            var systemPassword = serializedObject.FindProperty("systemPassword");
            var systemUserPicture = serializedObject.FindProperty("systemUserPicture");
            var onLogin = serializedObject.FindProperty("onLogin");
            var onLock = serializedObject.FindProperty("onLock");
            var bootScreen = serializedObject.FindProperty("bootScreen");
            var setupScreen = serializedObject.FindProperty("setupScreen");
            var lockScreen = serializedObject.FindProperty("lockScreen");
            var desktopScreen = serializedObject.FindProperty("desktopScreen");
            var lockScreenPassword = serializedObject.FindProperty("lockScreenPassword");
            var lockScreenErrorPopup = serializedObject.FindProperty("lockScreenErrorPopup");
            var lockScreenBlur = serializedObject.FindProperty("lockScreenBlur");
            var errorTextObject = serializedObject.FindProperty("errorTextObject");
            var disableUserCreating = serializedObject.FindProperty("disableUserCreating");
            var deletePrefsAtStart = serializedObject.FindProperty("deletePrefsAtStart");
            var showUserData = serializedObject.FindProperty("showUserData");

            switch (currentTab)
            {
                case 0:
                    if (disableUserCreating.boolValue == false)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Min / Max Name Char"), customSkin.FindStyle("Text"), GUILayout.Width(150));

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(minNameCharacter, new GUIContent(""));
                        EditorGUILayout.PropertyField(maxNameCharacter, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Min / Max Password Char"), customSkin.FindStyle("Text"), GUILayout.Width(150));

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(minPasswordCharacter, new GUIContent(""));
                        EditorGUILayout.PropertyField(maxPasswordCharacter, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Min / Max Security Char"), customSkin.FindStyle("Text"), GUILayout.Width(150));

                        GUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(minSecurityCharacter, new GUIContent(""));
                        EditorGUILayout.PropertyField(maxSecurityCharacter, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }

                    else
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Username"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(systemUsername, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Lastname"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(systemLastname, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Password"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(systemPassword, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("User Picture"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(systemUserPicture, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Wrong Password Message"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
                    EditorGUILayout.PropertyField(errorMessage, new GUIContent(""), GUILayout.Height(50));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(20);
                    EditorGUILayout.PropertyField(onLogin, new GUIContent("On Login Events"));
                    EditorGUILayout.PropertyField(onLock, new GUIContent("On Lock Events"));
                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Boot Screen"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(bootScreen, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Setup Screen"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(setupScreen, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Lock Screen"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(lockScreen, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Desktop Screen"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(desktopScreen, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Lock Screen Password"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(lockScreenPassword, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Lock Screen Error"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(lockScreenErrorPopup, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Lock Screen Blur"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(lockScreenBlur, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Wrong Password Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(errorTextObject, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    disableUserCreating.boolValue = GUILayout.Toggle(disableUserCreating.boolValue, new GUIContent("Disable User Creating"), customSkin.FindStyle("Toggle"));
                    disableUserCreating.boolValue = GUILayout.Toggle(disableUserCreating.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (disableUserCreating.boolValue == false)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        deletePrefsAtStart.boolValue = GUILayout.Toggle(deletePrefsAtStart.boolValue, new GUIContent("Delete PlayerPrefs At Start"), customSkin.FindStyle("Toggle"));
                        deletePrefsAtStart.boolValue = GUILayout.Toggle(deletePrefsAtStart.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();

                        if (deletePrefsAtStart.boolValue == true)
                            EditorGUILayout.HelpBox("While this option is enabled, all PlayerPrefs data will be wiped at start. Use with caution.", MessageType.Warning);

                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        showUserData.boolValue = GUILayout.Toggle(showUserData.boolValue, new GUIContent("Show User Data [DEBUG]"), customSkin.FindStyle("Toggle"));
                        showUserData.boolValue = GUILayout.Toggle(showUserData.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                    }

                    else
                    {
                        EditorGUILayout.HelpBox("Disable User Creating is enabled. You can change the default user settings by switching to the first tab.", MessageType.Info);
                        showUserData.boolValue = false;
                    }

                    GUILayout.Space(4);

                    if (showUserData.boolValue == true)
                    {
                        EditorGUI.BeginDisabledGroup(disableGroup == false);
                        EditorGUILayout.LabelField("First name:");
                        EditorGUILayout.TextField(PlayerPrefs.GetString("FirstName"));
                        EditorGUILayout.LabelField("Last name:");
                        EditorGUILayout.TextField(PlayerPrefs.GetString("LastName"));
                        GUILayout.Space(10f);

                        userTarget.hasPassword = EditorGUILayout.Toggle("Has password: ", userTarget.hasPassword);
                        EditorGUILayout.LabelField("Password:");
                        EditorGUILayout.TextField(PlayerPrefs.GetString("Password"));
                        GUILayout.Space(10f);

                        userTarget.hasSecurityAnswer = EditorGUILayout.Toggle("Has security answer: ", userTarget.hasSecurityAnswer);
                        EditorGUILayout.LabelField("Security answer:");
                        EditorGUILayout.TextField(PlayerPrefs.GetString("SecurityQuestion"));
                        EditorGUILayout.LabelField("Security answer hint:");
                        EditorGUILayout.TextField(PlayerPrefs.GetString("SecurityQuestionHint"));
                        GUILayout.Space(10f);

                        userTarget.profilePicture = EditorGUILayout.ObjectField("Profile picture: ", Resources.Load<Sprite>(PlayerPrefs.GetString("Profile Picture")), typeof(Sprite), true) as Sprite;
                        GUILayout.Space(10f);

                        EditorGUILayout.PrefixLabel("Updating User");
                        userTarget.nameOK = EditorGUILayout.Toggle("First name status: ", userTarget.nameOK);
                        userTarget.lastNameOK = EditorGUILayout.Toggle("Last name status: ", userTarget.lastNameOK);
                        userTarget.passwordOK = EditorGUILayout.Toggle("Password status: ", userTarget.passwordOK);
                        userTarget.passwordRetypeOK = EditorGUILayout.Toggle("Password Re-type status: ", userTarget.passwordRetypeOK);
                        EditorGUI.indentLevel--;
                    }
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif