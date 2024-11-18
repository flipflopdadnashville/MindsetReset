using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    public class WorldSpaceManager : MonoBehaviour
    {
        // Resources
        public Transform mainCamera;
        public Transform enterMount;
        public Camera projectorCam;
        public Camera OSCam;
        public Canvas osCanvas;
        public FloatingIconManager useFloatingIcon;

        // Settings
        public bool requiresOpening = true;
        public bool autoGetIn = false;
        public bool lockCursorWhenOut = false;
        public string playerTag = "Player";
        public KeyCode getInKey;
        public KeyCode getOutKey;
        [Range(0.1f, 50f)] public float transitionSpeed = 10f;

        // Events
        public UnityEvent onEnter;
        public UnityEvent onEnterEnd;
        public UnityEvent onExit;
        public UnityEvent onExitEnd;

        [HideInInspector] public bool isPowerOn;
        bool isInTrigger = false;
        bool isInSystem = false;
        bool enableCameraIn = false;
        bool enableCameraOut = false;
        bool camRotTaken;
        bool onExitHelper;
        RenderTexture uiRT;
        CanvasScaler osScaler;
        Quaternion camRotHelper;
        Vector3 camPosHelper;

        void Start()
        {
            if (OSCam != null)
                OSCam.gameObject.SetActive(false);

            uiRT = projectorCam.targetTexture;
            uiRT.width = Screen.currentResolution.width;
            uiRT.height = Screen.currentResolution.height;
            osScaler = osCanvas.GetComponent<CanvasScaler>();

            if (requiresOpening == true)
                osCanvas.gameObject.SetActive(false);
        }

        void Update()
        {
            if (isInTrigger == true)
            {
                if (Input.GetKey(getInKey))
                {
                    osCanvas.gameObject.SetActive(true);
                    useFloatingIcon.enableUpdating = false;
                    enableCameraOut = false;
                    enableCameraIn = true;
                    onEnter.Invoke();
                    isInTrigger = false;
                    isInSystem = true;
                }
            }

            if (isInSystem == true)
            {
                if (Input.GetKey(getOutKey))
                {
                    if (enableCameraIn == true)
                    {
                        osCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        projectorCam.gameObject.SetActive(false);
                        enableCameraIn = false;
                        onEnterEnd.Invoke();
                    }

                    enableCameraOut = true;
                    onExit.Invoke();
                    isInTrigger = true;
                    isInSystem = false;
                    enableCameraIn = false;
                }
            }

            if (enableCameraIn == true)
            {
                if (camRotTaken == false)
                {
                    camRotHelper = mainCamera.rotation;
                    camPosHelper = mainCamera.position;
                    camRotTaken = true;
                }

                Cursor.visible = true;
                mainCamera.position = Vector3.Lerp(mainCamera.position, enterMount.position, transitionSpeed * Time.deltaTime);
                mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, enterMount.rotation, transitionSpeed * Time.deltaTime);

                if (lockCursorWhenOut == true)
                    Cursor.lockState = CursorLockMode.None;

                if (mainCamera.position == enterMount.position)
                {
                    osCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    projectorCam.gameObject.SetActive(false);
                    OSCam.gameObject.SetActive(true);
                    enableCameraIn = false;
                    onEnterEnd.Invoke();
                }
            }

            else if (enableCameraOut == true)
            {
                Cursor.visible = false;
                onExitHelper = true;

                if (onExitHelper == true)
                {
                    onExitHelper = false;
                    projectorCam.gameObject.SetActive(true);
                    OSCam.gameObject.SetActive(false);
                    onExit.Invoke();
                }

                osCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                projectorCam.targetTexture = uiRT;
                mainCamera.position = Vector3.Lerp(mainCamera.position, camPosHelper, transitionSpeed * Time.deltaTime);
                mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, camRotHelper, transitionSpeed * Time.deltaTime);

                if (lockCursorWhenOut == true)
                    Cursor.lockState = CursorLockMode.Locked;

                if (mainCamera.position == camPosHelper)
                {
                    enableCameraOut = false;
                    camRotTaken = false;
                    onExitEnd.Invoke();

                    if (useFloatingIcon != null)
                        useFloatingIcon.enableUpdating = true;
                }
            }
        }

        public void GetOut()
        {
            enableCameraIn = false;
            enableCameraOut = true;
            isInTrigger = true;
            isInSystem = false;
            onExit.Invoke();
        }

        public void GetIn()
        {
            enableCameraIn = true;
            enableCameraOut = false;
            isInTrigger = true;
            isInSystem = true;
            onEnter.Invoke();
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                isInTrigger = true;

                if (autoGetIn == true)
                {
                    enableCameraIn = true;
                    onEnter.Invoke();
                    isInTrigger = false;
                    isInSystem = true;
                }

                else if (autoGetIn == false && useFloatingIcon != null)
                    useFloatingIcon.enableUpdating = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == playerTag)
                isInTrigger = false;
        }
    }
}