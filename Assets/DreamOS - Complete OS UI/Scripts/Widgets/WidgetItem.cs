using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Michsky.DreamOS
{
    public class WidgetItem : MonoBehaviour, IEndDragHandler
    {
        [Header("Resources")]
        public Animator widgetAnimator;

        [Header("Settings")]
        public bool isOn;

        [Header("Events")]
        public UnityEvent enabledEvents;
        public UnityEvent disabledEvents;

        float widgetPosX;
        float widgetPosY;
        bool firstTime = true;

        void Start()
        {
            PrepareWidget();
            firstTime = false;
        }

        void OnEnable()
        {
            if (firstTime == true)
                return;

            if (PlayerPrefs.GetString(gameObject.name + "Widget") == "enabled")
                widgetAnimator.Play("Widget In");
            else
                widgetAnimator.Play("Widget Out");
        }

        public void PrepareWidget()
        {
            if (widgetAnimator != null)
                widgetAnimator = gameObject.GetComponent<Animator>();

            if (!PlayerPrefs.HasKey(gameObject.name + "Widget") && isOn == true)
            {
                AlignToCenter();
                widgetPosX = gameObject.transform.position.x;
                PlayerPrefs.SetFloat(gameObject.name + "WidgetPosX", widgetPosX);
                widgetPosY = gameObject.transform.position.y;
                PlayerPrefs.SetFloat(gameObject.name + "WidgetPosY", widgetPosY);
                widgetAnimator.Play("Widget In");
                enabledEvents.Invoke();
                PlayerPrefs.SetString(gameObject.name + "Widget", "enabled");
                isOn = true;
            }

            else if (!PlayerPrefs.HasKey(gameObject.name + "Widget") && isOn == false)
            {
                AlignToCenter();
                widgetPosX = gameObject.transform.position.x;
                PlayerPrefs.SetFloat(gameObject.name + "WidgetPosX", widgetPosX);
                widgetPosY = gameObject.transform.position.y;
                PlayerPrefs.SetFloat(gameObject.name + "WidgetPosY", widgetPosY);
                widgetAnimator.Play("Widget Out");
                disabledEvents.Invoke();
                PlayerPrefs.SetString(gameObject.name + "Widget", "disabled");
                isOn = false;
            }

            else if (PlayerPrefs.GetString(gameObject.name + "Widget") == "enabled")
            {
                widgetPosX = PlayerPrefs.GetFloat(gameObject.name + "WidgetPosX");
                widgetPosY = PlayerPrefs.GetFloat(gameObject.name + "WidgetPosY");
                gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
                widgetAnimator.Play("Widget In");
                enabledEvents.Invoke();
                isOn = true;
            }

            else if (PlayerPrefs.GetString(gameObject.name + "Widget") == "disabled")
            {
                widgetAnimator.Play("Widget Out");
                disabledEvents.Invoke();
                isOn = false;
            }
        }

        public void EnableWidget()
        {
            StopCoroutine("DisableObject");
            gameObject.SetActive(true);
            widgetPosX = PlayerPrefs.GetFloat(gameObject.name + "WidgetPosX");
            widgetPosY = PlayerPrefs.GetFloat(gameObject.name + "WidgetPosY");
            gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
            widgetAnimator.Play("Widget In");
            enabledEvents.Invoke();
            PlayerPrefs.SetString(gameObject.name + "Widget", "enabled");
            isOn = true;
        }

        public void DisableWidget()
        {
            widgetAnimator.Play("Widget Out");
            disabledEvents.Invoke();
            PlayerPrefs.SetString(gameObject.name + "Widget", "disabled");
            isOn = false;
            StartCoroutine("DisableObject");
        }

        public void AlignToCenter()
        {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            widgetPosX = gameObject.transform.position.x;
            PlayerPrefs.SetFloat(gameObject.name + "WidgetPosX", widgetPosX);
            widgetPosY = gameObject.transform.position.y;
            PlayerPrefs.SetFloat(gameObject.name + "WidgetPosY", widgetPosY);
        }

        public void OnEndDrag(PointerEventData data)
        {
            widgetPosX = gameObject.transform.position.x;
            PlayerPrefs.SetFloat(gameObject.name + "WidgetPosX", widgetPosX);
            widgetPosY = gameObject.transform.position.y;
            PlayerPrefs.SetFloat(gameObject.name + "WidgetPosY", widgetPosY);
            gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            StopCoroutine("DisableObject");
        }
    }
}