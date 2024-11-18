using UnityEngine;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/Events/Hold Key Event")]
    public class HoldKeyEvent : MonoBehaviour
    {
        // Settings
        public KeyCode hotkey;

        // Events
        public UnityEvent holdAction;
        public UnityEvent releaseAction;

        // Events
        [HideInInspector] public bool isOn = false;
        [HideInInspector] public bool isHolding = false;

        void Update()
        {
            if (Input.GetKey(hotkey))
            {
                isHolding = true;
                isOn = false;
            }

            else
            {
                isHolding = false;
                isOn = true;
            }

            if (isOn == true && isHolding == false)
            {
                releaseAction.Invoke();
                isHolding = false;
                isOn = false;
            }

            else if (isOn == false && isHolding == true)
            {
                holdAction.Invoke();
                isHolding = true;
            }
        }
    }
}