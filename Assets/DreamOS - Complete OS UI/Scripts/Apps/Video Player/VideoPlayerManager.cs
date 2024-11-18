using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public class VideoPlayerManager : MonoBehaviour
    {
        // Resources
        public VideoPlayerLibrary libraryAsset;
        public Transform musicLibraryParent; // Migrating
        public GameObject musicLibraryButton; // Migrating
        public Transform videoLibraryParent;
        public GameObject videoLibraryButton;
        public GameObject videoPlayerWindow;
        public Animator videoControlsAnimator;
        public Animator miniPlayerAnimator;
        [HideInInspector] public VideoPlayer videoComponent;
        [HideInInspector] public VideoPlayerManager videoManager;

        // Settings
        public bool sortListByName = true;
        [Range(1, 15)] public float controlsOutTime = 2.5f;
        [Range(1, 60)] public float seekTime = 10;
        public string videoPanelName = "Now Playing";

        // Hidden variables
        [HideInInspector] public int currentClipIndex;
        [HideInInspector] public int secondsPassed;
        [HideInInspector] public int minutesPassed;
        [HideInInspector] public int totalSeconds;
        [HideInInspector] public int totalMinutes;
        [HideInInspector] public bool loop;

        [HideInInspector] public bool isDone;
        [HideInInspector] public bool miniPlayerEnabled = false;
        [HideInInspector] public WindowPanelManager wManager;
        bool updateInputPos;
        bool controlHelper;
        int indexHelper;
        Vector3 lastMousePos;

        public bool IsPlaying
        {
            get { return videoComponent.isPlaying; }
        }

        public bool IsLooping
        {
            get { return videoComponent.isLooping; }
        }

        public bool IsPrepared
        {
            get { return videoComponent.isPrepared; }
        }

        public bool IsDone
        {
            get { return isDone; }
        }

        public double Time
        {
            get { return videoComponent.time; }
        }

        public ulong Duration
        {
            get { return (ulong)(videoComponent.frameCount / videoComponent.frameRate); }
        }

        public double NTime
        {
            get { return Time / Duration; }
        }

        void Start()
        {
            // Variable swapping for the old version
            if (videoLibraryParent == null)
                videoLibraryParent = musicLibraryParent;

            if (videoLibraryButton == null)
                videoLibraryButton = musicLibraryButton;

            PrepareVideoPlayer();

            if (miniPlayerAnimator != null)
                miniPlayerAnimator.gameObject.SetActive(false);
        }

        private static int SortByName(VideoPlayerLibrary.VideoItem o1, VideoPlayerLibrary.VideoItem o2)
        {
            return o1.videoTitle.CompareTo(o2.videoTitle);
        }

        public void PrepareVideoPlayer()
        {
            videoComponent = gameObject.GetComponent<VideoPlayer>();
            videoManager = gameObject.GetComponent<VideoPlayerManager>();
            videoComponent.SetTargetAudioSource(0, gameObject.GetComponent<AudioSource>());
            wManager = videoPlayerWindow.GetComponent<WindowPanelManager>();

            foreach (Transform child in videoLibraryParent)
                Destroy(child.gameObject);

            if (sortListByName == true)
                libraryAsset.videos.Sort(SortByName);

            for (int i = 0; i < libraryAsset.videos.Count; ++i)
            {
                if (libraryAsset.videos[i].excludeFromLibrary == false)
                {
                    // Spawn standard notification to the requested parent
                    GameObject go = Instantiate(videoLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(videoLibraryParent, false);
                    go.gameObject.name = libraryAsset.videos[i].videoTitle;

                    // Set cover image
                    Transform coverGO;
                    coverGO = go.transform.Find("Cover Mask/Cover/Image").GetComponent<Transform>();
                    coverGO.GetComponent<Image>().sprite = libraryAsset.videos[i].videoCover;

                    // Set ID tags
                    TextMeshProUGUI videoTitle;
                    videoTitle = go.transform.Find("Video Title").GetComponent<TextMeshProUGUI>();
                    videoTitle.text = libraryAsset.videos[i].videoTitle;
                    TextMeshProUGUI descText;
                    descText = go.transform.Find("Video Description").GetComponent<TextMeshProUGUI>();
                    descText.text = libraryAsset.videos[i].videoDescription;
                    TextMeshProUGUI durationText;
                    durationText = go.transform.Find("Duration Area/Duration").GetComponent<TextMeshProUGUI>();

                    if (libraryAsset.videos[i].playFromURL == false)
                        durationText.text = (((int)libraryAsset.videos[i].videoClip.length / 60) % 60) + ":" + ((int)libraryAsset.videos[i].videoClip.length % 60).ToString("D2");
                    else
                        durationText.text = "URL";

                    // Add button events
                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(delegate
                    {
                        if (libraryAsset.videos[go.transform.GetSiblingIndex()].playFromURL == false)
                        {
                            videoComponent.source = VideoSource.VideoClip;
                            PlayCustomVideo(go.transform.GetSiblingIndex());
                        }

                        else
                        {
                            videoComponent.source = VideoSource.Url;
                            indexHelper = go.transform.GetSiblingIndex();
                            PlayVideoURL(libraryAsset.videos[go.transform.GetSiblingIndex()].videoURL);
                        }

                        DisableMiniPlayer();
                        wManager.OpenPanel(videoPanelName);
                    });
                }

                else if (libraryAsset.videos[i].excludeFromLibrary == true)
                {
                    libraryAsset.videos.RemoveAt(i);

                    if (sortListByName == true)
                        i--;
                }
            }

            videoComponent.Play();
            videoComponent.Pause();
        }

        void Update()
        {
            if (videoManager.IsPrepared)
            {
                totalMinutes = (int)videoManager.Duration / 60;
                totalSeconds = (int)videoManager.Duration - totalMinutes * 60;
                minutesPassed = (int)videoManager.Time / 60;
                secondsPassed = (int)videoManager.Time - minutesPassed * 60;
            }

            if (updateInputPos == true)
            {
                if (lastMousePos == Input.mousePosition
                    && videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In"))
                {
                    StartCoroutine("ControlsFadeOutStart");
                }

                else if (lastMousePos != Input.mousePosition
                    && !videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In")
                    && controlHelper == true)
                {
                    StopCoroutine("ControlsFadeOutStart");
                    Cursor.visible = true;
                    videoControlsAnimator.CrossFade("In", 0.2f);
                    controlHelper = false;
                }

                else if (lastMousePos != Input.mousePosition)
                    StopCoroutine("ControlsFadeOutStart");

                lastMousePos = Input.mousePosition;
            }
        }

        public void PlayCustomVideo(int videoIndex)
        {
            videoComponent.Stop();
            currentClipIndex = videoIndex;
            videoComponent.clip = libraryAsset.videos[videoIndex].videoClip;
            videoComponent.time = 0;
            videoComponent.Play();
        }

        public void PlayVideoURL(string url)
        {
            videoComponent.Stop();
            currentClipIndex = indexHelper;
            videoComponent.url = libraryAsset.videos[indexHelper].videoURL;
            videoComponent.time = 0;
            videoComponent.Play();
        }

        public void PlayVideoClip(VideoClip video, string title)
        {
            for (int i = 0; i < libraryAsset.videos.Count; ++i)
            {
                if (libraryAsset.videos[i].videoTitle == title)
                {
                    libraryAsset.videos.RemoveAt(i);
                    currentClipIndex = i;
                    break;
                }
            }

            VideoPlayerLibrary.VideoItem item = new VideoPlayerLibrary.VideoItem();
            item.videoClip = video;
            item.videoTitle = title;
            item.excludeFromLibrary = true;
            libraryAsset.videos.Add(item);
            videoComponent.source = VideoSource.VideoClip;
            PlayCustomVideo(currentClipIndex);
        }

        public void Play()
        {
            this.enabled = true;
            videoComponent.Play();
        }

        public void Pause()
        {
            videoComponent.Pause();
            StopCoroutine("ControlsFadeOutStart");
        }

        public void SeekForward()
        {
            videoComponent.time += seekTime;
        }

        public void SeekBackward()
        {
            videoComponent.time -= seekTime;
        }

        public void IncreasePlaybackSpeed()
        {
            if (!videoComponent.canSetPlaybackSpeed)
                return;

            videoComponent.playbackSpeed += 1;
            videoComponent.playbackSpeed = Mathf.Clamp(videoComponent.playbackSpeed, 0, 10);
        }

        public void DecreasePlaybackSpeed()
        {
            if (!videoComponent.canSetPlaybackSpeed)
                return;

            videoComponent.playbackSpeed -= 1;
            videoComponent.playbackSpeed = Mathf.Clamp(videoComponent.playbackSpeed, 0, 10);
        }

        public void DisableControls()
        {
            StopCoroutine("ControlsFadeOutStart");
            updateInputPos = false;
            videoControlsAnimator.CrossFade("Out", 0.2f);
            Cursor.visible = true;
        }

        public void DisableInputUpdating()
        {
            updateInputPos = false;
            Cursor.visible = true;
        }

        public void EnableInputUpdating()
        {
            updateInputPos = true;
        }

        public void ControlOutOfBounds()
        {
            Cursor.visible = true;
        }

        public void EnableMiniPlayer()
        {
            if (miniPlayerAnimator != null && videoComponent.isPlaying == true)
            {
                StopCoroutine("MiniPlayerOut");
                miniPlayerEnabled = true;
                miniPlayerAnimator.gameObject.SetActive(true);
                miniPlayerAnimator.Play("In");
            }
        }

        public void DisableMiniPlayer()
        {
            if (miniPlayerAnimator != null && miniPlayerAnimator.gameObject.activeSelf == true)
            {
                miniPlayerAnimator.Play("Out");
                miniPlayerEnabled = false;
                StartCoroutine("MiniPlayerOut");
            }
        }

        IEnumerator ControlsFadeOutStart()
        {
            yield return new WaitForSeconds(controlsOutTime);
            Cursor.visible = false;
            videoControlsAnimator.CrossFade("Out", 0.2f);
            controlHelper = true;
            StopCoroutine("ControlsFadeOutStart");
        }

        IEnumerator MiniPlayerOut()
        {
            yield return new WaitForSeconds(0.5f);
            miniPlayerAnimator.gameObject.SetActive(false);
        }
    }
}