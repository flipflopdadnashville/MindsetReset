#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(MessagingManager))]
    public class MessagingManagerEditor : Editor
    {
        private MessagingManager mesTarget;
        private int currentTab;

        private void OnEnable()
        {
            mesTarget = (MessagingManager)target;
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Messaging Top Header"));

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

            var chatList = serializedObject.FindProperty("chatList");
            var chatsParent = serializedObject.FindProperty("chatsParent");
            var chatViewer = serializedObject.FindProperty("chatViewer");
            var chatMessageButton = serializedObject.FindProperty("chatMessageButton");
            var chatMessageSent = serializedObject.FindProperty("chatMessageSent");
            var chatMessageRecieved = serializedObject.FindProperty("chatMessageRecieved");
            var audioMessageSent = serializedObject.FindProperty("audioMessageSent");
            var audioMessageRecieved = serializedObject.FindProperty("audioMessageRecieved");
            var imageMessageSent = serializedObject.FindProperty("imageMessageSent");
            var imageMessageRecieved = serializedObject.FindProperty("imageMessageRecieved");
            var chatMessageTimer = serializedObject.FindProperty("chatMessageTimer");
            var chatLayout = serializedObject.FindProperty("chatLayout");
            var messageDate = serializedObject.FindProperty("messageDate");
            var messageInput = serializedObject.FindProperty("messageInput");
            var timeManager = serializedObject.FindProperty("timeManager");
            var audioPlayer = serializedObject.FindProperty("audioPlayer");
            var storyTellerAnimator = serializedObject.FindProperty("storyTellerAnimator");
            var storyTellerList = serializedObject.FindProperty("storyTellerList");
            var storyTellerObject = serializedObject.FindProperty("storyTellerObject");
            var debugStoryTeller = serializedObject.FindProperty("debugStoryTeller");
            var notificationCreator = serializedObject.FindProperty("notificationCreator");
            var useNotifications = serializedObject.FindProperty("useNotifications");
            var receivedMessageSound = serializedObject.FindProperty("receivedMessageSound");
            var sentMessageSound = serializedObject.FindProperty("sentMessageSound");
            var messageStoring = serializedObject.FindProperty("messageStoring");
            var saveSentMessages = serializedObject.FindProperty("saveSentMessages");

            switch (currentTab)
            {
                case 0:
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(chatList, new GUIContent("List"), true);
                    chatList.isExpanded = true;

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("+  Add a new chat", customSkin.button))
                        mesTarget.AddChat();
                   
                    GUILayout.EndVertical();
                    break;

                case 1:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chats Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatsParent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Viewer"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatViewer, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Layout"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatLayout, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Message Button"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatMessageButton, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Message Sent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatMessageSent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Message Recieved"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatMessageRecieved, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Audio Message Sent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(audioMessageSent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Audio Message Recieved"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(audioMessageRecieved, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Image Message Sent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(imageMessageSent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Image Message Recieved"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(imageMessageRecieved, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Chat Message Timer"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(chatMessageTimer, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Message Date"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(messageDate, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Message Input"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(messageInput, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Time Manager"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(timeManager, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Audio Player"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(audioPlayer, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Story Teller Animator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(storyTellerAnimator, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Story Teller List"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(storyTellerList, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Story Teller Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(storyTellerObject, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Message Storing"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(messageStoring, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (useNotifications.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Notification Creator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(notificationCreator, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    break;
                case 2:
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Sent Message Sound"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(sentMessageSound, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Received Message Sound"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(receivedMessageSound, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    debugStoryTeller.boolValue = GUILayout.Toggle(debugStoryTeller.boolValue, new GUIContent("Debug StoryTeller"), customSkin.FindStyle("Toggle"));
                    debugStoryTeller.boolValue = GUILayout.Toggle(debugStoryTeller.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    useNotifications.boolValue = GUILayout.Toggle(useNotifications.boolValue, new GUIContent("Use Notifications"), customSkin.FindStyle("Toggle"));
                    useNotifications.boolValue = GUILayout.Toggle(useNotifications.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    saveSentMessages.boolValue = GUILayout.Toggle(saveSentMessages.boolValue, new GUIContent("Save Sent Messages"), customSkin.FindStyle("Toggle"));
                    saveSentMessages.boolValue = GUILayout.Toggle(saveSentMessages.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    break;
            }

            if (saveSentMessages.boolValue == true && mesTarget.messageStoring == null)
            {
                EditorGUILayout.HelpBox("'Save Sent Messages' is enabled but 'Message Storing' is not assigned. " +
                    "Please add and/or assign 'Message Storing' component via Resources tab.", MessageType.Error);

                if (GUILayout.Button("+  Create Message Storing", customSkin.button))
                {
                    MessageStoring tempMS = mesTarget.gameObject.AddComponent<MessageStoring>();
                    mesTarget.messageStoring = tempMS;
                    tempMS.messagingManager = mesTarget;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif