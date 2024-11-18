using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Michsky.DreamOS
{
    public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        // Content
        public string buttonText = "Button";
        public Sprite buttonIcon;
        public AudioClip hoverSound;
        public AudioClip clickSound;
        private Button buttonVar;

        // Resources
        public TextMeshProUGUI normalText;
        public TextMeshProUGUI highlightedText;
        public Image normalIcon;
        public Image highlightedIcon;
        public AudioSource soundSource;
        public GameObject rippleParent;

        // Settings
        public AnimationSolution animationSolution = AnimationSolution.ANIMATOR;
        [Range(0.25f, 15)] public float fadingMultiplier = 8;
        public bool useCustomContent = false;
        public bool enableIcon = false;
        public bool enableButtonSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;
        public bool useRipple = true;

        // Ripple
        public RippleUpdateMode rippleUpdateMode = RippleUpdateMode.UNSCALED_TIME;
        public Sprite rippleShape;
        [Range(0.1f, 5)] public float speed = 1f;
        [Range(0.5f, 25)] public float maxSize = 4f;
        public Color startColor = new Color(1f, 1f, 1f, 1f);
        public Color transitionColor = new Color(1f, 1f, 1f, 1f);
        public bool renderOnTop = false;
        public bool centered = false;

        bool isPointerOn;
        float currentNormalValue;
        float currenthighlightedValue;
        CanvasGroup normalCG;
        CanvasGroup highlightedCG;

        public enum AnimationSolution
        {
            ANIMATOR,
            SCRIPT
        }

        public enum RippleUpdateMode
        {
            NORMAL,
            UNSCALED_TIME
        }

        void OnEnable()
        {
            // If custom content is disabled, then update the values
            if (useCustomContent == false)
                UpdateUI();

            if (normalCG == null && highlightedCG == null)
                return;

            normalCG.alpha = 1;
            highlightedCG.alpha = 0;
        }

        void Start()
        {
            maxSize = Mathf.Clamp(maxSize, 0.5f, 1000f);

            if (animationSolution == AnimationSolution.SCRIPT)
            {
                try
                {
                    normalCG = transform.Find("Normal").GetComponent<CanvasGroup>();
                    highlightedCG = transform.Find("Highlighted").GetComponent<CanvasGroup>();

                    Animator tempAnimator = this.GetComponent<Animator>();
                    Destroy(tempAnimator);
                }

                catch { animationSolution = AnimationSolution.ANIMATOR; }
            }

            if (buttonVar == null)
                buttonVar = gameObject.GetComponent<Button>();

            if (enableButtonSounds == true)
            {
                try { buttonVar = gameObject.GetComponent<Button>(); }

                catch
                {
                    gameObject.AddComponent<Button>();
                    buttonVar = gameObject.GetComponent<Button>();
                }

                if (useClickSound == true)
                    buttonVar.onClick.AddListener(delegate { soundSource.PlayOneShot(clickSound); });
            }

            // If ripple is disabled, Destroy it for optimization
            if (useRipple == true && rippleParent != null)
                rippleParent.SetActive(false);
            else if (useRipple == false && rippleParent != null)
                Destroy(rippleParent);
        }

        public void UpdateUI()
        {
            // Update the values when this function is called
            normalText.text = buttonText;
            highlightedText.text = buttonText;

            if (enableIcon == true)
            {
                normalIcon.sprite = buttonIcon;
                highlightedIcon.sprite = buttonIcon;
            }
        }

        public void CreateRipple(Vector2 pos)
        {
            // If Ripple Parent is assigned, create the object and get the necessary components
            if (rippleParent != null)
            {
                GameObject rippleObj = new GameObject();
                rippleObj.AddComponent<Image>();
                rippleObj.GetComponent<Image>().sprite = rippleShape;
                rippleObj.name = "Ripple";
                rippleParent.SetActive(true);
                rippleObj.transform.SetParent(rippleParent.transform);

                if (renderOnTop == true)
                    rippleParent.transform.SetAsLastSibling();
                else
                    rippleParent.transform.SetAsFirstSibling();

                if (centered == true)
                    rippleObj.transform.localPosition = new Vector2(0f, 0f);
                else
                    rippleObj.transform.position = pos;

                rippleObj.AddComponent<Ripple>();
                Ripple tempRipple = rippleObj.GetComponent<Ripple>();
                tempRipple.speed = speed;
                tempRipple.maxSize = maxSize;
                tempRipple.startColor = startColor;
                tempRipple.transitionColor = transitionColor;

                if (rippleUpdateMode == RippleUpdateMode.NORMAL)
                    tempRipple.unscaledTime = false;
                else
                    tempRipple.unscaledTime = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (useRipple == true && isPointerOn == true)
                CreateRipple(Input.mousePosition);
            else if (useRipple == false)
                this.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Process On Pointer Enter events
            if (enableButtonSounds == true && useHoverSound == true && buttonVar.interactable == true)
                soundSource.PlayOneShot(hoverSound);

            isPointerOn = true;

            if (animationSolution == AnimationSolution.SCRIPT && buttonVar.interactable == true)
                StartCoroutine("FadeIn");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Process On Pointer Exit events
            isPointerOn = false;

            if (animationSolution == AnimationSolution.SCRIPT && buttonVar.interactable == true)
                StartCoroutine("FadeOut");
        }

        IEnumerator FadeIn()
        {
            StopCoroutine("FadeOut");
            currentNormalValue = normalCG.alpha;
            currenthighlightedValue = highlightedCG.alpha;

            while (currenthighlightedValue <= 1)
            {
                currentNormalValue -= Time.deltaTime * fadingMultiplier;
                normalCG.alpha = currentNormalValue;

                currenthighlightedValue += Time.deltaTime * fadingMultiplier;
                highlightedCG.alpha = currenthighlightedValue;

                if (normalCG.alpha >= 1)
                    StopCoroutine("FadeIn");

                yield return null;
            }
        }

        IEnumerator FadeOut()
        {
            StopCoroutine("FadeIn");
            currentNormalValue = normalCG.alpha;
            currenthighlightedValue = highlightedCG.alpha;

            while (currentNormalValue >= 0)
            {
                currentNormalValue += Time.deltaTime * fadingMultiplier;
                normalCG.alpha = currentNormalValue;

                currenthighlightedValue -= Time.deltaTime * fadingMultiplier;
                highlightedCG.alpha = currenthighlightedValue;

                if (highlightedCG.alpha <= 0)
                    StopCoroutine("FadeOut");

                yield return null;
            }
        }
    }
}