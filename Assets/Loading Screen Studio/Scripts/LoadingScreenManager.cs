using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.LSS
{
    public class LoadingScreenManager : MonoBehaviour
    {
        // Settings
        public string prefabHelper = "Standard";
        public bool enableTrigger;
        public bool onTriggerExit;
        public bool onlyLoadWithTag;
        public bool startLoadingAtStart;
        public string objectTag;
        public string sceneName;

        // Temp Variables
        public Object[] loadingScreens;
        public int selectedLoadingIndex = 0;
        public int selectedTagIndex = 0;

        // Events
        public UnityEvent onLoadStart;
        public List<GameObject> dontDestroyOnLoad = new List<GameObject>();

        void Start()
        {
            if (startLoadingAtStart == true)
                LoadScene(sceneName);
        }

        public void SetStyle(string styleName)
        {
            prefabHelper = styleName;
        }

        public void LoadScene(string sceneName)
        {
            LoadingScreen.prefabName = prefabHelper;
            LoadingScreen.LoadScene(sceneName);

            for (int i = 0; i < dontDestroyOnLoad.Count; i++)
                DontDestroyOnLoad(dontDestroyOnLoad[i]);

            onLoadStart.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enableTrigger == true)
            {
                LoadingScreen.prefabName = prefabHelper;

                if (onlyLoadWithTag == true && onTriggerExit == false)
                {
                    if (other.gameObject.tag == objectTag)
                    {
                        onLoadStart.Invoke();
                        LoadingScreen.LoadScene(sceneName);
                    }
                }

                else if (onTriggerExit == false)
                {
                    onLoadStart.Invoke();
                    LoadingScreen.LoadScene(sceneName);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (enableTrigger == true)
            {
                if (onlyLoadWithTag == true && onTriggerExit == true)
                {
                    if (other.gameObject.tag == objectTag)
                    {
                        LoadingScreen.LoadScene(sceneName);
                        onLoadStart.Invoke();
                    }
                }

                else if (onlyLoadWithTag == false && onTriggerExit == true)
                {
                    LoadingScreen.LoadScene(sceneName);
                    onLoadStart.Invoke();
                }
            }
        }
    }
}