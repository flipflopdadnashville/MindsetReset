using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class SliderManager : MonoBehaviour
    {
        [Header("Resources")]
        public Slider mainSlider;
        public TextMeshProUGUI valueText;

        [Header("Saving")]
        public bool enableSaving = false;
        public string sliderTag = "Tag Text";

        [Header("Settings")]
        public bool usePercent = false;
        public bool showValue = true;
        public bool useRoundValue = false;
        public float valueMultiplier = 1;

        // Other Variables
        [HideInInspector] public float saveValue;

        void Start()
        {
            try
            {
                if (enableSaving == true)
                {
                    if (PlayerPrefs.HasKey(sliderTag + "SliderValue") == false)
                        saveValue = mainSlider.value;
                    else
                        saveValue = PlayerPrefs.GetFloat(sliderTag + "SliderValue");

                    mainSlider.value = saveValue;

                    mainSlider.onValueChanged.AddListener(delegate
                    {
                        saveValue = mainSlider.value;
                        PlayerPrefs.SetFloat(sliderTag + "SliderValue", saveValue);
                    });
                }
            }

            catch
            {
                Debug.LogError("Slider - Cannot initalize the object due to missing components.", this);
            }
        }

        void Update()
        {
            if (useRoundValue == true)
            {
                if (valueText != null && usePercent == true)
                    valueText.text = Mathf.Round(mainSlider.value * valueMultiplier).ToString() + "%";

                else if (valueText != null && usePercent == false)
                    valueText.text = Mathf.Round(mainSlider.value * valueMultiplier).ToString();
            }

            else
            {
                if (valueText != null && usePercent == true)
                    valueText.text = mainSlider.value.ToString("F1") + "%";

                else if (valueText != null && usePercent == false)
                    valueText.text = mainSlider.value.ToString("F1");
            }
        }
    }
}