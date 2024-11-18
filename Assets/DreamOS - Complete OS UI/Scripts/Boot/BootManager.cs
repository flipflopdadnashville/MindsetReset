using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Michsky.DreamOS
{
    public class BootManager : MonoBehaviour
    {
        // Events
        public UnityEvent onBootStart;
        public UnityEvent eventsAfterBoot;

        // Resources
        public Animator bootAnimator;
        public TextMeshProUGUI bootingTextObject;

        // Settings
        public float bootTime = 3f;
        public string bootingText = "Booting Up";

        void Start()
        {
            StartCoroutine("BootEventStart");
        }

        public void InvokeEvents()
        {
            bootAnimator.gameObject.SetActive(true);
            bootAnimator.Play("Boot Start");
            StartCoroutine("BootEventStart");
        }

        public void UpdateUI()
        {
            if (bootingTextObject != null)
                bootingTextObject.text = bootingText;
        }

        IEnumerator BootEventStart()
        {
            yield return new WaitForSeconds(bootTime);

            if (bootAnimator.gameObject.activeSelf == true)
                bootAnimator.Play("Boot Out");

            eventsAfterBoot.Invoke();
            StopCoroutine("BootEventStart");
        }
    }
}