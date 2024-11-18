using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.DreamOS
{
    public class ContextMenuContent : MonoBehaviour, IPointerClickHandler
    {
        [Header("Resources")]
        public ContextMenuManager contextManager;
        public GameObject buttonItem;
        public Transform itemParent;

        [Header("Items")]
        public List<ContextItem> contexItems = new List<ContextItem>();

        Animator contextAnimator;
        BlurManager contextBlur;
        GameObject selectedItem;
        Image setItemImage;
        TextMeshProUGUI setItemText;
        Sprite imageHelper;
        string textHelper;

        [System.Serializable]
        public class ContextItem
        {
            public string itemText;
            public Sprite itemIcon;
            public ContextItemType contextItemType;
            public UnityEvent onClickEvents;
        }

        public enum ContextItemType
        {
            BUTTON
        }

        void Start()
        {
            if (contextManager == null)
            {
                try { contextManager = (ContextMenuManager)GameObject.FindObjectsOfType(typeof(ContextMenuManager))[0]; }
                catch { Debug.Log("<b>[Context Menu]</b> No variable attached to Context Manager. It won't be working properly.", this); }
            }

            if (itemParent == null)
            {
                try { itemParent = GameObject.Find("/Canvas/Context Menu/Content/Item List").transform; }
                catch { itemParent = contextManager.itemParent; }
            }

            if (contextManager.enableBlur == true)
            {
                try { contextBlur = contextManager.contextObject.gameObject.GetComponent<BlurManager>(); }
                catch { Debug.Log("Context Menu - No Blur Manager attached to Context Manager. Background Blur won't be working.", this); }
            }

            try { contextAnimator = contextManager.contextObject.gameObject.GetComponent<Animator>(); }
            catch { Debug.Log("Context Menu - No Animator attached to Context Animator. Context Menu won't be working properly.", this); }

            if (itemParent != null)
                foreach (Transform child in itemParent)
                    Destroy(child.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (contextManager.isContextMenuOn == true)
            {
                if (contextManager.enableBlur == true)
                    contextBlur.BlurOutAnim();

                contextAnimator.Play("Menu Out");
                contextManager.isContextMenuOn = false;
            }

            else if (eventData.button == PointerEventData.InputButton.Right && contextManager.isContextMenuOn == false)
            {
                foreach (Transform child in itemParent)
                    Destroy(child.gameObject);

                for (int i = 0; i < contexItems.Count; ++i)
                {
                    if (contexItems[i].contextItemType == ContextItemType.BUTTON)
                        selectedItem = buttonItem;

                    GameObject go = Instantiate(selectedItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(itemParent, false);

                    setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                    textHelper = contexItems[i].itemText;
                    setItemText.text = textHelper;

                    Transform goImage;
                    goImage = go.gameObject.transform.Find("Icon");
                    setItemImage = goImage.GetComponent<Image>();
                    imageHelper = contexItems[i].itemIcon;
                    setItemImage.sprite = imageHelper;

                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(contexItems[i].onClickEvents.Invoke);
                    itemButton.onClick.AddListener(CloseOnClick);

                    StartCoroutine(ExecuteAfterTime(0.01f));
                }

                contextManager.SetContextMenuPosition();

                if (contextManager.enableBlur == true)
                    contextBlur.BlurInAnim();

                contextAnimator.Play("Menu In");
                contextManager.isContextMenuOn = true;
                contextManager.SetContextMenuPosition();
            }
        }

        // This is for forcing Unity UI to refresh the objects. RebuildLayout is not that accurate for this function.
        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            itemParent.gameObject.SetActive(false);
            itemParent.gameObject.SetActive(true);
            StopCoroutine(ExecuteAfterTime(0.01f));
            StopCoroutine("ExecuteAfterTime");
        }

        public void CloseOnClick()
        {
            if (contextManager.enableBlur == true)
                contextBlur.BlurOutAnim();

            contextAnimator.Play("Menu Out");
            contextManager.isContextMenuOn = false;
        }

        public void CreateNewButton(string title, Sprite icon)
        {
            ContextItem item = new ContextItem();
            item.itemText = title;
            item.itemIcon = icon;
            contexItems.Add(item);
        }
    }
}