using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class MessagingManager : MonoBehaviour
    {
        // Resources
        public Transform chatsParent;
        public Transform chatViewer;
        public GameObject chatLayout;
        public GameObject chatMessageButton;
        public GameObject chatMessageSent;
        public GameObject chatMessageRecieved;
        public GameObject audioMessageSent;
        public GameObject audioMessageRecieved;
        public GameObject imageMessageSent;
        public GameObject imageMessageRecieved;
        public GameObject chatMessageTimer;
        public GameObject messageDate;
        public TMP_InputField messageInput;
        public GlobalTime timeManager;
        public AudioSource audioPlayer;
        public Animator storyTellerAnimator;
        public Transform storyTellerList;
        public GameObject storyTellerObject;
        public NotificationCreator notificationCreator;
        public MessageStoring messageStoring;
        [HideInInspector] public GameObject selectedLayout;

        // List
        public List<ChatItem> chatList = new List<ChatItem>();
        List<GameObject> createdLayouts = new List<GameObject>();

        // Settings
        public AudioClip sentMessageSound;
        public AudioClip receivedMessageSound;
        public bool debugStoryTeller = true;
        public bool useNotifications = true;
        public bool saveSentMessages = false;
        // public bool saveStoryTellerMessages = false;
        // public bool saveDynamicMessages = false;
        public string audioMessageNotification = "Sent an audio message: ";
        public string imageMessageNotification = "Sent an image: ";

        // Helper variables
        public int currentLayout;
        [HideInInspector] public int storyTellerIndex = 0;
        [HideInInspector] public int stItemIndex = 0;
        [HideInInspector] public int stIndexHelper = 0;
        [HideInInspector] public bool getTimeData = true;
        [HideInInspector] public bool isStoryTellerOpen = false;
        GameObject layoutHelper;
        GameObject messageTimerObject;
        Button helperButton;
        bool enableUpdating = false;
        bool sentSoundHelper = false;
        int dynamicMessageIndex = 0;
        string latestDynamicMessage;
        string latestSTMessage;
        string timeHelper;

        [System.Serializable]
        public class ChatItem
        {
            public string chatTitle = "Chat Title";
            public string individualName = "Name";
            public string individualSurname = "Surname";
            public Sprite individualPicture;
            public MessageChat chatAsset;
            public bool showAtStart = true;
        }

        void OnEnable()
        {
            if (chatList[0].showAtStart == false)
            {
                currentLayout = 0;
                selectedLayout = null;
            }

            if (isStoryTellerOpen == true && stIndexHelper == currentLayout && storyTellerAnimator != null)
                storyTellerAnimator.Play("In");
        }

        void Start()
        {
            InitializeChat();

            if (messageStoring != null)
                messageStoring.ReadMessageData();

            this.enabled = false;
        }

        void Update()
        {
            if (enableUpdating == true && Input.GetKeyDown(KeyCode.Return))
            {
                CreateCustomMessageFromInput(null, true);
                messageInput.text = "";
                messageInput.ActivateInputField();
            }
        }

        public void InitializeChat()
        {
            foreach (Transform child in chatsParent)
                Destroy(child.gameObject);

            foreach (Transform child in chatViewer)
                Destroy(child.gameObject);

            for (int i = 0; i < chatList.Count; ++i)
            {
                // Create chat layout
                GameObject clObj = Instantiate(chatLayout, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                clObj.transform.SetParent(chatViewer, false);
                clObj.gameObject.name = chatList[i].chatTitle;
                createdLayouts.Add(clObj);
                Transform messageParent = clObj.transform.Find("Content/Message List");

                // Create chat layout content
                for (int x = 0; x < chatList[i].chatAsset.messageList.Count; ++x)
                {
                    if (chatList[i].chatAsset.messageList[x].objectType == MessageChat.ObjectType.MESSAGE)
                    {
                        if (chatList[i].chatAsset.messageList[x].messageAuthor == MessageChat.MessageAuthor.INDIVIDUAL)
                        {
                            GameObject msgObj = Instantiate(chatMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            TextMeshProUGUI messageContent;
                            messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
                            messageContent.text = chatList[i].chatAsset.messageList[x].messageContent;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }

                        else
                        {
                            GameObject msgObj = Instantiate(chatMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            TextMeshProUGUI messageContent;
                            messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
                            messageContent.text = chatList[i].chatAsset.messageList[x].messageContent;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }
                    }

                    else if (chatList[i].chatAsset.messageList[x].objectType == MessageChat.ObjectType.AUDIO_MESSAGE)
                    {
                        if (chatList[i].chatAsset.messageList[x].messageAuthor == MessageChat.MessageAuthor.INDIVIDUAL
                            && audioMessageRecieved != null)
                        {
                            GameObject msgObj = Instantiate(audioMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            AudioMessage audioMessage;
                            audioMessage = msgObj.GetComponent<AudioMessage>();

                            if (audioPlayer == null)
                                audioPlayer = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

                            audioMessage.aSource = audioPlayer;
                            audioMessage.aClip = chatList[i].chatAsset.messageList[x].audioMessage;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }

                        else if (audioMessageSent != null)
                        {
                            GameObject msgObj = Instantiate(audioMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            AudioMessage audioMessage;
                            audioMessage = msgObj.GetComponent<AudioMessage>();
                            audioMessage.aSource = audioPlayer;
                            audioMessage.aClip = chatList[i].chatAsset.messageList[x].audioMessage;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }
                    }

                    else if (chatList[i].chatAsset.messageList[x].objectType == MessageChat.ObjectType.IMAGE_MESSAGE)
                    {
                        if (chatList[i].chatAsset.messageList[x].messageAuthor == MessageChat.MessageAuthor.INDIVIDUAL
                            && imageMessageRecieved != null)
                        {
                            GameObject msgObj = Instantiate(imageMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            ImageMessage imgMessage;
                            imgMessage = msgObj.GetComponent<ImageMessage>();
                            imgMessage.title = chatList[i].chatAsset.messageList[x].messageContent;
                            imgMessage.description = chatList[i].individualName + " " + chatList[i].individualSurname;
                            imgMessage.spriteVar = chatList[i].chatAsset.messageList[x].imageMessage;
                            imgMessage.imageObject.sprite = imgMessage.spriteVar;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }

                        else if (imageMessageSent != null)
                        {
                            GameObject msgObj = Instantiate(imageMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            msgObj.transform.SetParent(messageParent, false);

                            ImageMessage imgMessage;
                            imgMessage = msgObj.GetComponent<ImageMessage>();
                            imgMessage.title = chatList[i].chatAsset.messageList[x].messageContent;
                            imgMessage.description = chatList[i].individualName + " " + chatList[i].individualSurname;
                            imgMessage.spriteVar = chatList[i].chatAsset.messageList[x].imageMessage;
                            imgMessage.imageObject.sprite = imgMessage.spriteVar;

                            TextMeshProUGUI timeText;
                            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();
                            timeText.text = chatList[i].chatAsset.messageList[x].sentTime;
                        }
                    }

                    else
                    {
                        GameObject dateObj = Instantiate(messageDate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        dateObj.transform.SetParent(messageParent, false);

                        TextMeshProUGUI messageContent;
                        messageContent = dateObj.transform.Find("Date").GetComponent<TextMeshProUGUI>();
                        messageContent.text = chatList[i].chatAsset.messageList[x].messageContent;
                    }
                }

                // Create chat message button
                GameObject msgButton = Instantiate(chatMessageButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                msgButton.transform.SetParent(chatsParent, false);
                msgButton.gameObject.name = chatList[i].chatTitle;

                // Set profile picture
                Transform coverGO;
                coverGO = msgButton.transform.Find("Profile/Image").GetComponent<Transform>();
                coverGO.GetComponent<Image>().sprite = chatList[i].individualPicture;

                // Set date, text and name
                TextMeshProUGUI individualNameText;
                individualNameText = msgButton.transform.Find("From").GetComponent<TextMeshProUGUI>();
                individualNameText.text = chatList[i].individualName + " " + chatList[i].individualSurname;

                TextMeshProUGUI lastMessage;
                lastMessage = msgButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                lastMessage.text = chatList[i].chatAsset.messageList[chatList[i].chatAsset.messageList.Count - 1].messageContent;

                TextMeshProUGUI sentTime;
                sentTime = msgButton.transform.Find("Time").GetComponent<TextMeshProUGUI>();
                sentTime.text = chatList[i].chatAsset.messageList[chatList[i].chatAsset.messageList.Count - 1].sentTime;

                // Add button events
                Button itemButton;
                itemButton = msgButton.GetComponent<Button>();
                itemButton.onClick.AddListener(delegate
                {
                    if (selectedLayout != null && selectedLayout.name == itemButton.gameObject.name)
                        return;

                    StopCoroutine("DisableLayout");

                    if (selectedLayout != null)
                    {
                        layoutHelper = selectedLayout;
                        selectedLayout.GetComponent<Animator>().Play("Panel Out");
                        StartCoroutine("DisableLayout");
                    }

                    int indexHelper = 0;
                    for (int s = 0; s < createdLayouts.Count; s++)
                    {
                        if (createdLayouts[s].name == itemButton.gameObject.name)
                        {
                            selectedLayout = createdLayouts[s];
                            indexHelper = s;
                            currentLayout = s;
                            break;
                        }
                    }

                    selectedLayout.SetActive(true);
                    Image test = selectedLayout.transform.Find("Profile/Image").GetComponent<Image>();
                    selectedLayout.transform.Find("Profile/Image").GetComponent<Image>().sprite = chatList[indexHelper].individualPicture;
                    selectedLayout.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = chatList[indexHelper].individualName + " " + chatList[indexHelper].individualSurname;
                    selectedLayout.GetComponent<Animator>().Play("Panel In");

                    if (isStoryTellerOpen == true && stIndexHelper != indexHelper && storyTellerAnimator != null)
                        storyTellerAnimator.Play("Out");
                    else if (isStoryTellerOpen == true && stIndexHelper == indexHelper && storyTellerAnimator != null)
                        storyTellerAnimator.Play("In");
                });

                if (helperButton == null)
                    helperButton = itemButton;

                clObj.SetActive(false);

                if (chatList[i].showAtStart == false)
                    msgButton.SetActive(false);
            }
        }

        public void CreateMessageFromInput()
        {
            CreateCustomMessageFromInput(null, true);
        }

        void CreateCustomMessageFromInput(Transform parent, bool isSelf)
        {
            if (parent == null)
                parent = selectedLayout.transform;

            if (string.IsNullOrEmpty(messageInput.text) == true || messageInput.text == " ")
            {
                messageInput.text = "";
                return;
            }

            if (selectedLayout != null)
            {
                GameObject msgObj = Instantiate(chatMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

                TextMeshProUGUI messageContent;
                messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
                messageContent.text = messageInput.text;

                TextMeshProUGUI timeText;
                timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();

                GetTimeData();
                timeText.text = timeHelper;

                if (saveSentMessages == true && messageStoring != null && timeManager != null && isSelf == true)
                    messageStoring.ApplyMessageData(currentLayout, "standard", "self", messageInput.text, timeHelper);
                else if (saveSentMessages == true && messageStoring != null && timeManager != null && isSelf == false)
                    messageStoring.ApplyMessageData(currentLayout, "standard", "individual", messageInput.text, timeHelper);

                LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponentInParent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());
            }

            if (chatList[currentLayout].chatAsset.useDynamicMessages == true
                && selectedLayout != null && messageInput.text.Length >= 1)
            {
                latestDynamicMessage = messageInput.text.Replace("\n", "");
                CreateDynamicMessage(currentLayout, true);
            }

            if (debugStoryTeller == true && selectedLayout != null && messageInput.text.Length >= 1
                && chatList[currentLayout].chatAsset.useStoryTeller == true)
            {
                latestSTMessage = messageInput.text.Replace("\n", "");
                CreateStoryTeller(currentLayout, latestSTMessage);
            }

            messageInput.text = "";

            if (this.enabled == true && audioPlayer != null && sentSoundHelper == false)
                audioPlayer.PlayOneShot(sentMessageSound);

            sentSoundHelper = false;
        }

        public void CreateMessage(Transform parent, int layoutIndex, string msgContent)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            messageInput.text = msgContent;
            CreateCustomMessageFromInput(parent, true);
        }

        public void CreateIndividualMessage(Transform parent, int layoutIndex, string msgContent)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            sentSoundHelper = true;
            GameObject tempMsgObj = chatMessageSent;
            chatMessageSent = chatMessageRecieved;
            messageInput.text = msgContent;
            CreateCustomMessageFromInput(parent, false);
            chatMessageSent = tempMsgObj;

            if (this.enabled == false && useNotifications == true && notificationCreator != null)
            {
                for (int i = 0; i < chatList.Count; i++)
                {
                    if (parent.name == chatList[i].chatTitle)
                    {
                        notificationCreator.notificationTitle = chatList[i].individualName + " " + chatList[i].individualSurname;
                        notificationCreator.notificationDescription = msgContent;
                        notificationCreator.popupDescription = msgContent;
                        notificationCreator.CreateOnlyPopup();
                        break;
                    }
                }
            }

            else if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(receivedMessageSound);
        }

        public void CreateStoredMessage(int layoutIndex, string message, string time, bool isSelf)
        {
            Transform parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();
            GameObject msgObj;
          
            if (isSelf == true)
                msgObj = Instantiate(chatMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            else
                msgObj = Instantiate(chatMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            TextMeshProUGUI messageContent;
            messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
            messageContent.text = message;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();
            timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());
        }

        public void CreateCustomMessage(Transform parent, int layoutIndex, string message, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(chatMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            TextMeshProUGUI messageContent;
            messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
            messageContent.text = message;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();
            timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(sentMessageSound);

            if (saveSentMessages == true && messageStoring != null && timeManager != null)
                messageStoring.ApplyMessageData(currentLayout, "standard", "self", message, timeHelper);
        }

        public void CreateCustomIndividualMessage(Transform parent, int layoutIndex, string message, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(chatMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            TextMeshProUGUI messageContent;
            messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
            messageContent.text = message;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();
            timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == false && useNotifications == true && notificationCreator != null)
            {
                for (int i = 0; i < chatList.Count; i++)
                {
                    if (parent.name == chatList[i].chatTitle)
                    {
                        notificationCreator.notificationTitle = chatList[i].individualName + " " + chatList[i].individualSurname;
                        notificationCreator.notificationDescription = message;
                        notificationCreator.popupDescription = message;
                        notificationCreator.CreateOnlyPopup();
                        break;
                    }
                }
            }

            else if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(receivedMessageSound);

            if (saveSentMessages == true && messageStoring != null && timeManager != null)
                messageStoring.ApplyMessageData(currentLayout, "standard", "individual", message, timeHelper);
        }

        public void CreateDate(Transform parent, int layoutIndex, string date)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject dateObj = Instantiate(messageDate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            dateObj.transform.SetParent(parent, false);

            TextMeshProUGUI messageContent;
            messageContent = dateObj.transform.Find("Date").GetComponent<TextMeshProUGUI>();
            messageContent.text = date;

            LayoutRebuilder.ForceRebuildLayoutImmediate(dateObj.GetComponent<RectTransform>());
        }

        public void CreateImageMessage(Transform parent, int layoutIndex, Sprite sprite, string title, string description, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(imageMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            ImageMessage imgMessage;
            imgMessage = msgObj.GetComponent<ImageMessage>();
            imgMessage.title = title;
            imgMessage.description = description;
            imgMessage.spriteVar = sprite;
            imgMessage.imageObject.sprite = imgMessage.spriteVar;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();

            if (time == "")
            {
                GetTimeData();
                timeText.text = timeHelper;
            }

            else
                timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(sentMessageSound);
        }

        public void CreateIndividualImageMessage(Transform parent, int layoutIndex, Sprite sprite, string title, string description, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(imageMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            ImageMessage imgMessage;
            imgMessage = msgObj.GetComponent<ImageMessage>();
            imgMessage.title = title;
            imgMessage.description = description;
            imgMessage.spriteVar = sprite;
            imgMessage.imageObject.sprite = imgMessage.spriteVar;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();

            if (time == "")
            {
                GetTimeData();
                timeText.text = timeHelper;
            }

            else
                timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == false && useNotifications == true && notificationCreator != null)
            {
                for (int i = 0; i < chatList.Count; i++)
                {
                    if (parent.name == chatList[i].chatTitle)
                    {
                        notificationCreator.notificationTitle = chatList[i].individualName + " " + chatList[i].individualSurname;
                        notificationCreator.notificationDescription = imageMessageNotification + title;
                        notificationCreator.popupDescription = imageMessageNotification + title;
                        notificationCreator.CreateOnlyPopup();
                        break;
                    }
                }
            }

            else if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(receivedMessageSound);
        }

        public void CreateAudioMessage(Transform parent, int layoutIndex, AudioClip audio, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(audioMessageSent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            AudioMessage audioMessage;
            audioMessage = msgObj.GetComponent<AudioMessage>();
            audioMessage.aSource = audioPlayer;
            audioMessage.aClip = audio;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();

            if (time == "")
            {
                GetTimeData();
                timeText.text = timeHelper;
            }

            else
                timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(sentMessageSound);
        }

        public void CreateIndividualAudioMessage(Transform parent, int layoutIndex, AudioClip audio, string time)
        {
            if (parent == null)
                parent = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

            GameObject msgObj = Instantiate(audioMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            msgObj.transform.SetParent(parent.Find("Content/Message List"), false);

            AudioMessage audioMessage;
            audioMessage = msgObj.GetComponent<AudioMessage>();
            audioMessage.aSource = audioPlayer;
            audioMessage.aClip = audio;

            TextMeshProUGUI timeText;
            timeText = msgObj.transform.Find("Content/Time").GetComponent<TextMeshProUGUI>();

            if (time == "")
            {
                GetTimeData();
                timeText.text = timeHelper;
            }

            else
                timeText.text = time;

            LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

            if (this.enabled == false && useNotifications == true && notificationCreator != null)
            {
                for (int i = 0; i < chatList.Count; i++)
                {
                    if (parent.name == chatList[i].chatTitle)
                    {
                        notificationCreator.notificationTitle = chatList[i].individualName + " " + chatList[i].individualSurname;
                        notificationCreator.notificationDescription = audioMessageNotification;
                        notificationCreator.popupDescription = audioMessageNotification;
                        notificationCreator.CreateOnlyPopup();
                        break;
                    }
                }
            }

            else if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(receivedMessageSound);
        }

        public void CreateDynamicMessage(int layoutIndex, bool waitingForTimer = true)
        {
            for (int i = 0; i < chatList[layoutIndex].chatAsset.dynamicMessages.Count; i++)
            {
                if (latestDynamicMessage == chatList[layoutIndex].chatAsset.dynamicMessages[i].messageContent)
                {
                    if (chatList[layoutIndex].chatAsset.dynamicMessages[i].enableReply == false)
                        return;

                    dynamicMessageIndex = i;
                    break;
                }
            }

            if (waitingForTimer == false && chatList[layoutIndex].chatAsset.useDynamicMessages == true &&
                chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].messageContent == latestDynamicMessage)
            {
                GameObject msgObj = Instantiate(chatMessageRecieved, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                try
                {
                    Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();
                    msgObj.transform.SetParent(layoutObj.Find("Content/Message List"), false);
                }

                catch { msgObj.transform.SetParent(selectedLayout.transform.Find("Content/Message List"), false); }

                TextMeshProUGUI messageContent;
                messageContent = msgObj.transform.Find("Container/Content/Text").GetComponent<TextMeshProUGUI>();
                messageContent.text = chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].replyContent;

                TextMeshProUGUI timeText;
                timeText = msgObj.transform.Find("Container/Content/Time").GetComponent<TextMeshProUGUI>();

                GetTimeData();
                timeText.text = timeHelper;

                LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponentInParent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(msgObj.GetComponent<RectTransform>());

                if (saveSentMessages == true && messageStoring != null && timeManager != null)
                    messageStoring.ApplyMessageData(currentLayout, "standard", "individual", messageContent.text, timeHelper);
            }

            else if (waitingForTimer == true
                && chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].messageContent == latestDynamicMessage)
            {
                waitingForTimer = false;
                enableUpdating = false;
                StartCoroutine(DynamicMessageLatency(chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].replyLatency, layoutIndex));
            }
        }

        public void CreateStoryTeller(int layoutIndex, string itemID)
        {
            if (storyTellerAnimator == null || storyTellerList == null)
                return;

            bool catchedID = false;

            for (int i = 0; i < chatList[layoutIndex].chatAsset.storyTeller.Count; i++)
            {
                if (itemID == chatList[layoutIndex].chatAsset.storyTeller[i].itemID)
                {
                    storyTellerIndex = i;
                    catchedID = true;
                    break;
                }
            }

            if (catchedID == false)
                return;

            stIndexHelper = layoutIndex;

            foreach (Transform child in storyTellerList)
                Destroy(child.gameObject);

            if (chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageContent != ""
                && chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageAuthor == MessageChat.MessageAuthor.SELF)
            {
                StartCoroutine(StoryTellerMessageLatency(chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageLatency, layoutIndex, false)); 
            }

            else if (chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageContent != ""
               && chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageAuthor == MessageChat.MessageAuthor.INDIVIDUAL)
            {
                StartCoroutine(StoryTellerMessageLatency(chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageLatency, layoutIndex, true));
            }

            for (int i = 0; i < chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].replies.Count; i++)
            {
                GameObject strObj = Instantiate(storyTellerObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                strObj.transform.SetParent(storyTellerList, false);

                TextMeshProUGUI strBrief;
                strBrief = strObj.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                strBrief.text = chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].replies[i].replyBrief;

                StoryTellerItem sti;
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();
                sti = strObj.GetComponent<StoryTellerItem>();
                sti.layoutObj = layoutObj;
                sti.layoutIndex = layoutIndex;
                sti.itemIndex = i;
                sti.msgManager = this;
            }

            catchedID = false;
        }

        public void GetTimeData()
        {
            if (timeManager != null && timeManager.enableAmPm == true)
            {
                if (timeManager.currentMinute.ToString().Length == 1)
                    timeHelper = timeManager.currentHour + ":" + "0" + timeManager.currentMinute;
                else
                    timeHelper = timeManager.currentHour + ":" + timeManager.currentMinute;

                if (timeManager.enableAmPm == true && PlayerPrefs.GetInt("isAM") == 1)
                    timeHelper = timeHelper + " AM";
                else if (timeManager.enableAmPm == true && PlayerPrefs.GetInt("isAM") == 0)
                    timeHelper = timeHelper + " PM";
            }

            else if (timeManager != null && timeManager.enableAmPm == false)
            {
                if (timeManager.currentMinute.ToString().Length == 1)
                    timeHelper = timeManager.currentHour + ":" + "0" + timeManager.currentMinute;
                else
                    timeHelper = timeManager.currentHour + ":" + timeManager.currentMinute;
            }
        }

        public void EnableDynamicMessageReply(string msgID)
        {
            for (int i = 0; i < chatList[currentLayout].chatAsset.dynamicMessages.Count; i++)
            {
                if (msgID == chatList[currentLayout].chatAsset.dynamicMessages[i].messageID)
                {
                    chatList[currentLayout].chatAsset.dynamicMessages[i].enableReply = true;
                    break;
                }
            }
        }

        public void DisableDynamicMessageReply(string msgID)
        {
            for (int i = 0; i < chatList[currentLayout].chatAsset.dynamicMessages.Count; i++)
            {
                if (msgID == chatList[currentLayout].chatAsset.dynamicMessages[i].messageID)
                {
                    chatList[currentLayout].chatAsset.dynamicMessages[i].enableReply = false;
                    break;
                }
            }
        }

        public void EnableUpdating(bool helperValue)
        {
            if (helperValue == true)
                enableUpdating = true;
            else
                enableUpdating = false;
        }

        public void ShowChatOnEnable()
        {
            if (helperButton != null && chatList[0].showAtStart == true)
            {
                helperButton.onClick.Invoke();
                selectedLayout.GetComponent<Animator>().Play("Panel In");
            }
        }

        public void EnableChat(string chatTitle)
        {
            chatsParent.Find(chatTitle).gameObject.SetActive(true);
        }

        IEnumerator DisableLayout()
        {
            yield return new WaitForSeconds(0.5f);
            layoutHelper.SetActive(false);
        }

        IEnumerator DynamicMessageLatency(float timer, int layoutIndex)
        {
            yield return new WaitForSeconds(timer);
            StartCoroutine(DynamicMessageHelper(chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].replyTimer, layoutIndex));
            GameObject timerObj = Instantiate(chatMessageTimer, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            try
            {
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle + "/Content/Message List").GetComponent<Transform>();
                timerObj.transform.SetParent(layoutObj, false);
            }

            catch { Debug.LogError("<b>[Messaging Manager]</b> Layout parent cannot found."); }

            messageTimerObject = timerObj;
        }

        IEnumerator DynamicMessageHelper(float timer, int layoutIndex)
        {
            yield return new WaitForSeconds(timer);
            enableUpdating = true;
            Destroy(messageTimerObject);
            CreateDynamicMessage(layoutIndex, false);

            if (this.enabled == false && useNotifications == true && notificationCreator != null)
            {
                notificationCreator.notificationTitle = chatList[layoutIndex].individualName + " " + chatList[layoutIndex].individualSurname;
                notificationCreator.notificationDescription = chatList[layoutIndex].chatAsset.dynamicMessages[dynamicMessageIndex].replyContent;
                notificationCreator.popupDescription = notificationCreator.notificationDescription;
                notificationCreator.CreateOnlyPopup();
            }

            else if (this.enabled == true && audioPlayer != null)
                audioPlayer.PlayOneShot(receivedMessageSound);
        }

        public IEnumerator StoryTellerMessageLatency(float timer, int layoutIndex, bool isIndividual)
        {
            yield return new WaitForSeconds(timer);
            StartCoroutine(StoryTellerMessageHelper(chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageTimer, layoutIndex, isIndividual));
            GameObject timerObj = Instantiate(chatMessageTimer, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            try
            {
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle + "/Content/Message List").GetComponent<Transform>();
                timerObj.transform.SetParent(layoutObj, false);
            }

            catch { Debug.LogError("<b>[Messaging Manager]</b> Layout parent cannot found."); }

            messageTimerObject = timerObj;
        }

        public IEnumerator StoryTellerMessageHelper(float timer, int layoutIndex, bool isIndividual)
        {
            yield return new WaitForSeconds(timer);
            Destroy(messageTimerObject);

            if (getTimeData == true)
                GetTimeData();

            try
            {
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();

                if (isIndividual == true)
                    CreateCustomIndividualMessage(layoutObj, 0, chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageContent, timeHelper);
                else
                    CreateCustomMessage(layoutObj, 0, chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].messageContent, timeHelper);
            }

            catch { Debug.LogError("<b>[Messaging Manager]</b> Layout parent cannot found."); }

            if (stIndexHelper == currentLayout)
                storyTellerAnimator.Play("In");

            isStoryTellerOpen = true;
        }

        public IEnumerator StoryTellerLatency(float timer, int layoutIndex, int itemIndex)
        {
            yield return new WaitForSeconds(timer);
            StartCoroutine(StoryTellerHelper(chatList[layoutIndex].chatAsset.storyTeller[dynamicMessageIndex].replies[itemIndex].feedbackTimer, layoutIndex));

            GameObject timerObj = Instantiate(chatMessageTimer, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            try
            {
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle + "/Content/Message List").GetComponent<Transform>();
                timerObj.transform.SetParent(layoutObj, false);
            }

            catch { Debug.LogError("<b>[Messaging Manager]</b> Layout parent cannot found."); }
  
            messageTimerObject = timerObj;
        }

        IEnumerator StoryTellerHelper(float timer, int layoutIndex)
        {
            yield return new WaitForSeconds(timer);
            Destroy(messageTimerObject);

            try
            {
                Transform layoutObj = chatViewer.Find(chatList[layoutIndex].chatTitle).GetComponent<Transform>();
                CreateIndividualMessage(layoutObj, 0, chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].replies[stItemIndex].replyFeedback);
            }

            catch { Debug.LogError("<b>[Messaging Manager]</b> Layout parent cannot found."); }

            if (chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].replies[stItemIndex].callAfter != "")
                CreateStoryTeller(layoutIndex, chatList[layoutIndex].chatAsset.storyTeller[storyTellerIndex].replies[stItemIndex].callAfter);
        }

        public void AddChat()
        {
            ChatItem citem = new ChatItem();
            citem.chatTitle = "New Chat";
            chatList.Add(citem);
        }
    }
}