using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class PhotoGalleryManager : MonoBehaviour
    {
        // Resources
        public PhotoGalleryLibrary libraryAssets;
        public Transform pictureLibraryParent;
        public GameObject pictureLibraryButton;
        public GameObject photoGalleryWindow;
        public Image imageViewer;
        public TextMeshProUGUI viewerTitle;
        public TextMeshProUGUI viewerDescription;
        [HideInInspector] public WindowPanelManager wManager;

        // Settings
        public bool sortListByName = true;
        public string viewerPanelName = "Viewer";

        private static int SortByName(PhotoGalleryLibrary.PictureItem o1, PhotoGalleryLibrary.PictureItem o2)
        {
            // Compare the names and sort by A to Z
            return o1.pictureTitle.CompareTo(o2.pictureTitle);
        }

        void Start()
        {
            InitializePictures();
        }

        public void InitializePictures()
        {
            // Get window manager from the main window
            wManager = photoGalleryWindow.GetComponent<WindowPanelManager>();

            // Destroy each object in picture library parent
            foreach (Transform child in pictureLibraryParent)
                Destroy(child.gameObject);

            // Sort pictures by A to Z if it's enabled
            if (sortListByName == true)
                libraryAssets.pictures.Sort(SortByName);

            // Instantiate the entire picture library as buttons
            for (int i = 0; i < libraryAssets.pictures.Count; ++i)
            {
                // Spawn picture button
                GameObject go = Instantiate(pictureLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(pictureLibraryParent, false);
                go.gameObject.name = libraryAssets.pictures[i].pictureTitle;

                // Set image
                Transform coverGO;
                coverGO = go.transform.Find("Image Parent/Image").GetComponent<Transform>();
                coverGO.GetComponent<Image>().sprite = libraryAssets.pictures[i].pictureSprite;

                // Set ID tags
                TextMeshProUGUI songName;
                songName = go.transform.Find("Highlighted/Image Title").GetComponent<TextMeshProUGUI>();
                songName.text = libraryAssets.pictures[i].pictureTitle;
                TextMeshProUGUI artistName;
                artistName = go.transform.Find("Highlighted/Image Description").GetComponent<TextMeshProUGUI>();
                artistName.text = libraryAssets.pictures[i].pictureDescription;

                // Add button events
                Button itemButton;
                itemButton = go.GetComponent<Button>();
                itemButton.onClick.AddListener(delegate
                {
                    OpenCustomPicture(go.transform.GetSiblingIndex());
                    wManager.OpenPanel(viewerPanelName);
                });
            }
        }

        public void OpenCustomPicture(int pictureIndex)
        {
            // Open picture depending on picture index from the library
            imageViewer.sprite = libraryAssets.pictures[pictureIndex].pictureSprite;
            viewerTitle.text = libraryAssets.pictures[pictureIndex].pictureTitle;
            viewerDescription.text = libraryAssets.pictures[pictureIndex].pictureDescription;
        }

        public void OpenCustomSprite(Sprite pictureIndex, string title, string description)
        {
            // Open picture depending on vars (e.g. downloaded file)
            imageViewer.sprite = pictureIndex;
            viewerTitle.text = title;
            viewerDescription.text = description;
            wManager.OpenPanel(viewerPanelName);
        }
    }
}