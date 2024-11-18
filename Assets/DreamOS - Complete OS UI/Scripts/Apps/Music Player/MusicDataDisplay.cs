using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/Apps/Music Player/Music Data Display")]
    public class MusicDataDisplay : MonoBehaviour
    {
        [Header("Resources")]
        public MusicPlayerManager mpManager;

        [Header("Settings")]
        public ObjectType objectType;

        // Hidden helper variables
        TextMeshProUGUI textObj;
        Image coverImageObj;
        Slider sliderObj;
        Button btnObj;
        Animator animatorObj;
        bool firstTime = true;

        public enum ObjectType
        {
            TITLE,
            ARTIST,
            ALBUM,
            COVER,
            CURRENT_TIME,
            DURATION,
            MUSIC_SLIDER,
            PLAY_BUTTON,
            PAUSE_BUTTON,
            NEXT_BUTTON,
            PREV_BUTTON,
            REPEAT,
            SHUFFLE,
            VOLUME_SLIDER
        }

        void Start()
        {
            InitalizeTags();
            firstTime = false;
        }

        void OnEnable()
        {
            if (firstTime == true)
                return;

            // Check some variables and process the events for animators
            if (objectType == ObjectType.SHUFFLE && mpManager.shuffle == true)
                animatorObj.Play("Shuffle On");
            else if (objectType == ObjectType.SHUFFLE && mpManager.shuffle == false)
                animatorObj.Play("Shuffle Off");

            if (objectType == ObjectType.REPEAT && mpManager.repeat == true)
                animatorObj.Play("Repeat On");
            else if (objectType == ObjectType.REPEAT && mpManager.repeat == false)
                animatorObj.Play("Repeat Off");
        }

        public void InitalizeTags()
        {
            try
            {
                // If music player manager is not assigned, then try to find it
                if (mpManager == null)
                    mpManager = GameObject.Find("Music Player").GetComponent<MusicPlayerManager>();
            }

            // Give a log when unsucessful
            catch { Debug.LogWarning("<b>[Music Data Display]</b> Music Player Manager is not assigned.", this); }

            // Don't go further if music player manager is not assigned
            if (mpManager == null)
                return;

            // Get and change the value depending on the object type
            if (objectType == ObjectType.TITLE)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
            
            else if (objectType == ObjectType.ARTIST)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
            
            else if (objectType == ObjectType.ALBUM)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
            
            else if (objectType == ObjectType.COVER)
                coverImageObj = gameObject.GetComponent<Image>();
           
            else if (objectType == ObjectType.CURRENT_TIME)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();
          
            else if (objectType == ObjectType.DURATION)
                textObj = gameObject.GetComponent<TextMeshProUGUI>();

            else if (objectType == ObjectType.MUSIC_SLIDER)
            {
                sliderObj = gameObject.GetComponent<Slider>();
                sliderObj.onValueChanged.AddListener(delegate { MoveSlider(); });
            }

            else if (objectType == ObjectType.PLAY_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponentInParent<Animator>();
                btnObj.onClick.AddListener(mpManager.PlayMusic);
            }

            else if (objectType == ObjectType.PAUSE_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponentInParent<Animator>();
                btnObj.onClick.AddListener(mpManager.PauseMusic);
            }

            else if (objectType == ObjectType.NEXT_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(mpManager.NextTitle);
                btnObj.onClick.AddListener(ResetSlider);
                btnObj.onClick.AddListener(Next);
            }

            else if (objectType == ObjectType.PREV_BUTTON)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(mpManager.PrevTitle);
                btnObj.onClick.AddListener(ResetSlider);
                btnObj.onClick.AddListener(Prev);
            }

            else if (objectType == ObjectType.REPEAT)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(Repeat);

                if (PlayerPrefs.GetString("MusicPlayerRepeat") == "")
                    mpManager.repeat = true;

                if (PlayerPrefs.GetString("MusicPlayerRepeat") == "On")
                    mpManager.repeat = true;
                else
                    mpManager.repeat = false;

                if (mpManager.repeat == true)
                    animatorObj.Play("Repeat On");
                else
                    animatorObj.Play("Repeat Off");
            }

            else if (objectType == ObjectType.SHUFFLE)
            {
                btnObj = gameObject.GetComponent<Button>();
                animatorObj = gameObject.GetComponent<Animator>();
                btnObj.onClick.AddListener(Shuffle);

                if (PlayerPrefs.GetString("MusicPlayerShuffle") == "")
                    mpManager.shuffle = true;

                if (PlayerPrefs.GetString("MusicPlayerShuffle") == "On")
                    mpManager.shuffle = true;
                else
                    mpManager.shuffle = false;

                if (mpManager.shuffle == true)
                    animatorObj.Play("Shuffle On");
                else
                    animatorObj.Play("Shuffle Off");
            }

            else if (objectType == ObjectType.VOLUME_SLIDER)
            {
                sliderObj = gameObject.GetComponent<Slider>();
                sliderObj.onValueChanged.AddListener(SetVolume);

                if (PlayerPrefs.GetString("MusicPlayerVolumeFirst") == "")
                {
                    sliderObj.value = 1;
                    PlayerPrefs.SetString("MusicPlayerVolumeFirst", "initalized");
                    PlayerPrefs.SetFloat("MusicPlayerVolume", sliderObj.value);
                }

                sliderObj.value = PlayerPrefs.GetFloat("MusicPlayerVolume");

                if (mpManager.source != null)
                    mpManager.source.volume = sliderObj.value;
            }
        }

        void LateUpdate()
        {
            UpdateValues();
        }

        public void UpdateValues()
        {
            if (mpManager == null)
                return;

            // Change the value depending on the object type
            if (objectType == ObjectType.TITLE)
                textObj.text = mpManager.currentPlaylist.playlist[mpManager.currentTrack].musicTitle;

            else if (objectType == ObjectType.ARTIST)
                textObj.text = mpManager.currentPlaylist.playlist[mpManager.currentTrack].artistTitle;

            else if (objectType == ObjectType.ALBUM)
                textObj.text = mpManager.currentPlaylist.playlist[mpManager.currentTrack].albumTitle;

            else if (objectType == ObjectType.COVER)
                coverImageObj.sprite = mpManager.currentPlaylist.playlist[mpManager.currentTrack].musicCover;

            else if (objectType == ObjectType.CURRENT_TIME)
                textObj.text = mpManager.minutes + ":" + mpManager.seconds.ToString("D2");

            else if (objectType == ObjectType.DURATION)
            {
                mpManager.ShowCurrentTitle();
                textObj.text = ((mpManager.duration / 60) % 60) + ":" + (mpManager.duration % 60).ToString("D2");
            }

            else if (objectType == ObjectType.MUSIC_SLIDER)
            {
                sliderObj.maxValue = mpManager.source.clip.length;
                sliderObj.value = mpManager.source.time;
            }

            else if (objectType == ObjectType.PLAY_BUTTON || objectType == ObjectType.PAUSE_BUTTON)
            {
                if (mpManager.source.isPlaying == true)
                    animatorObj.Play("Pause In");
                else
                    animatorObj.Play("Play In");
            }
        }

        public void UpdateManually()
        {
            if (mpManager == null)
                return;

            if (objectType == ObjectType.SHUFFLE)
            {
                if (mpManager.shuffle == true)
                    animatorObj.Play("Shuffle Off");
                else
                    animatorObj.Play("Shuffle On");
            }

            else if (objectType == ObjectType.REPEAT)
            {
                if (mpManager.repeat == true)
                    animatorObj.Play("Repeat Off");
                else
                    animatorObj.Play("Repeat On");
            }

            else if (objectType == ObjectType.VOLUME_SLIDER && sliderObj != null)
                sliderObj.value = PlayerPrefs.GetFloat("MusicPlayerVolume");
        }

        public void ResetSlider()
        {
            // Reset duration slider
            mpManager.source.time = 0;
        }

        public void MoveSlider()
        {
            // Change the duration if slider value is valid for the current music
            try { mpManager.source.time = sliderObj.value; }
            catch { }
        }

        public void SetVolume(float volume)
        {
            // Get music player manager if it's not assigned
            if (mpManager.source == null)
                mpManager.source = mpManager.gameObject.GetComponent<AudioSource>();

            // Set the volume depending on slider value and save the data
            mpManager.source.volume = sliderObj.value;
            PlayerPrefs.SetFloat("MusicPlayerVolume", sliderObj.value);
        }

        public void Prev()
        {
            // Animate previous button
            animatorObj.Play("Animate");
        }

        public void Next()
        {
            // Animate next button
            animatorObj.Play("Animate");
        }

        public void Shuffle()
        {
            // If shuffle is enabled, play the animation and save the data
            if (mpManager.shuffle == true)
            {
                mpManager.shuffle = false;
                animatorObj.Play("Shuffle Off");
                PlayerPrefs.SetString("MusicPlayerShuffle", "Off");
            }

            else
            {
                mpManager.shuffle = true;
                animatorObj.Play("Shuffle On");
                PlayerPrefs.SetString("MusicPlayerShuffle", "On");
            }
        }

        public void Repeat()
        {
            // If repeat is enabled, play the animation and save the data
            if (mpManager.repeat == true)
            {
                mpManager.repeat = false;
                animatorObj.Play("Repeat Off");
                PlayerPrefs.SetString("MusicPlayerRepeat", "Off");
            }

            else
            {
                mpManager.repeat = true;
                animatorObj.Play("Repeat On");
                PlayerPrefs.SetString("MusicPlayerRepeat", "On");
            }
        }
    }
}