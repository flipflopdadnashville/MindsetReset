using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.DreamOS
{
    [DisallowMultipleComponent]
    public class ItemDragger : UIBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Resources")]
        public ItemDragContainer dragContainer;
        private RectTransform dragObject;

        [Header("Settings")]
        public bool rememberPosition = false;

        private Vector2 originalLocalPointerPosition;
        private Vector3 originalPanelLocalPosition;

        private RectTransform dragObjectInternal
        {
            get
            {
                if (dragObject == null)
                    return (transform as RectTransform);
                else
                    return dragObject;
            }
        }

        private RectTransform dragAreaInternal
        {
            get
            {
                if (dragContainer.dragBorder != null)
                {
                    RectTransform newArea = transform.parent as RectTransform;
                    return newArea;
                }

                else
                    return dragContainer.dragBorder;
            }
        }

        public new void Start()
        {
            if (dragContainer == null)
            {
                if (gameObject.GetComponentInParent<ItemDragContainer>() == null)
                    transform.parent.gameObject.AddComponent<ItemDragContainer>();

                dragContainer = gameObject.GetComponentInParent<ItemDragContainer>();
            }

            if (rememberPosition == true && dragContainer.dragMode == ItemDragContainer.DragMode.SNAPPED
                && PlayerPrefs.HasKey(gameObject.name + "DraggerIndex"))
            {
                transform.SetSiblingIndex(PlayerPrefs.GetInt(gameObject.name + "DraggerIndex"));
            }

            else if (rememberPosition == true && dragContainer.dragMode == ItemDragContainer.DragMode.FREE
                && PlayerPrefs.HasKey(gameObject.name + "DraggerXPos"))
            {
                if (dragContainer.gridLayoutGroup.enabled == true)
                {
                    StartCoroutine("UpdateFreePosition");
                    return;
                }

                float x = PlayerPrefs.GetFloat(gameObject.name + "DraggerXPos");
                float y = PlayerPrefs.GetFloat(gameObject.name + "DraggerYPos");
                Vector3 tempPos = new Vector3(x, y, 0);
                transform.position = tempPos;
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (dragContainer.dragMode == ItemDragContainer.DragMode.SNAPPED)
                dragContainer.objectBeingDragged = this.gameObject;
            else
            {
                this.transform.SetAsLastSibling();
                originalPanelLocalPosition = dragObjectInternal.localPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(dragAreaInternal, data.position, data.pressEventCamera, out originalLocalPointerPosition);
            }
        }

        public void OnDrag(PointerEventData data) 
        {
            if (dragContainer.dragMode == ItemDragContainer.DragMode.FREE)
            {
                Vector2 localPointerPosition;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(dragAreaInternal, data.position, data.pressEventCamera, out localPointerPosition))
                {
                    Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                    dragObjectInternal.localPosition = originalPanelLocalPosition + offsetToOriginal;
                }

                ClampToArea();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragContainer.dragMode == ItemDragContainer.DragMode.SNAPPED)
            {
                if (dragContainer.objectBeingDragged == this.gameObject)
                    dragContainer.objectBeingDragged = null;

                if (rememberPosition == true)
                    PlayerPrefs.SetInt(gameObject.name + "DraggerIndex", transform.GetSiblingIndex());
            }

            else if (dragContainer.dragMode == ItemDragContainer.DragMode.FREE && rememberPosition == true)
            {
                PlayerPrefs.SetFloat(gameObject.name + "DraggerXPos", transform.position.x);
                PlayerPrefs.SetFloat(gameObject.name + "DraggerYPos", transform.position.y);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (dragContainer.dragMode != ItemDragContainer.DragMode.SNAPPED)
                return;

            GameObject objectBeingDragged = dragContainer.objectBeingDragged;
            
            if (objectBeingDragged != null && objectBeingDragged != this.gameObject)
                objectBeingDragged.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        }

        public void ClampToArea()
        {
            Vector3 pos = dragObjectInternal.localPosition;
            Vector3 minPosition = dragAreaInternal.rect.min - dragObjectInternal.rect.min;
            Vector3 maxPosition = dragAreaInternal.rect.max - dragObjectInternal.rect.max;
            pos.x = Mathf.Clamp(dragObjectInternal.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(dragObjectInternal.localPosition.y, minPosition.y, maxPosition.y);
            dragObjectInternal.localPosition = pos;
        }

        public void RemoveData()
        {
            PlayerPrefs.DeleteKey(gameObject.name + "DraggerXPos");
            PlayerPrefs.DeleteKey(gameObject.name + "DraggerYPos");
            PlayerPrefs.DeleteKey(gameObject.name + "DraggerIndex");
        }

        IEnumerator UpdateFreePosition()
        {
            yield return new WaitForSeconds(0.11f);
            float x = PlayerPrefs.GetFloat(gameObject.name + "DraggerXPos");
            float y = PlayerPrefs.GetFloat(gameObject.name + "DraggerYPos");
            Vector3 tempPos = new Vector3(x, y, 0);
            transform.position = tempPos;
        }
    }
}