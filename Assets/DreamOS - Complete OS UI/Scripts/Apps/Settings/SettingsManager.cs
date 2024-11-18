using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class SettingsManager : MonoBehaviour
    {
        // Resources
        public ThemeManager themeManager;
        public RebootManager rebootManager;
        public UserManager userManager;
        public SetupManager setupManager;
        public Image wallpaperImage;
        public Image wallpaperPreview;
        public Image lockscreenImage;
        public GridLayoutGroup desktopList;
        public Transform accentColorList;
        public Transform accentReversedColorList;

        // Settings
        public Sprite defaultWallpaper;
        public bool lockDesktopItems = true;

        // Debug
        Sprite currentWallpaper;
        Toggle toggleHelper;

        void Start()
        {
            // If there's a data for desktop wallpaper
            if (PlayerPrefs.HasKey("DesktopWallpaper") == true)
            {
                // Get the wallpaper and set for the desktop
                wallpaperImage.sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("DesktopWallpaper"));
                wallpaperPreview.sprite = wallpaperImage.sprite;
                currentWallpaper = wallpaperImage.sprite;

                if (lockscreenImage != null)
                    lockscreenImage.sprite = wallpaperImage.sprite;
            }

            // If not, set default desktop wallpaper
            else
            {
                wallpaperImage.sprite = defaultWallpaper;
                wallpaperPreview.sprite = wallpaperImage.sprite;
                currentWallpaper = wallpaperImage.sprite;

                if (lockscreenImage != null)
                    lockscreenImage.sprite = wallpaperImage.sprite;
            }

            // Set selected theme as custom and get the color data
            if (PlayerPrefs.GetString("UseCustomColors") == "true")
            {
                ChangeAccentColor(PlayerPrefs.GetString("AccentColor"));
                ChangeAccentReversedColor(PlayerPrefs.GetString("AccentReversedColor"));
                CheckForToggles();
            }
        }

        public void SetWallpaper(Image newBackground)
        {
            // Set Wallpaper
            wallpaperImage.sprite = newBackground.sprite;
            wallpaperPreview.sprite = wallpaperImage.sprite;
            currentWallpaper = wallpaperImage.sprite;

            if (lockscreenImage != null)
                lockscreenImage.sprite = wallpaperImage.sprite;

            // Save data so we can fetch it later
            PlayerPrefs.SetString("DesktopWallpaper", "Wallpapers\\" + newBackground.sprite.name);
        }

        public void SetWallpaperDirectly(Sprite newBackground)
        {
            // Set Wallpaper
            wallpaperImage.sprite = newBackground;
            wallpaperPreview.sprite = wallpaperImage.sprite;
            currentWallpaper = wallpaperImage.sprite;

            if (lockscreenImage != null)
                lockscreenImage.sprite = wallpaperImage.sprite;

            // Save data so we can fetch it later
            PlayerPrefs.SetString("DesktopWallpaper", "Wallpapers\\" + newBackground.name);
        }

        public void LockDesktopItem(bool locked)
        {
            // WIP
            if (locked == true)
                desktopList.enabled = true;
            else
                desktopList.enabled = false;
        }

        public void CheckForToggles()
        {
            // Invoke color toggle depending on the data
            foreach (Transform obj in accentColorList)
            {
                if (obj.name == PlayerPrefs.GetString("AccentColor"))
                {
                    toggleHelper = obj.GetComponent<Toggle>();
                    toggleHelper.isOn = true;
                    toggleHelper.onValueChanged.Invoke(true);
                }
            }

            foreach (Transform obj in accentReversedColorList)
            {
                if (obj.name == PlayerPrefs.GetString("AccentReversedColor"))
                {
                    toggleHelper = obj.GetComponent<Toggle>();
                    toggleHelper.isOn = true;
                    toggleHelper.onValueChanged.Invoke(true);
                }
            }
        }

        public void SelectSystemTheme()
        {
            themeManager.selectedTheme = ThemeManager.SelectedTheme.SYSTEM;
            PlayerPrefs.SetString("UseCustomColors", "false");
        }

        public void SelectCustomTheme()
        {
            themeManager.selectedTheme = ThemeManager.SelectedTheme.CUSTOM;
            PlayerPrefs.SetString("UseCustomColors", "true");
        }

        public void ChangeAccentColor(string colorCode)
        {
            // Change color depending on the color code
            Color colorHelper;
            ColorUtility.TryParseHtmlString("#" + colorCode, out colorHelper);
            themeManager.highlightedColorCustom = new Color(colorHelper.r, colorHelper.g, colorHelper.b, themeManager.highlightedColorCustom.a);
            PlayerPrefs.SetString("AccentColor", colorCode);
        }

        public void ChangeAccentReversedColor(string colorCodeReversed)
        {
            // Change color depending on the color code
            Color colorHelper;
            ColorUtility.TryParseHtmlString("#" + colorCodeReversed, out colorHelper);
            themeManager.highlightedColorSecondaryCustom = new Color(colorHelper.r, colorHelper.g, colorHelper.b, themeManager.highlightedColorSecondaryCustom.a);
            PlayerPrefs.SetString("AccentReversed Color", colorCodeReversed);
        }

        public void WipeUserData()
        {
            // Delete user data
            DeleteUserData();
            rebootManager.WipeSystem();
            StartCoroutine("WaitForReboot");
        }

        public void WipeEverything()
        {
            // Delete EVERYTHING! Use with caution
            PlayerPrefs.DeleteAll();
        }

        IEnumerator WaitForReboot()
        {
            yield return new WaitForSeconds(rebootManager.waitTime);
            SelectSystemTheme();
            userManager.InitializeUserManager();
            userManager.lockScreen.Play("Lock Screen Out");
            userManager.desktopScreen.Play("Desktop Out");
            setupManager.PanelAnim(0);
            setupManager.currentBGAnimator = setupManager.steps[0].background.GetComponent<Animator>();
            setupManager.currentPanelAnimator = setupManager.steps[0].panel.GetComponent<Animator>();
            setupManager.currentBGAnimator.Play("Panel In");
            setupManager.currentPanelAnimator.Play("Panel In");
            StopCoroutine("WaitForWipe");
        }

        public void DeleteUserData()
        {
            // User data
            PlayerPrefs.DeleteKey("FirstName");
            PlayerPrefs.DeleteKey("LastName");
            PlayerPrefs.DeleteKey("Password");
            PlayerPrefs.DeleteKey("SecurityQuestionHint");
            PlayerPrefs.DeleteKey("SecurityQuestion");
            PlayerPrefs.DeleteKey("UserCreated");
            PlayerPrefs.DeleteKey("ProfilePicture");

            // Reminder data
            PlayerPrefs.DeleteKey("Reminder1Enabled");
            PlayerPrefs.DeleteKey("Reminder2Enabled");
            PlayerPrefs.DeleteKey("Reminder3Enabled");
            PlayerPrefs.DeleteKey("Reminder4Enabled");
            PlayerPrefs.DeleteKey("Reminder1Title");
            PlayerPrefs.DeleteKey("Reminder1Hour");
            PlayerPrefs.DeleteKey("Reminder1Minute");
            PlayerPrefs.DeleteKey("Reminder1Type");
            PlayerPrefs.DeleteKey("Reminder2Title");
            PlayerPrefs.DeleteKey("Reminder2Hour");
            PlayerPrefs.DeleteKey("Reminder2Minute");
            PlayerPrefs.DeleteKey("Reminder2Type");
            PlayerPrefs.DeleteKey("Reminder3Title");
            PlayerPrefs.DeleteKey("Reminder3Hour");
            PlayerPrefs.DeleteKey("Reminder3Minute");
            PlayerPrefs.DeleteKey("Reminder3Type");
            PlayerPrefs.DeleteKey("Reminder4Title");
            PlayerPrefs.DeleteKey("Reminder4Hour");
            PlayerPrefs.DeleteKey("Reminder4Minute");
            PlayerPrefs.DeleteKey("Reminder4Type");

            // Network data
            PlayerPrefs.DeleteKey("ConnectedNetworkTitle");
            PlayerPrefs.DeleteKey("CurrentNetworkIndex");
        }
    }
}