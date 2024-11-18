using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalWindowManager : MonoBehaviour
    {
        // Content
        public Sprite windowIcon;
        public string titleText = "Title";
        [TextArea] public string descriptionText = "Description here";

        // Resources
        public Animator mwAnimator;
        public Image iconImage;
        public TextMeshProUGUI windowTitle;
        public TextMeshProUGUI windowDescription;
        public Button confirmButton;
        public Button cancelButton;
        public BlurManager blurManager;

        // Settings
        public bool useBlur = true;
        public bool useCustomValues = false;

        // Events
        public UnityEvent onConfirm;
        public UnityEvent onCancel;

        [HideInInspector] public bool isOn = false;

        void Start()
        {
            if (mwAnimator == null)
                mwAnimator = gameObject.GetComponent<Animator>();

            if (confirmButton != null)
                confirmButton.onClick.AddListener(onConfirm.Invoke);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(onCancel.Invoke);

            if (useCustomValues == false)
                UpdateUI();

            gameObject.SetActive(false);
        }

        public void UpdateUI()
        {
            try
            {
                iconImage.sprite = windowIcon;
                windowTitle.text = titleText;
                windowDescription.text = descriptionText;
            }

            catch
            {
                Debug.LogWarning("Modal Window - Cannot update the content due to missing variables.", this);
            }
        }

        public void OpenWindow()
        {       
            if (isOn == false)
            {
                StopCoroutine("DisableObject");
                gameObject.SetActive(true);
                mwAnimator.Play("Fade in");
                isOn = true;

                if (useBlur == true && blurManager != null)
                    blurManager.BlurInAnim();
            }
        }

        public void CloseWindow()
        {
            if (isOn == true)
            {
                mwAnimator.Play("Fade out");
                isOn = false;

                if (useBlur == true && blurManager != null)
                    blurManager.BlurOutAnim();

                StartCoroutine("DisableObject");
            }
        }

        public void AnimateWindow()
        {
            if (isOn == false)
            {
                StopCoroutine("DisableObject");
                gameObject.SetActive(true);
                mwAnimator.Play("Fade in");
                isOn = true;

                if (useBlur == true && blurManager != null)
                    blurManager.BlurInAnim();
            }

            else
            {
                mwAnimator.Play("Fade out");
                isOn = false;

                if (useBlur == true && blurManager != null)
                    blurManager.BlurOutAnim();

                StartCoroutine("DisableObject");
            }
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}