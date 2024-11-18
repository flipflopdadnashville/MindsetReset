using UnityEngine;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/Events/Press Key")]
    public class PressKeyEvent : MonoBehaviour
    {
        // Settings
        public KeyCode hotkey;
        public bool pressAnyKey;

        // Events
        public UnityEvent pressAction;

        void Update()
        {
            if (pressAnyKey == true && Input.anyKeyDown)
                pressAction.Invoke();
            else if (pressAnyKey == false && Input.GetKeyDown(hotkey))
                pressAction.Invoke();
        }
    }
}