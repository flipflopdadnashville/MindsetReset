using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Michsky.DreamOS
{
    public class NotificationDestroy : MonoBehaviour
    {
        void OnEnable()
        {
            StartCoroutine(SelfDestruct());
        }

        IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
    }
}