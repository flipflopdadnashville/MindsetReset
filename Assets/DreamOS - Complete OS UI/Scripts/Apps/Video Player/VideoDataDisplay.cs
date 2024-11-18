using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/Apps/Video Player/Video Data Display")]
    public class VideoDataDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Resources")]
        public VideoPlayerManager vManager;

        [Header("Settings")]
        public ObjectType objectType;

        TextMeshProUGUI textObj;
        Image coverImageObj;
        Slider sliderObj;
        Button btnObj;
        Animator animatorObj;
        AudioSource aSource;
        bool enableSliderUpdate = true;

        // Object type list
        public enum ObjectType
        {
            TITLE,
            DESCRIPTION,
            COVER,
            CURRENT_TIME,
            DURATION,
            VIDEO_SLIDER,
            PLAY_BUTTON,
            PAUSE_BUTTON,
            SEEK_FORWARD,
            SEEK_BACKWARD,
            LOOP,
            VOLUME_SLIDER
        }

        void Start()
        {
            // If video manager is not assigned, try to find it
            if (vManager == null)
                vManager = GameObject.Find("Video Player").GetComponent<VideoPlayerManager>();

            // Get and change value depending on the object type
            if (objectType == ObjectType.TITLE)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
            
            else if (objectType == ObjectType.DESCRIPTION)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();

            else if (objectType == ObjectType.COVER)
                coverImageObj = gameObject.GetComponent<Image>();
            
            else if (objectType == ObjectType.CURRENT_TIME)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
            
            else if (objectType == ObjectType.DURATION)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
           
            else if (objectType == ObjectType.VIDEO_SLIDER)
                sliderObj = gameObject.GetComponent<Slider>();

            else if (objectType == ObjectType.PLAY_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponentInParent<Animator>();
                btnObj.onClick.AddListener(vManager.Play);
            }

            else if (objectType == ObjectType.PAUSE_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponentInParent<Animator>();
                btnObj.onClick.AddListener(vManager.Pause);
            }

            else if (objectType == ObjectType.SEEK_FORWARD)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(vManager.SeekForward);
            }

            else if (objectType == ObjectType.SEEK_BACKWARD)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(vManager.SeekBackward);
            }

            else if (objectType == ObjectType.LOOP)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(Loop);

                if (PlayerPrefs.GetString("Video Player Loop") == "On")
                    vManager.videoComponent.isLooping = true;
                else
                    vManager.videoComponent.isLooping = false;

                if (vManager.videoComponent.isLooping == true)
                    animatorObj.Play("Repeat On");
                else
                    animatorObj.Play("Repeat Off");
            }

            else if (objectType == ObjectType.VOLUME_SLIDER)
            {
                aSource = vManager.gameObject.GetComponent<AudioSource>();

                sliderObj = gameObject.GetComponent<Slider>();
                sliderObj.onValueChanged.AddListener(SetVolume);

                if (!PlayerPrefs.HasKey("Video Player Volume Default"))
                {
                    sliderObj.value = 1;
                    PlayerPrefs.SetString("Video Player Volume Default", "initalized");
                    PlayerPrefs.SetFloat("Video Player Volume", sliderObj.value);
                }

                sliderObj.value = PlayerPrefs.GetFloat("Video Player Volume");
                aSource.volume = sliderObj.value;
            }
        }

        void LateUpdate()
        {
            // Get and change value depending on the object type
            if (objectType == ObjectType.TITLE)
                textObj.text = vManager.libraryAsset.videos[vManager.currentClipIndex].videoTitle;

            else if (objectType == ObjectType.COVER)
                coverImageObj.sprite = vManager.libraryAsset.videos[vManager.currentClipIndex].videoCover;

            else if (objectType == ObjectType.CURRENT_TIME)
                textObj.text = string.Format("{0:00}:{1:00}", vManager.minutesPassed, vManager.secondsPassed);

            else if (objectType == ObjectType.DURATION)
                textObj.text = string.Format("{0:00}:{1:00}", vManager.totalMinutes, vManager.totalSeconds);

            else if (objectType == ObjectType.VIDEO_SLIDER)
            {
                if (vManager.videoComponent != null && enableSliderUpdate == true)
                {
                    sliderObj.maxValue = (float)vManager.videoComponent.length;
                    sliderObj.value = (float)vManager.videoComponent.time;            
                }

                else if (vManager.videoComponent != null && enableSliderUpdate == false)
                    MoveSlider();
            }

            else if (objectType == ObjectType.PLAY_BUTTON || objectType == ObjectType.PAUSE_BUTTON)
            {
                if (vManager.videoComponent.isPlaying == true)
                    animatorObj.Play("Pause In");
                else
                    animatorObj.Play("Play In");
            }
        }

        public void ResetSlider()
        {
            // Reset video duration slider and pause the video player
            vManager.videoComponent.time = 0;
            vManager.Pause();
        }

        public void MoveSlider()
        {
            // Change the duration if slider value is valid for the current video
            try
            {
                vManager.videoComponent.time = sliderObj.value;
            }

            catch { }
        }

        public void SetVolume(float volume)
        {
            // Set the volume depending on slider value and save the data
            aSource.volume = sliderObj.value;
            PlayerPrefs.SetFloat("Video Player Volume", sliderObj.value);
        }

        public void SeekAnim()
        {
            // Just play seek animation
            animatorObj.Play("Animate");
        }

        public void Loop()
        {
            // If loop is enabled, play the animation and save the data
            if (vManager.videoComponent.isLooping == true)
            {
                vManager.videoComponent.isLooping = false;
                animatorObj.Play("Repeat Off");
                PlayerPrefs.SetString("Video Player Loop", "Off");
            }

            else
            {
                vManager.videoComponent.isLooping = true;
                animatorObj.Play("Repeat On");
                PlayerPrefs.SetString("Video Player Loop", "On");
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (objectType == ObjectType.VIDEO_SLIDER)
                enableSliderUpdate = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (objectType == ObjectType.VIDEO_SLIDER)
                enableSliderUpdate = true;
        }
    }
}