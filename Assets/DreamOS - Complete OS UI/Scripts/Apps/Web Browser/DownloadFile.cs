using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(Button))]
    public class DownloadFile : MonoBehaviour
    {
        [Header("Resources")]
        public Slider downloadBar;
        public TextMeshProUGUI downloadStatus;
        [HideInInspector] public float downloadMultiplier;
        [HideInInspector] public float fileSize;
        [HideInInspector] public bool enableNotification;

        [Header("Settings")]
        public string completeDescription = "Download completed";
        
        // Hidden variables
        Button objButton;
        Animator sliderAnimator;
        NotificationCreator downloadNotifier;
        WebBrowserManager wbm;
        bool updateDownloadBar = false;

        void Start()
        {
            // If download isn't completed and is in progress
            if (PlayerPrefs.GetInt("Downloaded" + gameObject.name) == 1)
            {
                // Get the necessary components and enable updating
                objButton = gameObject.GetComponent<Button>();
                sliderAnimator = downloadBar.gameObject.GetComponent<Animator>();
                downloadBar.value = 0;
                downloadBar.maxValue = fileSize;
                updateDownloadBar = true;

                try
                {
                    // Try getting web browser manager for deleting and updating the speed
                    wbm = (WebBrowserManager)GameObject.FindObjectsOfType(typeof(WebBrowserManager))[0];

                    // Try getting notification creator if it's enabled
                    if (enableNotification == true)
                        downloadNotifier = GameObject.Find("Download Notifier").gameObject.GetComponent<NotificationCreator>();
                }

                catch { }
            }

            // If download is completed
            else
            {
                // Enable the object
                objButton = gameObject.GetComponent<Button>();
                objButton.interactable = true;
                Destroy(downloadStatus.gameObject);
                this.enabled = false;
            }
        }

        void Update()
        {
            // If updating is enabled and there's a connected network
            if (updateDownloadBar == true && PlayerPrefs.HasKey("ConnectedNetworkTitle") == true)
            {
                // If download speed is different than the connected network, update it
                if (wbm != null && downloadMultiplier != wbm.networkManager.networkItems[wbm.networkManager.currentNetworkIndex].networkSpeed)
                    downloadMultiplier = wbm.networkManager.networkItems[wbm.networkManager.currentNetworkIndex].networkSpeed;

                // Increase the visuals depending on download size
                downloadBar.value += Time.deltaTime * downloadMultiplier;
                downloadStatus.text = downloadBar.value.ToString("F1") + " MB / " + downloadBar.maxValue.ToString("F1") + " MB";

                // When the download is completed
                if (downloadBar.value == fileSize)
                {
                    objButton.interactable = true; // Make it interactable
                    sliderAnimator.Play("Fade Out"); // Fade-out progress bar
                    StartCoroutine("EndProcess"); // Start EndProcess events
                    updateDownloadBar = false; // Disable uddating
                    PlayerPrefs.SetInt("Downloaded" + gameObject.name, 2); // Change data

                    // Create a notification if it's enabled
                    if (enableNotification == true)
                    {
                        downloadNotifier.notificationTitle = gameObject.name;
                        downloadNotifier.popupDescription = completeDescription;
                        downloadNotifier.CreateOnlyPopup();
                    }
                }
            }
        }

        public void DeleteFile()
        {
            try
            {
                if (wbm == null)
                    wbm = (WebBrowserManager)GameObject.FindObjectsOfType(typeof(WebBrowserManager))[0];
               
                wbm.DeleteDownloadFile(gameObject.name);
            }

            catch { }
        }

        IEnumerator EndProcess()
        {
            // Delete some visual objects on complete
            yield return new WaitForSeconds(1);
            Destroy(downloadBar.gameObject);
            Destroy(downloadStatus.gameObject);
            this.enabled = false;
        }
    }
}