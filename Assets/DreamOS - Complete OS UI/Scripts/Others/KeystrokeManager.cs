using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(AudioSource))]
    public class KeystrokeManager : MonoBehaviour
    {
        [Header("Strokes")]
        public List<AudioClip> keyboardStrokes = new List<AudioClip>();
        public List<AudioClip> mouseStrokes = new List<AudioClip>();

        [Header("Settings")]
        public AudioSource strokeSource;
        public bool enableStrokes = true;
        public bool enableKeyboard = true;
        public bool enableMouse = true;

        void Start()
        {
            if (strokeSource == null)
                strokeSource = gameObject.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (enableStrokes == false)
                return;

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2))
                PlayMouseStroke();
            else if (Input.anyKeyDown)
                PlayKeyboardStroke();
        }

        public void PlayKeyboardStroke()
        {
            if (enableKeyboard == true)
                strokeSource.PlayOneShot(keyboardStrokes[Random.Range(0, keyboardStrokes.Count)]);
        }

        public void PlayMouseStroke()
        {
            if (enableMouse == true)
                strokeSource.PlayOneShot(mouseStrokes[Random.Range(0, mouseStrokes.Count)]);
        }

        public void EnableStrokes(bool bValue)
        {
            if (bValue == true)
            {
                enableStrokes = true;
                this.enabled = true;
            }

            else
            {
                enableStrokes = false;
                this.enabled = false;
            }
           
        }
    }
}