using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.DreamOS
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class ItemDragContainer : MonoBehaviour
    {
        [Header("Resources")]
        public GridLayoutGroup gridLayoutGroup;
        public RectTransform dragBorder;

        [Header("Settings")]
        public DragMode dragMode = DragMode.FREE;

        public enum DragMode
        {
            SNAPPED,
            FREE
        }

        public GameObject objectBeingDragged { get; set; }

        void Awake()
        {
            objectBeingDragged = null;

            if (gridLayoutGroup == null)
                gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

            if (dragBorder == null)
                dragBorder = gameObject.GetComponent<RectTransform>();

            StartCoroutine("ApplyDragModeWithTimer");
        }

        public void FreeDragMode()
        {
            dragMode = DragMode.FREE;
            gridLayoutGroup.enabled = false;
        }

        public void SnappedDragMode()
        {
            dragMode = DragMode.SNAPPED;
            gridLayoutGroup.enabled = true;
        }

        public void ApplyDragMode()
        {
            if (dragMode == DragMode.FREE)
                gridLayoutGroup.enabled = false;
            else
                gridLayoutGroup.enabled = true;

            StopCoroutine("ApplyDragModeWithTimer");
        }

        IEnumerator ApplyDragModeWithTimer()
        {
            // For those upgrading from 1.0.8 to 1.0.9
            yield return new WaitForSeconds(1f);
            ApplyDragMode();
        }
    }
}