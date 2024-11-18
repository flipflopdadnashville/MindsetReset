using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowManager : MonoBehaviour, IPointerClickHandler
    {
        // Resources
        public GameObject taskBarButton;
        public WindowDragger windowDragger;
        public WindowResize windowResize;

        // Fullscreen & minimize
        public Image imageObject;
        public Sprite fullscreenImage;
        public Sprite minimizeImage;

        // Settings
        public bool enableBackgroundBlur = true;
        public bool hasNavDrawer = true;

        // Events
        public UnityEvent onEnableEvents;
        public UnityEvent onLaunchEvents;
        public UnityEvent onQuitEvents;

        Animator windowAnimator;
        Animator navDrawerAnimator;
        TaskBarButton tbbHelper;
        BlurManager windowBGBlur;
        RectTransform windowRect;

        float left;
        float right;
        float top;
        float bottom;
        bool isNavDrawerOpen = true;

        [HideInInspector] public bool isNormalized;
        [HideInInspector] public bool isFullscreen;
        [HideInInspector] public bool disableAtStart = true;

        void Start()
        {
            SetupWindow();
        }

        void OnEnable()
        {
            if (hasNavDrawer == true)
            {
                if (navDrawerAnimator == null)
                    navDrawerAnimator = transform.Find("Content").GetComponent<Animator>();

                if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "true")
                {
                    navDrawerAnimator.Play("Content Expand");
                    isNavDrawerOpen = true;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "false"
                    || !PlayerPrefs.HasKey(gameObject.name + "NavDrawer"))
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }
            }

            onEnableEvents.Invoke();
        }

        public void SetupWindow()
        {
            try { windowAnimator = gameObject.GetComponent<Animator>(); }

            catch
            {
                Debug.Log("<b>[Window Manager]</b> No Animator attached to Game Object. Window Manager won't be working properly.", this);
            }

            if (taskBarButton != null)
            {
                try
                {
                    tbbHelper = taskBarButton.GetComponent<TaskBarButton>();
                    tbbHelper.windowManager = this.GetComponent<WindowManager>();
                }

                catch
                {
                    Debug.Log("Window Manager - No variable attached to Task Bar Button. Task Bar functions won't be working properly.", this);
                }
            }

            if (enableBackgroundBlur == true)
            {
                try { windowBGBlur = gameObject.GetComponent<BlurManager>(); }

                catch
                {
                    Debug.Log("Window Manager - No Blur Manager attached to Game Object. Background Blur won't be working.", this);
                }
            }

            windowRect = gameObject.GetComponent<RectTransform>();

            if (hasNavDrawer == true)
            {
                navDrawerAnimator = transform.Find("Content").GetComponent<Animator>();

                if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "true")
                {
                    navDrawerAnimator.Play("Content Expand");
                    isNavDrawerOpen = true;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "false")
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "")
                {
                    navDrawerAnimator.Play("Content Minimize");
                    isNavDrawerOpen = false;
                }
            }

            if (windowDragger != null)
            {
                try { windowDragger.wManager = this.GetComponent<WindowManager>(); }

                catch
                {
                    Debug.Log("Window Manager - Window Manager has a missing variable.", this);
                }
            }

            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            try { imageObject.sprite = fullscreenImage; }

            catch
            {
                Debug.Log("Window Manager - No object attached to Image Object. Window Controls won't be working properly.", this);
            }

            if (disableAtStart == true)
                gameObject.SetActive(false);
        }

        public void NavDrawerAnimate()
        {
            if (isNavDrawerOpen == true)
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "false");
                navDrawerAnimator.Play("Content Minimize");
                isNavDrawerOpen = false;
            }

            else
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "true");
                navDrawerAnimator.Play("Content Expand");
                isNavDrawerOpen = true;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            FocusToWindow();
        }

        public void FocusToWindow()
        {
            gameObject.transform.SetAsLastSibling();
        }

        public void OpenWindow()
        {
            StopCoroutine("DisableObject");
            gameObject.SetActive(true);

            if (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Fullscreen")
                && (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Normalize")))
                windowAnimator.Play("Panel In");

            if (taskBarButton != null)
                tbbHelper.SetOpen();

            if (windowBGBlur != null)
                windowBGBlur.BlurInAnim();

            FocusToWindow();
            onLaunchEvents.Invoke();
        }

        public void CloseWindow()
        {
            if (taskBarButton != null)
                tbbHelper.SetClosed();

            if (enableBackgroundBlur == true && windowBGBlur != null)
                windowBGBlur.BlurOutAnim();

            StartCoroutine("DisableObject");
            windowAnimator.Play("Panel Out");
            onQuitEvents.Invoke();
        }

        public void MinimizeWindow()
        {
            windowAnimator.Play("Panel Minimize");

            if (taskBarButton != null)
                tbbHelper.SetMinimized();

            if (enableBackgroundBlur == true && windowBGBlur != null)
                windowBGBlur.BlurOutAnim();
        }

        public void FullscreenWindow()
        {
            if (isFullscreen == false)
            {
                isFullscreen = true;
                isNormalized = false;
                // windowDragger.gameObject.SetActive(false);
                imageObject.sprite = minimizeImage;
                StartCoroutine("SetFullscreen");
            }

            else
            {
                isFullscreen = false;
                isNormalized = true;
                // windowDragger.gameObject.SetActive(true);
                imageObject.sprite = fullscreenImage;
                StartCoroutine("SetNormalized");
            }
        }

        IEnumerator SetFullscreen()
        {
            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            windowAnimator.Play("Panel Fullscreen");

            // LEFT / BOTTOM
            windowRect.offsetMin = new Vector2(0, 0);

            // RIGHT / TOP
            windowRect.offsetMax = new Vector2(0, 0);

            isFullscreen = true;
            isNormalized = false;

            if (windowResize != null)
                windowResize.gameObject.SetActive(false);

            yield return null;
        }

        IEnumerator SetNormalized()
        {
            windowAnimator.Play("Panel Normalize");

            // LEFT / BOTTOM
            windowRect.offsetMin = new Vector2(left, bottom);

            // RIGHT / TOP
            windowRect.offsetMax = new Vector2(-right, -top);

            isFullscreen = false;
            isNormalized = true;

            if (windowResize != null)
                windowResize.gameObject.SetActive(true);

            yield return null;
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}