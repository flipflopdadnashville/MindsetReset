using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class WebBrowserManager : MonoBehaviour
    {
        // Resources
        public NetworkManager networkManager;
        public WebBrowserLibrary webLibrary;
        public TextMeshProUGUI tabTitle;
        public Image tabIcon;
        public Transform pageViewer;
        public TMP_InputField urlField;
        public Slider progressBar;
        public Button previousButton;
        public Button nextButton;
        public Animator favAnimator;
        public Animator favWindowAnimator;
        public Transform favsParent;
        public GameObject favListButton;
        public GameObject downloadedFile;
        public Transform downloadFolder;
        public GameObject downloadEmpty;
        public WindowManager downloadsWindow;

        // Settings
        [Range(0, 30)] public float loadingTime = 1;

        // Debug
        GameObject currentTabPage;
        public List<string> previousSites = new List<string>();
        public List<FavoriteSite> favoriteSites = new List<FavoriteSite>();

        Animator pbAnimator;
        [HideInInspector] public int dirtIndex = 0;
        int urlIndex;
        int dlIndex;
        bool dirtHelper = false;
        bool updatePB = false;
        bool urlFieldActive = false;
        bool isfavWindowOpen = false;
        bool updateIndex = true;

        [System.Serializable]
        public class FavoriteSite
        {
            public Sprite icon;
            public string title = "Title";
            public string url = "Url";
        }

        void Start()
        {
            OpenHome();
            previousButton.interactable = false;
            nextButton.interactable = false;
            progressBar.gameObject.SetActive(false);
            pbAnimator = progressBar.gameObject.GetComponent<Animator>();
            ListFavorites();
            RefreshFavStatus();
            ListDownloadedFiles();
            this.enabled = false;
        }

        void Update()
        {
            if (updatePB == true)
            {
                progressBar.value += Time.deltaTime / loadingTime;

                if (progressBar.value == 1)
                    pbAnimator.Play("Fade Out");
            }

            if (urlFieldActive == true && Input.GetKeyDown(KeyCode.Return))
            {
                OpenWebPage();
                urlField.interactable = false;
                urlField.interactable = true;
                urlFieldActive = false;
            }
        }

        public void UpdateUrlField()
        {
            urlFieldActive = true;
        }

        void OpenHome()
        {
            tabIcon.sprite = webLibrary.webPages[0].pageIcon;
            tabTitle.text = webLibrary.webPages[0].pageTitle;
            urlField.text = webLibrary.webPages[0].pageURL;

            foreach (Transform child in pageViewer)
                child.gameObject.SetActive(false);

            if (currentTabPage != null)
                currentTabPage.SetActive(false);

            GameObject tObject = Instantiate(webLibrary.webPages[0].pageContent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            tObject.name = tObject.name.Replace("(Clone)", "").Trim();
            tObject.transform.SetParent(pageViewer, false);

            currentTabPage = tObject;
            currentTabPage.SetActive(true);
            previousSites.Add(urlField.text);
        }

        public void OpenWebPage()
        {
            StopCoroutine("StartLoadingPage");
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0;
            StartCoroutine("StartLoadingPage");
            updatePB = true;
        }

        IEnumerator StartLoadingPage()
        {
            // Start loading the page depending on loading time
            yield return new WaitForSeconds(loadingTime + 0.4f);
            InitializeWebPage();
            StopCoroutine("StartLoadingPage");
        }

        public void InitializeWebPage()
        {
            for (int i = 0; i < webLibrary.webPages.Count; i++)
            {
                if (urlField.text.ToLower() == webLibrary.webPages[i].pageURL 
                    || urlField.text.ToLower() == "www." + webLibrary.webPages[i].pageURL)
                {
                    urlIndex = i;
                    break;
                }
            }

            if (networkManager.hasConnection == true && urlField.text.ToLower() == webLibrary.webPages[urlIndex].pageURL || networkManager.hasConnection == true && urlField.text.ToLower() == "www." + webLibrary.webPages[urlIndex].pageURL)
            {
                tabIcon.sprite = webLibrary.webPages[urlIndex].pageIcon;
                tabTitle.text = webLibrary.webPages[urlIndex].pageTitle;
                urlField.text = webLibrary.webPages[urlIndex].pageURL.Replace("www.", "").Trim();

                Destroy(currentTabPage);

                GameObject tObject = Instantiate(webLibrary.webPages[urlIndex].pageContent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tObject.name = tObject.name.Replace("(Clone)", "").Trim();
                tObject.transform.SetParent(pageViewer, false);

                currentTabPage = tObject;
                currentTabPage.SetActive(true);
            }

            else if (networkManager.hasConnection == false)
            {
                tabIcon.sprite = webLibrary.webPages[2].pageIcon;
                tabTitle.text = webLibrary.webPages[2].pageTitle;
                urlField.text = webLibrary.webPages[2].pageURL;

                Destroy(currentTabPage);

                GameObject tObject = Instantiate(webLibrary.webPages[2].pageContent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tObject.name = tObject.name.Replace("(Clone)", "").Trim();
                tObject.transform.SetParent(pageViewer, false);

                currentTabPage = tObject;
                currentTabPage.SetActive(true);
            }

            else
            {
                tabIcon.sprite = webLibrary.webPages[1].pageIcon;
                tabTitle.text = webLibrary.webPages[1].pageTitle;
                urlField.text = webLibrary.webPages[1].pageURL;

                Destroy(currentTabPage);

                GameObject tObject = Instantiate(webLibrary.webPages[1].pageContent, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tObject.name = tObject.name.Replace("(Clone)", "").Trim();
                tObject.transform.SetParent(pageViewer, false);

                currentTabPage = tObject;
                currentTabPage.SetActive(true);
            }

            progressBar.value = 0;
            progressBar.gameObject.SetActive(false);
            updatePB = false;

            if (dirtHelper == false)
            {
                dirtIndex += 1;
                previousSites.Add(urlField.text);
            }

            if (updateIndex == true)
                dirtIndex = previousSites.Count - 1;
            else
                updateIndex = true;

            dirtHelper = false;
            CheckPreviousActions();
            RefreshFavStatus();
        }

        public void OpenPage(string url)
        {
            // Open a specific page
            urlField.text = url;
            OpenWebPage();
        }

        public void GoBack()
        {
            // Simple back method
            if (dirtIndex >= 1)
            {
                dirtIndex -= 1;
                urlField.text = previousSites[dirtIndex];
                dirtHelper = true;
                OpenWebPage();
            }

            updateIndex = false;
            CheckPreviousActions();
        }

        public void GoForward()
        {
            // Simple forward method
            if (dirtIndex <= previousSites.Count - 2)
            {
                dirtIndex += 1;
                urlField.text = previousSites[dirtIndex];
                dirtHelper = true;
                OpenWebPage();
            }

            updateIndex = false;
            CheckPreviousActions();
        }

        public void Refresh()
        {
            // Simple refresh method
            urlField.text = previousSites[dirtIndex];
            dirtHelper = true;
            OpenWebPage();
        }

        public void CheckPreviousActions()
        {
            // Check for previous button
            if (dirtIndex == 0)
                previousButton.interactable = false;
            else
                previousButton.interactable = true;

            // Check for next button
            if (dirtIndex == previousSites.Count - 1)
                nextButton.interactable = false;
            else
                nextButton.interactable = true;
        }

        public void SetFavorite()
        {
            // Add the current website to favorites - only if it is not already added
            if (PlayerPrefs.GetString(urlField.text) == "")
            {
                FavoriteSite fav = new FavoriteSite();
                fav.icon = tabIcon.sprite;
                fav.title = tabTitle.text;
                fav.url = urlField.text;
                favoriteSites.Add(fav);

                GameObject favObj = Instantiate(favListButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                favObj.name = urlField.text;
                favObj.transform.SetParent(favsParent, false);

                Image favObjImg = favObj.transform.Find("Icon").GetComponent<Image>();
                favObjImg.sprite = tabIcon.sprite;

                TextMeshProUGUI favObjTxt = favObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                favObjTxt.text = tabTitle.text;

                PlayerPrefs.SetString(fav.url, fav.url);

                Button favObjBtn = favObj.GetComponent<Button>();
                favObjBtn.onClick.AddListener(delegate
                {
                    OpenPage(PlayerPrefs.GetString(fav.url));
                });

                favAnimator.Play("Enable");
            }

            // If not, delete it from favorites
            else
            {
                for (int i = 0; i < favoriteSites.Count; i++)
                {
                    if (urlField.text.ToLower() == favoriteSites[i].url)
                    {
                        GameObject favObj = favsParent.Find(favoriteSites[i].url).gameObject;
                        Destroy(favObj);
                        favoriteSites.RemoveAt(i);
                        PlayerPrefs.DeleteKey(favObj.name);
                        favAnimator.Play("Disable");
                    }
                }
            }
        }

        public void RefreshFavStatus()
        {
            // Simple animating solution for refreshing favorite status
            if (favAnimator.gameObject.activeInHierarchy == true)
            {
                if (PlayerPrefs.GetString(urlField.text) == "")
                    favAnimator.Play("Disable");
                else
                    favAnimator.Play("Enable");
            }
        }

        public void ListFavorites()
        {
            // First of all, clear the parent object
            foreach (Transform child in favsParent)
                Destroy(child.gameObject);

            // Start searching in web library list
            for (int i = 0; i < webLibrary.webPages.Count; i++)
            {
                int index = i;

                // Create the items which was already added to fav list
                if (webLibrary.webPages[i].pageURL == PlayerPrefs.GetString(webLibrary.webPages[i].pageURL))
                {
                    FavoriteSite fav = new FavoriteSite();

                    GameObject favObj = Instantiate(favListButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    favObj.name = webLibrary.webPages[index].pageURL;
                    favObj.transform.SetParent(favsParent, false);

                    Image favObjImg = favObj.transform.Find("Icon").GetComponent<Image>();
                    favObjImg.sprite = webLibrary.webPages[index].pageIcon;

                    TextMeshProUGUI favObjTxt = favObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                    favObjTxt.text = webLibrary.webPages[index].pageTitle;

                    Button favObjBtn = favObj.GetComponent<Button>();
                    favObjBtn.onClick.AddListener(delegate
                    {
                        OpenPage(PlayerPrefs.GetString(webLibrary.webPages[index].pageURL));
                        AnimateFavWindow();
                    });

                    fav.url = PlayerPrefs.GetString(webLibrary.webPages[index].pageURL);
                    favoriteSites.Add(fav);
                }
            }
        }

        public void ClearFavoriteList()
        {
            // Search and clear favorite list
            for (int i = 0; i < favoriteSites.Count; i++)
            {
                GameObject favObj = favsParent.Find(favoriteSites[i].url).gameObject;
                Destroy(favObj);
                PlayerPrefs.DeleteKey(favoriteSites[i].url);
            }
        }

        public void AnimateFavWindow()
        {
            // Simple animating solution for fav window
            if (isfavWindowOpen == false)
            {
                favWindowAnimator.Play("In");
                isfavWindowOpen = true;
            }

            else
            {
                favWindowAnimator.Play("Out");
                isfavWindowOpen = false;
            }
        }

        public void DownloadFile(string title)
        {
            // Open download window
            if (downloadsWindow != null)
                downloadsWindow.OpenWindow();

            // Checking if the file is already in download process
            Transform checker = downloadFolder.Find(title);

            // And if it is, don't go further
            if (checker != null)
                return;

            // If not, search for the file in web library
            for (int i = 0; i < webLibrary.dlFiles.Count; i++)
            {
                if (title == webLibrary.dlFiles[i].fileName)
                {
                    dlIndex = i;
                    PlayerPrefs.SetInt(title + "EventHelper", i);
                    break;
                }
            }

            // Start downloading depending on the file state (0: not downloaded, 1: in progress, 2: downloaded)
            if (PlayerPrefs.GetInt("Downloaded" + title) == 0 || PlayerPrefs.GetInt("Downloaded" + title) == 1)
            {
                GameObject dfObj = Instantiate(downloadedFile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                dfObj.name = title;
                dfObj.transform.SetParent(downloadFolder, false);

                Image dfImg = dfObj.transform.Find("Icon").GetComponent<Image>();
                dfImg.sprite = webLibrary.dlFiles[dlIndex].fileIcon;

                TextMeshProUGUI dfTxt = dfObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                dfTxt.text = webLibrary.dlFiles[dlIndex].fileName;

                TextMeshProUGUI dfSize = dfObj.transform.Find("Size").GetComponent<TextMeshProUGUI>();
                dfSize.text = webLibrary.dlFiles[dlIndex].fileSize.ToString() + " MB";

                DownloadFile dfVar = dfObj.gameObject.GetComponent<DownloadFile>();
                dfVar.downloadMultiplier = networkManager.networkItems[networkManager.currentNetworkIndex].networkSpeed;
                dfVar.fileSize = webLibrary.dlFiles[dlIndex].fileSize;
                dfVar.enableNotification = true;

                Button dfObjBtn = dfObj.GetComponent<Button>();
                dfObjBtn.onClick.AddListener(delegate
                {
                    webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].externalEvents.Invoke();

                    try
                    {
                        if (webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileType == WebBrowserLibrary.FileType.MUSIC)
                        {
                            MusicPlayerManager mp = GameObject.Find("Managers/Music Player").GetComponent<MusicPlayerManager>();
                            WindowManager wm = mp.musicPanelManager.gameObject.GetComponent<WindowManager>();
                            wm.OpenWindow();
                            mp.PlayCustomClip(webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].musicReference, // Music clip
                             webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileIcon, // Music cover
                             webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileName, // Music name
                             "Downloads"); // Artist name
                        }

                        else if (webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileType == WebBrowserLibrary.FileType.NOTE)
                        {
                            NotepadManager nm = GameObject.Find("Managers/Notepad").GetComponent<NotepadManager>();
                            WindowManager wm = nm.notepadWindow.gameObject.GetComponent<WindowManager>();
                            wm.OpenWindow();
                            nm.OpenCustomNote(webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileName, // Note title
                                webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].noteReference); // Note content
                        }

                        else if (webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileType == WebBrowserLibrary.FileType.PHOTO)
                        {
                            PhotoGalleryManager pm = GameObject.Find("Managers/Photo Gallery").GetComponent<PhotoGalleryManager>();
                            WindowManager wm = pm.photoGalleryWindow.gameObject.GetComponent<WindowManager>();
                            wm.OpenWindow();
                            pm.OpenCustomSprite(webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].photoReference, // Sprite
                                webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileName, // Photo title
                                "Downloads"); // Photo description
                        }

                        else if (webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileType == WebBrowserLibrary.FileType.VIDEO)
                        {
                            VideoPlayerManager vpm = GameObject.Find("Managers/Video Player").GetComponent<VideoPlayerManager>();
                            WindowManager wm = vpm.videoPlayerWindow.gameObject.GetComponent<WindowManager>();
                            wm.OpenWindow();
                            vpm.wManager.OpenPanel(vpm.videoPanelName);
                            vpm.PlayVideoClip(webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].videoReference, // Video clip
                                webLibrary.dlFiles[PlayerPrefs.GetInt(title + "EventHelper")].fileName); // Video name
                        }
                    }

                    catch
                    {
                        Debug.LogError("<b>[Web Browser]</b> Cannot process the events due to missing resources.", dfObjBtn.gameObject);
                    }
                });

                PlayerPrefs.SetInt("Downloaded" + title, 1);
                dfObjBtn.interactable = false;

                if (downloadFolder != null && downloadEmpty != null)
                    downloadEmpty.SetActive(false);
            }
        }

        public void ListDownloadedFiles()
        {
            // First of all, clear the parent object
            foreach (Transform child in downloadFolder)
                Destroy(child.gameObject);

            // Start searching in web library list
            for (int i = 0; i < webLibrary.dlFiles.Count; i++)
            {
                int index = i;

                // Create the files which was already downloaded
                if (PlayerPrefs.GetInt("Downloaded" + webLibrary.dlFiles[i].fileName) == 2)
                {
                    GameObject dfObj = Instantiate(downloadedFile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    dfObj.name = webLibrary.dlFiles[index].fileName;
                    dfObj.transform.SetParent(downloadFolder, false);

                    Image dfImg = dfObj.transform.Find("Icon").GetComponent<Image>();
                    dfImg.sprite = webLibrary.dlFiles[index].fileIcon;

                    TextMeshProUGUI dfTxt = dfObj.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                    dfTxt.text = webLibrary.dlFiles[index].fileName;

                    TextMeshProUGUI dfSize = dfObj.transform.Find("Size").GetComponent<TextMeshProUGUI>();
                    dfSize.text = webLibrary.dlFiles[index].fileSize.ToString() + " MB";

                    DownloadFile dfVar = dfObj.gameObject.GetComponent<DownloadFile>();
                    dfVar.downloadMultiplier = networkManager.networkItems[networkManager.currentNetworkIndex].networkSpeed;
                    dfVar.fileSize = webLibrary.dlFiles[index].fileSize;

                    Button dfObjBtn = dfObj.GetComponent<Button>();
                    dfObjBtn.onClick.AddListener(delegate
                    {
                        webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].externalEvents.Invoke();

                        try
                        {
                            if (webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileType == WebBrowserLibrary.FileType.MUSIC)
                            {
                                MusicPlayerManager mp = GameObject.Find("Managers/Music Player").GetComponent<MusicPlayerManager>();
                                WindowManager wm = mp.musicPanelManager.gameObject.GetComponent<WindowManager>();
                                wm.OpenWindow();
                                mp.PlayCustomClip(webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].musicReference, // Music clip
                                 webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileIcon, // Music cover
                                 webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileName, // Music name
                                 "Downloads"); // Artist name
                            }

                            else if (webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileType == WebBrowserLibrary.FileType.NOTE)
                            {
                                NotepadManager nm = GameObject.Find("Managers/Notepad").GetComponent<NotepadManager>();
                                WindowManager wm = nm.notepadWindow.gameObject.GetComponent<WindowManager>();
                                wm.OpenWindow();
                                nm.OpenCustomNote(webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileName, // Note name
                                    webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].noteReference); // Note content
                            }

                            else if (webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileType == WebBrowserLibrary.FileType.PHOTO)
                            {
                                PhotoGalleryManager pm = GameObject.Find("Managers/Photo Gallery").GetComponent<PhotoGalleryManager>();
                                WindowManager wm = pm.photoGalleryWindow.gameObject.GetComponent<WindowManager>();
                                wm.OpenWindow();
                                pm.OpenCustomSprite(webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].photoReference, // Sprite
                                    webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileName, // Photo name
                                    "Downloads"); // Description
                            }

                            else if (webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileType == WebBrowserLibrary.FileType.VIDEO)
                            {
                                VideoPlayerManager vpm = GameObject.Find("Managers/Video Player").GetComponent<VideoPlayerManager>();
                                WindowManager wm = vpm.videoPlayerWindow.gameObject.GetComponent<WindowManager>();
                                wm.OpenWindow();
                                vpm.wManager.OpenPanel(vpm.videoPanelName);
                                vpm.PlayVideoClip(webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].videoReference, // Video clip
                                    webLibrary.dlFiles[PlayerPrefs.GetInt(dfObj.name + "EventHelper")].fileName); // Video name
                            }
                        }

                        catch
                        {
                            Debug.LogError("<b>[Web Browser]</b> Cannot process the events due to missing resources.", dfObjBtn.gameObject);
                        }
                    });

                    dfObjBtn.interactable = false;
                }
            }

            // If there isn't any downloaded file, then enable the label
            if (downloadFolder != null && downloadEmpty != null && downloadFolder.childCount == 0)
                downloadEmpty.SetActive(true);
            else if (downloadEmpty != null)
                downloadEmpty.SetActive(false);
        }

        public void DeleteDownloadFile(string title)
        {
            // Search for the file in the library
            for (int i = 0; i < webLibrary.dlFiles.Count; i++)
            {
                // When we got a match
                if (title == webLibrary.dlFiles[i].fileName)
                {
                    // Find the object in download folder, destroy it, and delete the data
                    GameObject dlObj = downloadFolder.Find(webLibrary.dlFiles[i].fileName).gameObject;
                    Destroy(dlObj);
                    PlayerPrefs.SetInt("Downloaded" + title, 0);
                }
            }

            // If there isn't any downloaded file, then enable the label
            if (downloadFolder != null && downloadEmpty != null && downloadFolder.childCount == 1)
                downloadEmpty.SetActive(true);
            else if (downloadEmpty != null)
                downloadEmpty.SetActive(false);
        }
    }
}