using UnityEngine;

namespace Michsky.DreamOS
{
    public class NotificationListClear : MonoBehaviour
    {
        [Header("Resources")]
        public Transform listContent;

        void Awake()
        {
            if (listContent == null)
                listContent = transform.Find("Notification List");

            foreach (Transform child in listContent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        void Update()
        {
            if (listContent == null)
                listContent = transform.Find("Notification List");

            foreach (Transform child in listContent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}