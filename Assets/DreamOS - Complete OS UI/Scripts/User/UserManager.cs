using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Michsky.DreamOS
{
    public class UserManager : MonoBehaviour
    {
        // Resources
        public Animator bootScreen;
        public Animator setupScreen;
        public Animator lockScreen;
        public Animator desktopScreen;
        public TMP_InputField lockScreenPassword;
        public Animator lockScreenErrorPopup;
        public BlurManager lockScreenBlur;
        public TextMeshProUGUI errorTextObject;

        // Content
        [TextArea] public string errorMessage = "Wrong Password";

        [Range(1, 20)] public int minNameCharacter = 1;
        [Range(1, 20)] public int maxNameCharacter = 14;

        [Range(1, 20)] public int minPasswordCharacter = 4;
        [Range(1, 20)] public int maxPasswordCharacter = 16;

        [Range(1, 20)] public int minSecurityCharacter = 1;
        [Range(1, 20)] public int maxSecurityCharacter = 20;

        // Events
        public UnityEvent onLogin;
        public UnityEvent onLock;

        public string systemUsername = "Admin";
        public string systemLastname = "";
        public string systemPassword = "1234";
        public Sprite systemUserPicture;

        // Settings
        public bool disableUserCreating = false;
        public bool deletePrefsAtStart = false;
        public bool showUserData = true;

        // User variables
        [HideInInspector] public string firstName;
        [HideInInspector] public string lastName;
        [HideInInspector] public string password;
        [HideInInspector] public string securityHint;
        [HideInInspector] public string securityAnswer;
        [HideInInspector] public Sprite profilePicture;

        [HideInInspector] public bool hasPassword;
        [HideInInspector] public bool hasSecurityAnswer;
        [HideInInspector] public bool nameOK;
        [HideInInspector] public bool lastNameOK;
        [HideInInspector] public bool passwordOK;
        [HideInInspector] public bool passwordRetypeOK;

        [HideInInspector] public bool isLockScreenOpen = false;
        int userCreated;
        BootManager bootMngr;

        void Start()
        {
            InitializeUserManager();
        }

        public void InitializeUserManager()
        {
            if (disableUserCreating == false)
            {
                // Delete given prefs if option is enabled
                if (deletePrefsAtStart == true)
                    PlayerPrefs.DeleteAll();

                // Getting prefs
                firstName = PlayerPrefs.GetString("FirstName");
                lastName = PlayerPrefs.GetString("LastName");
                password = PlayerPrefs.GetString("Password");
                securityHint = PlayerPrefs.GetString("SecurityQuestionHint");
                securityAnswer = PlayerPrefs.GetString("SecurityQuestion");
                userCreated = PlayerPrefs.GetInt("UserCreated");

                // If password is null, change boolean
                if (password == "")
                    hasPassword = false;
                else
                    hasPassword = true;

                // If security answer is null, change boolean
                if (securityAnswer == "")
                    hasSecurityAnswer = false;
                else
                    hasSecurityAnswer = true;

                // Find Boot manager in the scene
                bootMngr = (BootManager)GameObject.FindObjectsOfType(typeof(BootManager))[0];

                // If user is not created, show Setup screen
                if (userCreated == 0)
                {
                    bootMngr.enabled = false;
                    bootScreen.gameObject.SetActive(false);
                    setupScreen.gameObject.SetActive(true);
                    setupScreen.Play("Panel In");
                }

                // If not, boot the system
                else
                    BootSystem();

                if (errorTextObject == null)
                    errorTextObject = lockScreenErrorPopup.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                errorTextObject.text = errorMessage;
                errorTextObject.gameObject.SetActive(false);
                errorTextObject.gameObject.SetActive(true);
            }

            else
            {
                // If password is null, change boolean
                if (systemPassword == "")
                    hasPassword = false;
                else
                    hasPassword = true;

                // Find Boot manager in the scene
                bootMngr = (BootManager)GameObject.FindObjectsOfType(typeof(BootManager))[0];
                BootSystem();

                if (errorTextObject == null)
                    errorTextObject = lockScreenErrorPopup.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                errorTextObject.text = errorMessage;
                errorTextObject.gameObject.SetActive(false);
                errorTextObject.gameObject.SetActive(true);

                // Setting up the user details
                firstName = systemUsername;
                lastName = systemLastname;
                password = systemPassword;
                profilePicture = systemUserPicture;
                PlayerPrefs.DeleteKey("ProfilePicture");
                PlayerPrefs.SetString("FirstName", firstName);
                PlayerPrefs.SetString("LastName", lastName);
                PlayerPrefs.SetString("Password", password);
                PlayerPrefs.SetString("ProfilePicture", "Profile Pictures/" + systemUserPicture.name);
            }
        }

        public void UpdateUser()
        {
            // Update given prefs
            userCreated = 1;
            PlayerPrefs.SetString("FirstName", firstName);
            PlayerPrefs.SetString("LastName", lastName);
            PlayerPrefs.SetString("Password", password);
            PlayerPrefs.SetString("SecurityQuestionHint", securityHint);
            PlayerPrefs.SetString("SecurityQuestion", securityAnswer);
            PlayerPrefs.SetInt("UserCreated", userCreated);
            profilePicture = Resources.Load<Sprite>(PlayerPrefs.GetString("ProfilePicture"));

            if (showUserData == true)
            {
                showUserData = false;
                showUserData = true;
            }
        }

        public void BootSystem()
        {
            bootMngr.enabled = true;
            bootScreen.gameObject.SetActive(true);
            setupScreen.gameObject.SetActive(false);
            bootScreen.Play("Boot Start");
        }

        public void StartOS()
        {
            if (hasPassword == true)
            {
                lockScreenPassword.gameObject.SetActive(false);
                lockScreen.Play("Skip Login");
            }

            else
            {
                lockScreenPassword.gameObject.SetActive(true);
                lockScreen.Play("Lock Screen In");
            }
        }

        public void LockOS()
        {
            if (lockScreenBlur != null)
                lockScreenBlur.BlurOutAnim();

            lockScreen.Play("Lock Screen In");
            desktopScreen.Play("Desktop Out");
            onLock.Invoke();
        }

        public void LockScreenOpenClose()
        {
            if (isLockScreenOpen == true)
            {
                if (lockScreenBlur != null)
                    lockScreenBlur.BlurOutAnim();

                lockScreen.Play("Lock Screen Out");
            }

            else
            {
                lockScreen.Play("Lock Screen In");
            }
        }

        public void LockScreenAnimate()
        {
            if (hasPassword == true)
            {

                if (lockScreenBlur != null)
                    lockScreenBlur.BlurInAnim();

                lockScreen.Play("Lock Screen Password In");
            }

            else
            {
                if (lockScreenBlur != null)
                    lockScreenBlur.BlurOutAnim();

                lockScreen.Play("Lock Screen Out");
                desktopScreen.Play("Desktop In");
                onLogin.Invoke();
            }
        }

        public void Login()
        {
            if (lockScreenPassword.text == password)
            {
                lockScreen.Play("Lock Screen Password Out");
                desktopScreen.Play("Desktop In");
                onLogin.Invoke();
            }

            else if (lockScreenPassword.text != password && lockScreenErrorPopup != null)
                lockScreenErrorPopup.Play("Message Auto In");
        }
    }
}