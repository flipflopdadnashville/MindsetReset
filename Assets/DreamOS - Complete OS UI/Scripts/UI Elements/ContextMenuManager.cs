using UnityEngine;

namespace Michsky.DreamOS
{
    public class ContextMenuManager : MonoBehaviour
    {
        // Resources
        [SerializeField]
        public Canvas mainCanvas;
        public GameObject contextObject;
        public GameObject contextContent;
        public Transform itemParent;

        // Settings
        public bool enableBlur = true;
        [HideInInspector] public bool isContextMenuOn;

        // Bounds
        [Range(-50, 50)] public int vBorderTop = 0;
        [Range(-50, 50)] public int vBorderBottom = 0;
        [Range(-50, 50)] public int hBorderLeft = 0;
        [Range(-50, 50)] public int hBorderRight = 0;

        Vector2 uiPos;
        Vector3 cursorPos;
        Vector3 contentPos = new Vector3(0, 0, 0);
        Vector3 contextVelocity = Vector3.zero;
        RectTransform contextRect;
        Animator contextAnimator;
        BlurManager contextBlur;

        void Start()
        {
            if (mainCanvas == null)
                mainCanvas = gameObject.GetComponentInParent<Canvas>();

            contextAnimator = contextObject.gameObject.GetComponent<Animator>();
            contextRect = contextObject.GetComponent<RectTransform>();
            contentPos = new Vector3(vBorderTop, hBorderLeft, 0);
            contextObject.transform.SetAsLastSibling();

            if (enableBlur == true)
                contextBlur = contextObject.gameObject.GetComponent<BlurManager>();
        }

        public void CheckForBounds()
        {
            if (uiPos.x <= -100)
            {
                contentPos = new Vector3(hBorderLeft, contentPos.y, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(0f, contextContent.GetComponent<RectTransform>().pivot.y);
            }

            else if (uiPos.x >= 100)
            {
                contentPos = new Vector3(hBorderRight, contentPos.y, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(1f, contextContent.GetComponent<RectTransform>().pivot.y);
            }

            if (uiPos.y <= -75)
            {
                contentPos = new Vector3(contentPos.x, vBorderBottom, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(contextContent.GetComponent<RectTransform>().pivot.x, 0f);
            }

            else if (uiPos.y >= 75)
            {
                contentPos = new Vector3(contentPos.x, vBorderTop, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(contextContent.GetComponent<RectTransform>().pivot.x, 1f);
            }
        }

        public void SetContextMenuPosition()
        {
            cursorPos = Input.mousePosition;
            uiPos = contextRect.anchoredPosition;
            CheckForBounds();

            if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || mainCanvas.renderMode == RenderMode.WorldSpace)
            {
                cursorPos.z = gameObject.transform.position.z;
                contextRect.position = Camera.main.ScreenToWorldPoint(cursorPos);
                contextContent.transform.localPosition = Vector3.SmoothDamp(contextContent.transform.localPosition, contentPos, ref contextVelocity, 0);
            }

            else if (mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                contextRect.position = cursorPos;
                // contextContent.transform.position = Vector3.SmoothDamp(contextContent.transform.position, cursorPos + contentPos, ref contextVelocity, 0);
                contextContent.transform.position = new Vector3(cursorPos.x + contentPos.x, cursorPos.y + contentPos.y, 0);
            }
        }

        public void CloseOnClick()
        {
            if (enableBlur == true)
                contextBlur.BlurOutAnim();

            contextAnimator.Play("Menu Out");
            isContextMenuOn = false;
        }
    }
}