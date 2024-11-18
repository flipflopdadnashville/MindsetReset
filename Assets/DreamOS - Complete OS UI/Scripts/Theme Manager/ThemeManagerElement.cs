using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [ExecuteInEditMode]
    public class ThemeManagerElement : MonoBehaviour
    {
        [Header("Settings")]
        public ThemeManager themeManagerAsset;
        public ObjectType objectType;
        public ColorType colorType = ColorType.PRIMARY;
        public FontType fontType = FontType.REGULAR;
        public bool keepAlphaValue = false;
        public bool useCustomFont = false;

        Image imageObject;
        TextMeshProUGUI textObject;

        public enum ObjectType
        {
            TEXT,
            IMAGE
        }

        public enum ColorType
        {
            WINDOW_BG,
            BACKGROUND,
            PRIMARY,
            SECONDARY,
            ACCENT,
            ACCENT_REVERSED,
            TASK_BAR
        }

        public enum FontType
        {
            THIN,
            LIGHT,
            REGULAR,
            SEMIBOLD,
            BOLD
        }

        void Awake()
        {
            try
            {
                if (themeManagerAsset == null)
                    themeManagerAsset = Resources.Load<ThemeManager>("Theme/Theme Manager");

                if (objectType == ObjectType.IMAGE && imageObject == null)
                    imageObject = gameObject.GetComponent<Image>();
                else if (objectType == ObjectType.TEXT && textObject == null)
                    textObject = gameObject.GetComponent<TextMeshProUGUI>();

                this.enabled = true;

                if (themeManagerAsset.enableDynamicUpdate == false)
                {
                    UpdateElement();
                    this.enabled = false;
                }
            }

            catch
            {
                Debug.LogWarning("No <b>Theme Manager</b> variable found. Assign it manually, otherwise you might get errors about it.", this);
            }
        }

        void LateUpdate()
        {
            if (themeManagerAsset == null)
                return;

            if (themeManagerAsset.enableDynamicUpdate == true)
                UpdateElement();
        }

        void UpdateElement()
        {
            try
            {
                if (objectType == ObjectType.IMAGE && imageObject != null)
                {
                    if (keepAlphaValue == false)
                    {
                        if (colorType == ColorType.WINDOW_BG)
                            imageObject.color = themeManagerAsset.windowBGColorDark;
                        else if (colorType == ColorType.BACKGROUND)
                            imageObject.color = themeManagerAsset.backgroundColorDark;
                        else if (colorType == ColorType.PRIMARY)
                            imageObject.color = themeManagerAsset.primaryColorDark;
                        else if (colorType == ColorType.SECONDARY)
                            imageObject.color = themeManagerAsset.secondaryColorDark;
                        else if (colorType == ColorType.TASK_BAR)
                            imageObject.color = themeManagerAsset.taskBarColorDark;

                        if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.SYSTEM)
                        {
                            if (colorType == ColorType.ACCENT)
                                imageObject.color = themeManagerAsset.highlightedColorDark;
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                imageObject.color = themeManagerAsset.highlightedColorSecondaryDark;
                        }

                        else if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.CUSTOM)
                        {
                            if (colorType == ColorType.ACCENT)
                                imageObject.color = themeManagerAsset.highlightedColorCustom;
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                imageObject.color = themeManagerAsset.highlightedColorSecondaryCustom;
                        }
                    }

                    else
                    {
                        if (colorType == ColorType.WINDOW_BG)
                            imageObject.color = new Color(themeManagerAsset.windowBGColorDark.r, themeManagerAsset.windowBGColorDark.g, themeManagerAsset.windowBGColorDark.b, imageObject.color.a);
                        else if (colorType == ColorType.BACKGROUND)
                            imageObject.color = new Color(themeManagerAsset.backgroundColorDark.r, themeManagerAsset.backgroundColorDark.g, themeManagerAsset.backgroundColorDark.b, imageObject.color.a);
                        else if (colorType == ColorType.PRIMARY)
                            imageObject.color = new Color(themeManagerAsset.primaryColorDark.r, themeManagerAsset.primaryColorDark.g, themeManagerAsset.primaryColorDark.b, imageObject.color.a);
                        else if (colorType == ColorType.SECONDARY)
                            imageObject.color = new Color(themeManagerAsset.secondaryColorDark.r, themeManagerAsset.secondaryColorDark.g, themeManagerAsset.secondaryColorDark.b, imageObject.color.a);

                        if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.SYSTEM)
                        {
                            if (colorType == ColorType.ACCENT)
                                imageObject.color = new Color(themeManagerAsset.highlightedColorDark.r, themeManagerAsset.highlightedColorDark.g, themeManagerAsset.highlightedColorDark.b, imageObject.color.a);
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                imageObject.color = new Color(themeManagerAsset.highlightedColorSecondaryDark.r, themeManagerAsset.highlightedColorSecondaryDark.g, themeManagerAsset.highlightedColorSecondaryDark.b, imageObject.color.a);
                        }

                        else if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.CUSTOM)
                        {
                            if (colorType == ColorType.ACCENT)
                                imageObject.color = new Color(themeManagerAsset.highlightedColorCustom.r, themeManagerAsset.highlightedColorCustom.g, themeManagerAsset.highlightedColorCustom.b, imageObject.color.a);
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                imageObject.color = new Color(themeManagerAsset.highlightedColorSecondaryCustom.r, themeManagerAsset.highlightedColorSecondaryCustom.g, themeManagerAsset.highlightedColorSecondaryCustom.b, imageObject.color.a);
                        }
                    }
                }

                else if (objectType == ObjectType.TEXT && textObject != null)
                {
                    if (keepAlphaValue == false)
                    {
                        if (colorType == ColorType.WINDOW_BG)
                            textObject.color = themeManagerAsset.windowBGColorDark;
                        else if (colorType == ColorType.BACKGROUND)
                            textObject.color = themeManagerAsset.backgroundColorDark;
                        else if (colorType == ColorType.PRIMARY)
                            textObject.color = themeManagerAsset.primaryColorDark;
                        else if (colorType == ColorType.SECONDARY)
                            textObject.color = themeManagerAsset.secondaryColorDark;

                        if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.SYSTEM)
                        {
                            if (colorType == ColorType.ACCENT)
                                textObject.color = themeManagerAsset.highlightedColorDark;
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                textObject.color = themeManagerAsset.highlightedColorSecondaryDark;
                        }

                        else if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.CUSTOM)
                        {
                            if (colorType == ColorType.ACCENT)
                                textObject.color = themeManagerAsset.highlightedColorCustom;
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                textObject.color = themeManagerAsset.highlightedColorSecondaryCustom;
                        }
                    }

                    else
                    {
                        if (colorType == ColorType.WINDOW_BG)
                            textObject.color = new Color(themeManagerAsset.windowBGColorDark.r, themeManagerAsset.windowBGColorDark.g, themeManagerAsset.windowBGColorDark.b, textObject.color.a);
                        else if (colorType == ColorType.BACKGROUND)
                            textObject.color = new Color(themeManagerAsset.backgroundColorDark.r, themeManagerAsset.backgroundColorDark.g, themeManagerAsset.backgroundColorDark.b, textObject.color.a);
                        else if (colorType == ColorType.PRIMARY)
                            textObject.color = new Color(themeManagerAsset.primaryColorDark.r, themeManagerAsset.primaryColorDark.g, themeManagerAsset.primaryColorDark.b, textObject.color.a);
                        else if (colorType == ColorType.SECONDARY)
                            textObject.color = new Color(themeManagerAsset.secondaryColorDark.r, themeManagerAsset.secondaryColorDark.g, themeManagerAsset.secondaryColorDark.b, textObject.color.a);

                        if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.SYSTEM)
                        {
                            if (colorType == ColorType.ACCENT)
                                textObject.color = new Color(themeManagerAsset.highlightedColorDark.r, themeManagerAsset.highlightedColorDark.g, themeManagerAsset.highlightedColorDark.b, textObject.color.a);
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                textObject.color = new Color(themeManagerAsset.highlightedColorSecondaryDark.r, themeManagerAsset.highlightedColorSecondaryDark.g, themeManagerAsset.highlightedColorSecondaryDark.b, textObject.color.a);
                        }

                        else if (themeManagerAsset.selectedTheme == ThemeManager.SelectedTheme.CUSTOM)
                        {
                            if (colorType == ColorType.ACCENT)
                                textObject.color = new Color(themeManagerAsset.highlightedColorCustom.r, themeManagerAsset.highlightedColorCustom.g, themeManagerAsset.highlightedColorCustom.b, textObject.color.a);
                            else if (colorType == ColorType.ACCENT_REVERSED)
                                textObject.color = new Color(themeManagerAsset.highlightedColorSecondaryCustom.r, themeManagerAsset.highlightedColorSecondaryCustom.g, themeManagerAsset.highlightedColorSecondaryCustom.b, textObject.color.a);
                        }
                    }

                    if (useCustomFont != true)
                    {
                        if (fontType == FontType.THIN)
                            textObject.font = themeManagerAsset.systemFontThin;
                        else if (fontType == FontType.LIGHT)
                            textObject.font = themeManagerAsset.systemFontLight;
                        else if (fontType == FontType.REGULAR)
                            textObject.font = themeManagerAsset.systemFontRegular;
                        else if (fontType == FontType.SEMIBOLD)
                            textObject.font = themeManagerAsset.systemFontSemiBold;
                        else if (fontType == FontType.BOLD)
                            textObject.font = themeManagerAsset.systemFontBold;
                    }
                }
            }

            catch { }
        }
    }
}