using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Michsky.DreamOS
{
    public class CommanderManager : MonoBehaviour
    {
        // Command List
        public List<CommandItem> commands = new List<CommandItem>();

        // Settings
        [TextArea] public string errorText = "is not recognized as a command.";
        [TextArea] public string onEnableText = "Welcome to Commander.";
        public Color textColor;

        // Resources
        public TMP_InputField commandInput;
        public TextMeshProUGUI commandHistory;
        public Scrollbar scrollbar;

        // External
        public bool getTimeData;
        public GlobalTime timeManager;
        public Color timeColorCode = new Color(0, 255, 0);

        string currentCommand;
        int commandIndex;

        [System.Serializable]
        public class CommandItem
        {
            public string commandName = "Connect To Network";
            public string command = "Root";
            [TextArea] public string feedbackText;
            public float feedbackDelay = 0;
            public float onProcessDelay = 0;
            public UnityEvent onProcessEvent;
        }

        void OnEnable()
        {
            commandHistory.text = "";
            commandInput.text = "";
            UpdateColors();

            if (getTimeData == true && timeManager != null)
                UpdateTime();

            commandHistory.text = commandHistory.text + onEnableText;
        }

        void Start()
        {
            this.enabled = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                ProcessCommand();
        }

        public void UpdateColors()
        {
            commandInput.textComponent.color = textColor;
            commandHistory.color = textColor;
        }
     
        public void ProcessCommand()
        {
            if (commandInput.text == "")
                return;

            currentCommand = "";
            commandIndex = -1;
            currentCommand = commandInput.text;

            for (int i = 0; i < commands.Count; i++)
            {
                if (currentCommand == commands[i].command)
                {
                    currentCommand = commands[i].command;
                    commandIndex = i;
                    break;
                }
            }

            commandHistory.text = commandHistory.text + "\n";

            if (getTimeData == true && timeManager != null)
            {
                if (getTimeData == true && timeManager != null)
                    UpdateTime();
            }

            commandHistory.text = commandHistory.text + commandInput.text;

            if (commandIndex == -1)
            {
                commandHistory.text = commandHistory.text + "\n";

                if (getTimeData == true && timeManager != null)
                    UpdateTime();

                commandHistory.text = commandHistory.text + "'" + commandInput.text + "' " + errorText;
                commandInput.text = "";
                LayoutRebuilder.ForceRebuildLayoutImmediate(commandInput.transform.parent.GetComponent<RectTransform>());

                if (scrollbar != null)
                    scrollbar.value = 0;

                return;
            }

            if (commands[commandIndex].feedbackText != "")
                StartCoroutine("WaitForFeedbackDelay", commands[commandIndex].feedbackDelay);

            if (commands[commandIndex].onProcessDelay == 0)
                commands[commandIndex].onProcessEvent.Invoke();
            else
                StartCoroutine("WaitForProcessDelay", commands[commandIndex].onProcessDelay);

            commandInput.text = "";
            commandInput.ActivateInputField();
            LayoutRebuilder.ForceRebuildLayoutImmediate(commandInput.transform.parent.GetComponent<RectTransform>());

            if (scrollbar != null)
                scrollbar.value = 0;
        }

        public void UpdateTime()
        {
            if (getTimeData == true && timeManager != null)
            {
                string colorValue = ColorUtility.ToHtmlStringRGB(timeColorCode);
                commandHistory.text = commandHistory.text + "<color=#" + colorValue + ">[";

                if (timeManager.currentHour.ToString().Length == 1)
                    commandHistory.text = commandHistory.text + "0" + timeManager.currentHour + ":";
                else
                    commandHistory.text = commandHistory.text + timeManager.currentHour + ":";

                if (timeManager.currentMinute.ToString().Length == 1)
                    commandHistory.text = commandHistory.text + "0" + timeManager.currentMinute + ":";
                else
                    commandHistory.text = commandHistory.text + timeManager.currentMinute + ":";

                if (timeManager.currentSecond.ToString("F0").Length == 1)
                    commandHistory.text = commandHistory.text + "0" + timeManager.currentSecond.ToString("F0");
                else
                    commandHistory.text = commandHistory.text + timeManager.currentSecond.ToString("F0");

                commandHistory.text = commandHistory.text + "]</color> ";
            }
        }

        public void AddToHistory(string text)
        {
            UpdateTime();
            commandHistory.text = commandHistory.text + text;
        }

        public void ClearHistory()
        {
            commandHistory.text = "";
            AddToHistory("Clear completed.");
            LayoutRebuilder.ForceRebuildLayoutImmediate(commandInput.transform.parent.GetComponent<RectTransform>());
        }

        public void AddNewCommand()
        {
            commands.Add(null);
        }

        IEnumerator WaitForFeedbackDelay(float forSec)
        {
            yield return new WaitForSeconds(forSec);
            commandHistory.text = commandHistory.text + "\n";
            UpdateTime();
            commandHistory.text = commandHistory.text + commands[commandIndex].feedbackText;
            StopCoroutine("WaitForFeedbackDelay");
            LayoutRebuilder.ForceRebuildLayoutImmediate(commandInput.transform.parent.GetComponent<RectTransform>());
        }

        IEnumerator WaitForProcessDelay(float forSec)
        {
            yield return new WaitForSeconds(forSec);
            commands[commandIndex].onProcessEvent.Invoke();
            StopCoroutine("WaitForProcessDelay");
        }
    }
}