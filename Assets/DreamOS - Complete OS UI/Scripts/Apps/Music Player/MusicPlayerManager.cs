using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayerManager : MonoBehaviour
    {
        // Playlist
        public MusicPlayerPlaylist currentPlaylist;
        private MusicPlayerPlaylist playlistHelper;
        public List<MusicPlayerPlaylist> playlists = new List<MusicPlayerPlaylist>();
        private AudioClip tempClip;

        // Resources
        public Transform musicLibraryParent;
        public GameObject musicLibraryButton;
        public WindowPanelManager musicPanelManager;
        public TextMeshProUGUI nowPlayingListTitle;

        // Playlist Resources
        public Transform playlistParent;
        public Transform playlistContentParent;
        public GameObject playlistButton;
        public TextMeshProUGUI playlistTitle;
        public TextMeshProUGUI playlistDescription;
        public Image playlistCover;
        public Image playlistCoverBanner;
        public Button playlistPlayAllButton;

        // Settings
        public bool repeat;
        public bool shuffle;
        public bool sortListByName = true;

        // Notification
        public bool enablePopupNotification;
        public NotificationCreator notificationCreator;

        // Hidden song data variables
        [HideInInspector] public int duration;
        [HideInInspector] public int playTime;
        [HideInInspector] public int seconds;
        [HideInInspector] public int minutes;
        [HideInInspector] public int currentTrack;
        [HideInInspector] public AudioSource source;

        // Hidden helper variables
        bool allowSwitchColor;
        bool isReady = true;
        int randomHelper;
        int itemCount = 0;

        void Start()
        {
            // Let's goooooo
            PrepareMusicPlayer();
            PlayMusic();
            PauseMusic();
            this.enabled = false;
        }

        void OnEnable()
        {
            // Stop the music if the manager tries to play on enable
            try
            {
                StopMusic();
            }

            catch { }
        }

        private static int SortByName(MusicPlayerPlaylist.MusicItem o1, MusicPlayerPlaylist.MusicItem o2)
        {
            // Compare the names and sort by A to Z
            return o1.musicTitle.CompareTo(o2.musicTitle);
        }

        public void PrepareMusicPlayer()
        {
            // Destroy each object in music library parent
            foreach (Transform child in musicLibraryParent)
                Destroy(child.gameObject);

            // Destroy each object in playlist library parent
            foreach (Transform child in playlistParent)
                Destroy(child.gameObject);

            // Sort songs by A to Z
            if (sortListByName == true)
                currentPlaylist.playlist.Sort(SortByName);

            // Get the audio source
            source = GetComponent<AudioSource>();

            // Instantiate the entire playlist songs as buttons
            for (int i = 0; i < currentPlaylist.playlist.Count; ++i)
            {
                if (currentPlaylist.playlist[i].excludeFromLibrary == false)
                {
                    // Spawn music button
                    GameObject go = Instantiate(musicLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(musicLibraryParent, false);
                    go.gameObject.name = currentPlaylist.playlist[i].musicTitle;

                    // Set button BG color
                    Button buttonGO;
                    buttonGO = go.transform.GetComponent<Button>();
                    ColorBlock buttonColor;

                    // This is for visibility in library, qol
                    if (allowSwitchColor == false)
                    {
                        buttonColor = buttonGO.colors;
                        buttonColor.normalColor = new Color32(255, 255, 255, 10);
                        buttonGO.colors = buttonColor;
                        allowSwitchColor = true;
                    }

                    else
                    {
                        buttonColor = buttonGO.colors;
                        buttonColor.normalColor = new Color32(255, 255, 255, 0);
                        buttonGO.colors = buttonColor;
                        allowSwitchColor = false;
                    }

                    // Set cover image
                    Transform coverGO;
                    coverGO = go.transform.Find("Cover/Image").GetComponent<Transform>();
                    coverGO.GetComponent<Image>().sprite = currentPlaylist.playlist[i].musicCover;

                    // Set ID tags
                    TextMeshProUGUI songName;
                    songName = go.transform.Find("Song Title").GetComponent<TextMeshProUGUI>();
                    songName.text = currentPlaylist.playlist[i].musicTitle;
                    TextMeshProUGUI artistName;
                    artistName = go.transform.Find("Artist Name").GetComponent<TextMeshProUGUI>();
                    artistName.text = currentPlaylist.playlist[i].artistTitle;
                    TextMeshProUGUI durationText;
                    durationText = go.transform.Find("Duration").GetComponent<TextMeshProUGUI>();
                    durationText.text = (((int)currentPlaylist.playlist[i].musicClip.length / 60) % 60) + ":" + ((int)currentPlaylist.playlist[i].musicClip.length % 60).ToString("D2");

                    // Add button events
                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(delegate
                    {
                        currentPlaylist = playlists[0];
                        PlayCustomMusic(go.transform.GetSiblingIndex());
                        nowPlayingListTitle.text = playlists[0].playlistName;
                    });
                }

                else if(currentPlaylist.playlist[i].excludeFromLibrary == true)
                {
                    currentPlaylist.playlist.RemoveAt(i);

                    if (sortListByName == true)
                        i--;
                }
            }

            // Instantiate the entire playlists as buttons
            for (int i = 0; i < playlists.Count; ++i)
            {
                // Spawn playlist button
                GameObject go = Instantiate(playlistButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(playlistParent, false);
                go.gameObject.name = playlists[i].ToString();

                // Set cover image
                Transform coverGO;
                coverGO = go.transform.Find("Cover/Image").GetComponent<Transform>();
                coverGO.GetComponent<Image>().sprite = playlists[i].coverImage;

                // Set titles
                TextMeshProUGUI playlistButtonTitle;
                playlistButtonTitle = go.transform.Find("Playlist Title").GetComponent<TextMeshProUGUI>();
                playlistButtonTitle.text = playlists[i].playlistName;
                TextMeshProUGUI countTitle;
                countTitle = go.transform.Find("Music Count").GetComponent<TextMeshProUGUI>();
                countTitle.text = playlists[i].playlist.Count.ToString() + " Songs";

                // Add playlist button events
                Button listButton;
                listButton = go.GetComponent<Button>();
                listButton.onClick.AddListener(delegate
                {
                    itemCount = 0;
                    musicPanelManager.OpenPanel("Playlist Content");
                    PreparePlaylist(go.transform.GetSiblingIndex());
                    playlistTitle.text = playlistHelper.playlistName;

                    for (int x = 0; x < playlistHelper.playlist.Count; ++x)
                    {
                        if (currentPlaylist.playlist[x].excludeFromLibrary == false)
                            itemCount++;
                        else
                            currentPlaylist.playlist.RemoveAt(x);
                    }

                    playlistDescription.text = itemCount.ToString() + " Songs";
                    playlistCover.sprite = playlistHelper.coverImage;
                    playlistCoverBanner.sprite = playlistHelper.coverImage;
                });

                // Add play button events
                Button playButton;
                playButton = go.transform.Find("Buttons/Play").GetComponent<Button>();
                playButton.onClick.AddListener(delegate
                {
                    currentPlaylist = playlists[go.transform.GetSiblingIndex()];
                    currentTrack = 0;
                    PreparePlaylist(go.transform.GetSiblingIndex());
                    PlayCustomMusic(0);
                    PlayMusic();
                    nowPlayingListTitle.text = playlistHelper.playlistName;
                });

                // Add show button events
                Button showButton;
                showButton = go.transform.Find("Buttons/Show").GetComponent<Button>();
                showButton.onClick.AddListener(delegate
                {
                    musicPanelManager.OpenPanel("Playlist Content");
                    PreparePlaylist(go.transform.GetSiblingIndex());
                    playlistTitle.text = playlistHelper.playlistName;
                    playlistDescription.text = playlistHelper.playlist.Count.ToString() + " Songs";
                    playlistCover.sprite = playlistHelper.coverImage;
                    playlistCoverBanner.sprite = playlistHelper.coverImage;
                });
            }

            // Set the first music and then pause it - this is for visiblity
            nowPlayingListTitle.text = playlists[0].playlistName;
            PlayMusic();
            PauseMusic();
            this.enabled = false;
        }

        public void PreparePlaylist(int playlistIndex)
        {
            // Destroy each object in playlist parent
            foreach (Transform child in playlistContentParent)
                Destroy(child.gameObject);

            // Set the playlist index to prepare for the next one
            playlistHelper = playlists[playlistIndex];

            // Sort playlist by A to Z
            if (sortListByName == true)
                playlistHelper.playlist.Sort(SortByName);

            // Instantiate the entire playlist songs as buttons
            for (int i = 0; i < playlistHelper.playlist.Count; ++i)
            {
                if (playlistHelper.playlist[i].excludeFromLibrary == false)
                {
                    // Spawn song button
                    GameObject go = Instantiate(musicLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(playlistContentParent, false);
                    go.gameObject.name = playlistHelper.playlist[i].musicTitle;

                    // Set button BG color
                    Button buttonGO;
                    buttonGO = go.transform.GetComponent<Button>();
                    ColorBlock buttonColor;

                    // This is for visibility in library, qol
                    if (allowSwitchColor == false)
                    {
                        buttonColor = buttonGO.colors;
                        buttonColor.normalColor = new Color32(255, 255, 255, 10);
                        buttonGO.colors = buttonColor;
                        allowSwitchColor = true;
                    }

                    else
                    {
                        buttonColor = buttonGO.colors;
                        buttonColor.normalColor = new Color32(255, 255, 255, 0);
                        buttonGO.colors = buttonColor;
                        allowSwitchColor = false;
                    }

                    // Set cover image
                    Transform coverGO;
                    coverGO = go.transform.Find("Cover/Image").GetComponent<Transform>();
                    coverGO.GetComponent<Image>().sprite = playlistHelper.playlist[i].musicCover;

                    // Set ID tags
                    TextMeshProUGUI songName;
                    songName = go.transform.Find("Song Title").GetComponent<TextMeshProUGUI>();
                    songName.text = playlistHelper.playlist[i].musicTitle;
                    TextMeshProUGUI artistName;
                    artistName = go.transform.Find("Artist Name").GetComponent<TextMeshProUGUI>();
                    artistName.text = playlistHelper.playlist[i].artistTitle;
                    TextMeshProUGUI durationText;
                    durationText = go.transform.Find("Duration").GetComponent<TextMeshProUGUI>();
                    durationText.text = (((int)playlistHelper.playlist[i].musicClip.length / 60) % 60) + ":" + ((int)playlistHelper.playlist[i].musicClip.length % 60).ToString("D2");

                    // Add button events
                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(delegate
                    {
                        currentPlaylist = playlists[playlistIndex];
                        PlayCustomMusic(go.transform.GetSiblingIndex());
                        nowPlayingListTitle.text = playlists[playlistIndex].playlistName;
                    });

                    // Reset Play All button and replace with the current playlist songs
                    playlistPlayAllButton.onClick.AddListener(delegate
                    {
                        playlistPlayAllButton.onClick.RemoveAllListeners();
                        currentPlaylist = playlists[playlistIndex];
                        currentTrack = 0;
                        PlayCustomMusic(0);
                        PlayMusic();
                        nowPlayingListTitle.text = playlists[playlistIndex].playlistName;
                    });
                }
            }
        }

        void Update()
        {
            if (source.isPlaying == false)
                return;

            // If shuffle is true and repeat is false, process NextTitle when the current song ends
            if (playTime == duration && shuffle == true && repeat == false)
            {
                NextTitle();
                source.time = 0;
                isReady = false;
                StartCoroutine("WaitForMusicEnd");
            }

            // If shuffle is false and repeat is true, repeat the current song
            else if (playTime == duration && repeat == true && shuffle == false)
            {
                source.Stop();
                source.time = 0;
                source.Play();
                StartCoroutine("WaitForMusicEnd");
            }

            // If shuffle is false and repeat is false, change to the next song when the current song ends
            else if (playTime == duration && shuffle == false && repeat == false)
            {
                source.Stop();
                currentTrack++;

                if (currentTrack > currentPlaylist.playlist.Count - 1)
                    currentTrack = 0;

                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
                source.Play();
                source.time = 0;
                isReady = true;
                StartCoroutine("WaitForMusicEnd");
            }

            // If shuffle is true and repeat is true, change to the next song when the current song ends
            else if (playTime == duration && shuffle == true && repeat == true)
            {
                source.Stop();
                currentTrack++;

                if (currentTrack > currentPlaylist.playlist.Count - 1)
                    currentTrack = 0;

                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
                source.Play();
                source.time = 0;
                isReady = true;
                StartCoroutine("WaitForMusicEnd");
            }
        }

        IEnumerator WaitForMusicEnd()
        {
            // Change the play time until the end
            while (source.isPlaying)
            {
                playTime = (int)source.time;
                ShowPlayTime();
                yield return null;
            }

            // Process NextTitle when the current song ends
            if (isReady == true)
            {
                NextTitle();
                isReady = false;
            }
        }

        public void PlayMusic()
        {
            this.enabled = true;

            // If resources are assigned
            if (source == null || tempClip == null)
                source.clip = currentPlaylist.playlist[currentTrack].musicClip;

            // Play and change the data
            source.Play();
            ShowCurrentTitle();
            StartCoroutine("WaitForMusicEnd");
            isReady = false;
        }

        public void PlayCustomMusic(int musicIndex)
        {
            // Play a specific music depending on the index
            source.Stop();
            currentTrack = musicIndex;
            source.clip = currentPlaylist.playlist[musicIndex].musicClip;
            source.time = 0;
            source.Play();
            ShowCurrentTitle();
            StartCoroutine("WaitForMusicEnd");
        }

        public void PlayCustomClip(AudioClip clip, Sprite cover, string clipName, string clipAuthor)
        {
            // Adding a new clip to the playlist
            MusicPlayerPlaylist.MusicItem item = new MusicPlayerPlaylist.MusicItem();
            item.musicClip = clip;
            item.musicTitle = clipName;
            item.artistTitle = clipAuthor;
            item.musicCover = cover;
            item.excludeFromLibrary = true;
            currentPlaylist.playlist.Add(item);

            // Play the clip
            source.Stop();
            currentTrack = currentPlaylist.playlist.Count - 1;
            source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            source.time = 0;
            source.Play();
            ShowCurrentTitle();
            StartCoroutine("WaitForMusicEnd");
        }

        public void PauseMusic()
        {
            // Yup, pause
            source.Pause();
        }

        public void NextTitle()
        {
            // Stop!
            source.Stop();

            // If shuffle is true and repeat is false, select a random song from the current list
            if (shuffle == true && repeat == false)
            {
                // Remember the current track and then pick a random one
                randomHelper = currentTrack;
                currentTrack = Random.Range(0, currentPlaylist.playlist.Count);

                // Change the song again - only if you get the same song with the previous one
                if (currentTrack == randomHelper || currentPlaylist.playlist[currentTrack].excludeFromLibrary == true)
                    NextTitle();

                // Assign the current song to audio source
                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            }

            // If not, then just skip to the next song
            else
            {
                currentTrack++;

                // Go back to the first song when reaching to the end of playlist
                if (currentTrack > currentPlaylist.playlist.Count - 1)
                    currentTrack = 0;

                if (currentPlaylist.playlist[currentTrack].excludeFromLibrary == true)
                    NextTitle();

                // Assign the current song to audio source
                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            }

            // Play and change the data
            source.time = 0;
            source.Play();
            ShowCurrentTitle();
            StartCoroutine("WaitForMusicEnd");

            // If notifications are on, then create one through the creator
            if (enablePopupNotification == true && notificationCreator != null)
            {
                notificationCreator.notificationTitle = currentPlaylist.playlist[currentTrack].musicTitle;
                notificationCreator.popupDescription = currentPlaylist.playlist[currentTrack].artistTitle;
                notificationCreator.CreateOnlyPopup();
            }
        }

        public void PrevTitle()
        {
            // Stop!
            source.Stop();

            // If shuffle is true and repeat is false, select a random song from the current list
            if (shuffle == true && repeat == false)
            {
                // Remember the current track and then pick a random one
                randomHelper = currentTrack;
                currentTrack = Random.Range(0, currentPlaylist.playlist.Count);

                // Change the song again - only if you get the same song with the previous one
                if (currentTrack == randomHelper)
                    currentTrack = Random.Range(0, currentPlaylist.playlist.Count);

                // Assign the current song to audio source
                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            }

            // If not, then just skip to the previous song
            else
            {
                currentTrack--;

                // Go back to the first song when it doesn't meet the requirements
                if (currentTrack < 0)
                    currentTrack = currentPlaylist.playlist.Count - 1;

                // Assign the current song to audio source
                source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            }

            // Play and change the data
            source.clip = currentPlaylist.playlist[currentTrack].musicClip;
            source.time = 0;
            source.Play();
            ShowCurrentTitle();
            StartCoroutine("WaitForMusicEnd");

            // If notifications are on, then create one through the creator
            if (enablePopupNotification == true)
            {
                notificationCreator.notificationTitle = currentPlaylist.playlist[currentTrack].musicTitle;
                notificationCreator.popupDescription = currentPlaylist.playlist[currentTrack].artistTitle;
                notificationCreator.CreateOnlyPopup();
            }
        }

        public void StopMusic()
        {
            // Stop the playback
            source.Stop();
            StopCoroutine("WaitForMusicEnd");
        }

        public void MuteMusic()
        {
            // Mute or unmute
            source.mute = !source.mute;
        }

        public void ShowCurrentTitle()
        {
            // Set duration depedning on the source clip
            duration = (int)source.clip.length;
        }

        public void ShowPlayTime()
        {
            // Divide seconds and minutes by 60
            seconds = playTime % 60;
            minutes = (playTime / 60) % 60;
        }

        public void AddPlaylist()
        {
            // Only for adding a playlist for editor
            playlists.Add(null);
        }

        public void UnloadMusicPlayerAudio()
        {
            source.clip.UnloadAudioData();
        }
    }
}