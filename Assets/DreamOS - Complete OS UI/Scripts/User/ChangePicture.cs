using UnityEngine;
using UnityEngine.UI;

namespace Michsky.DreamOS
{
    public class ChangePicture : MonoBehaviour
    {
        [Header("Settings")]
        public Sprite profilePicture;

        [Header("Resources")]
        public Image mainImage;
        public Image placeholderImage;

        public void ChangeImage()
        {
            try
            {
                if (mainImage == null)
                    mainImage = gameObject.transform.Find("Image").GetComponent<Image>();

                mainImage.sprite = profilePicture;
                placeholderImage.sprite = profilePicture;
            }

            catch
            {
                Debug.LogWarning("Change Picture - Cannot initalize because of missing resources.", this);
            }
        }
    }
}