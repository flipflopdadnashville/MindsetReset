using UnityEngine;

namespace Michsky.DreamOS.Examples
{
    public class CreatePhotoItem : MonoBehaviour
    {
        [Header("CONTENT")]
        public Sprite photoSprite;
        public string title;
        public string description;

        [Header("RESOURCES")]
        public PhotoGalleryManager photoManager;

        public void AddPhotoToLibrary()
        {
            PhotoGalleryLibrary.PictureItem item = new PhotoGalleryLibrary.PictureItem();
            item.pictureSprite = photoSprite;
            item.pictureTitle = title;
            item.pictureDescription = description;

            if (photoManager == null)
            {
                try
                {
                    var pmObj = (PhotoGalleryManager)GameObject.FindObjectsOfType(typeof(PhotoGalleryManager))[0];
                    photoManager = pmObj;
                }

                catch 
                {
                    Debug.LogWarning("Photo Manager is not assigned.", this);
                    return;
                }
            }

            photoManager.libraryAssets.pictures.Add(item);
            photoManager.InitializePictures();
        }

        public void AddCustomPhotoToLibrary(Sprite photoVar, string titleVar, string descriptionVar)
        {
            PhotoGalleryLibrary.PictureItem item = new PhotoGalleryLibrary.PictureItem();
            item.pictureSprite = photoVar;
            item.pictureTitle = titleVar;
            item.pictureDescription = descriptionVar;

            if (photoManager == null)
            {
                try
                {
                    var pmObj = (PhotoGalleryManager)GameObject.FindObjectsOfType(typeof(PhotoGalleryManager))[0];
                    photoManager = pmObj;
                }

                catch
                {
                    Debug.LogWarning("Photo Manager is not assigned.", this);
                    return;
                }
            }

            photoManager.libraryAssets.pictures.Add(item);
            photoManager.InitializePictures();
        }
    }
}