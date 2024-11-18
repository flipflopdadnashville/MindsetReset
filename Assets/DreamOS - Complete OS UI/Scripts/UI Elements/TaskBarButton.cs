using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.DreamOS
{
    public class TaskBarButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        public string buttonTitle = "App Window";
        public bool alwaysPinAtStart;
        public bool disableAtStart = false;

        [Header("Events")]
        public UnityEvent onClickEvents;

        Animator buttonAnimator;
        [HideInInspector] public bool isPinned;
        [HideInInspector] public int buttonID;
        public WindowManager windowManager;

        bool firstTime = true;

        void Start()
        {
            buttonAnimator = gameObject.GetComponent<Animator>();

            if(alwaysPinAtStart == true && !PlayerPrefs.HasKey(buttonTitle + "TaskbarShortcut"))
                PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "true");

            if (PlayerPrefs.GetString(buttonTitle + "TaskbarShortcut") == "true")
                isPinned = true;
            else
                isPinned = false;

            if (isPinned == true)
                buttonAnimator.Play("Draw");
            else
                buttonAnimator.Play("Hide");

            firstTime = false;

            if (disableAtStart == true)
                gameObject.SetActive(false);
        }

        void OnEnable()
        {
            if (firstTime == false && buttonAnimator != null)
            {
                if (isPinned == true)
                    buttonAnimator.Play("Draw");
                else
                    buttonAnimator.Play("Hide");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted")
                && windowManager.transform.GetSiblingIndex() != windowManager.transform.parent.childCount - 1)
            {
                windowManager.FocusToWindow();
                buttonAnimator.Play("Inactive to Active");
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Active")
                && windowManager.transform.GetSiblingIndex() == windowManager.transform.parent.childCount - 1)
            {
                windowManager.MinimizeWindow();
                buttonAnimator.Play("Active to Inactive");              
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Inactive")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Inactive"))
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Highlighted to Active");
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Hide to Active");
            }

            else
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Closed to Active");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Closed")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Closed"))
                buttonAnimator.Play("Closed to Highlighted");

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Active") ||
                     buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Active")
                     || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide to Active"))
                buttonAnimator.Play("Active to Highlighted");

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Inactive")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Inactive"))
                buttonAnimator.Play("Inactive to Highlighted");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Highlighted"))
                buttonAnimator.Play("Highlighted to Closed");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted"))
                buttonAnimator.Play("Highlighted to Active");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted"))
                buttonAnimator.Play("Highlighted to Inactive");
        }

        public void SetOpen()
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted") || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted"))
                buttonAnimator.Play("Highlighted to Active");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                buttonAnimator.Play("Hide to Active");
            else
                buttonAnimator.Play("Closed to Active");
        }

        public void SetClosed()
        {
            if (PlayerPrefs.GetString(buttonTitle + "TaskbarShortcut") == "true")
                buttonAnimator.Play("Active to Closed");
            else
                buttonAnimator.Play("Hide");
        }

        public void SetMinimized()
        {
            buttonAnimator.Play("Active to Inactive");
        }

        public void UnpinTaskBarButton()
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Closed"))
                buttonAnimator.Play("Hide");

            PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "false");
            isPinned = false;
        }

        public void PinTaskBarButton()
        {
            PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "true");
            isPinned = true;
        }
    }
}