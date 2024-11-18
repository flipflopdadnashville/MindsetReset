using UnityEngine;
using UnityEngine.Audio;

namespace Michsky.DreamOS
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Resources")]
        public AudioMixer mixer;
        public SliderManager masterSlider;

        void Start()
        {
            if (mixer != null && masterSlider != null)
                mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat(masterSlider.sliderTag + "SliderValue")) * 20);
        }

        public void VolumeSetMaster(float volume)
        {
            if (mixer == null)
                return;

            mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
    }
}