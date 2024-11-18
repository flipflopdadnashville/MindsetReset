using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/User/Set User Info")]
    public class SetUserInfo : MonoBehaviour
    {
        [Header("Resources")]
        public Button applyButton;

        [Header("Settings")]
        public UserManager userManagerScript;
        public Reference referenceType;

        [HideInInspector] public bool enableMessage;
        [HideInInspector] public Animator messageObject;
        [HideInInspector] public TextMeshProUGUI messageTextObject;
        [HideInInspector] public string messageText;
        [HideInInspector] public bool isRetype;
        [HideInInspector] public TMP_InputField retypeObject;

        // Reference objects
        private TMP_InputField firstNameObject;
        private TMP_InputField lastNameObject;
        private TMP_InputField securityHintObject;
        private TMP_InputField securityAnswerObject;
        private TMP_InputField passwordObject;
        private Image profilePictureObject;
        private Button profilePictureButton;
        Sprite tempSprite;

        public enum Reference
        {
            FIRST_NAME,
            LAST_NAME,
            SECURITY_HINT,
            SECURITY_ASNWER,
            PASSWORD,
            PROFILE_PICTURE
        }

        void Start()
        {
            // If attached object is first name, then set FirstName variable
            if (referenceType == Reference.FIRST_NAME)
            {
                firstNameObject = gameObject.GetComponent<TMP_InputField>();
                firstNameObject.characterLimit = userManagerScript.maxNameCharacter;
                applyButton.onClick.AddListener(SetFirstName);
            }

            // If attached object is last name, then set LastName variable
            else if (referenceType == Reference.LAST_NAME)
            {
                lastNameObject = gameObject.GetComponent<TMP_InputField>();
                lastNameObject.characterLimit = userManagerScript.maxNameCharacter;
                applyButton.onClick.AddListener(SetLastName);
            }

            // If attached object is security hint, then set SecurityQuestionHint variable
            else if (referenceType == Reference.SECURITY_HINT)
            {
                securityHintObject = gameObject.GetComponent<TMP_InputField>();
                securityHintObject.characterLimit = userManagerScript.maxSecurityCharacter;
                applyButton.onClick.AddListener(SetSecurityHint);
            }

            // If attached object is security question answer, then set SecurityAnswer variable
            else if (referenceType == Reference.SECURITY_ASNWER)
            {
                securityAnswerObject = gameObject.GetComponent<TMP_InputField>();
                securityAnswerObject.characterLimit = userManagerScript.maxSecurityCharacter;
                applyButton.onClick.AddListener(SetSecurityAnswer);
            }

            // If attached object is password, then set Password variable
            else if (referenceType == Reference.PASSWORD)
            {
                passwordObject = gameObject.GetComponent<TMP_InputField>();
                passwordObject.characterLimit = userManagerScript.maxPasswordCharacter;
                applyButton.onClick.AddListener(SetPassword);
            }

            // If attached object is profile picture, then set Profile Picture variable
            else if (referenceType == Reference.PROFILE_PICTURE)
            {
                profilePictureObject = gameObject.GetComponent<Image>();
                applyButton.onClick.AddListener(SetProfilePicture);
            }
        }

        public void SetFirstName()
        {
            // Set First Name variable
            userManagerScript.firstName = firstNameObject.text;
        }

        public void ChangeFirstName(string name)
        {
            // Set First Name variable
            userManagerScript.firstName = name;
        }

        public void SetLastName()
        {
            // Set Last Name variable
            userManagerScript.lastName = lastNameObject.text;
        }

        public void ChangeLastName(string name)
        {
            // Set Last Name variable
            userManagerScript.lastName = name;
        }

        public void SetSecurityHint()
        {
            // Set Security Hint variable
            userManagerScript.securityHint = securityHintObject.text;
        }

        public void SetSecurityAnswer()
        {
            // Set Security Answer variable - if it's null, then change HasSecurityAnswer boolean to false
            if (securityAnswerObject.text == null)
                userManagerScript.hasSecurityAnswer = false;

            // If it's not, then change HasSecurityAnswer boolean to true
            else
            {
                userManagerScript.securityAnswer = securityAnswerObject.text;
                userManagerScript.hasSecurityAnswer = true;
            }
        }

        public void SetPassword()
        {
            // Set Password variable - if it's null, then change HasPassword boolean to false
            if (passwordObject.text == null || passwordObject.text == "")
                userManagerScript.hasPassword = false;

            // If it's not, then change HasPassword boolean to true
            else
            {
                userManagerScript.password = passwordObject.text;
                userManagerScript.hasPassword = true;
            }
        }

        public void ChangePassword(string password)
        {
            // Set Password variable - if it's null, then change HasPassword boolean to false
            if (password == null || password == "")
                userManagerScript.hasPassword = false;

            // If it's not, then change HasPassword boolean to true
            else
            {
                userManagerScript.password = password;
                userManagerScript.hasPassword = true;
            }
        }

        public void SetProfilePicture()
        {
            // Set Profile Picture
            userManagerScript.profilePicture = profilePictureObject.sprite;

            // Save name to prefs
            PlayerPrefs.SetString("ProfilePicture", "Profile Pictures/" + profilePictureObject.GetComponent<Image>().sprite.name);
        }

        public void ChangeProfilePicture(Sprite spriteVar)
        {
            // Set Profile Picture
            userManagerScript.profilePicture = spriteVar;

            // Save name to prefs
            PlayerPrefs.SetString("ProfilePicture", "Profile Pictures/" + spriteVar.name);
        }

        void CheckName()
        {
            // If everything is OK about names, then enable Apply Button
            if (userManagerScript.nameOK == true && userManagerScript.lastNameOK == true)
            {
                applyButton.interactable = true;
                messageObject.Play("Message Out");
            }

            // If not, keep it disabled
            else
            {
                applyButton.interactable = false;
                messageTextObject.gameObject.SetActive(false);
                messageTextObject.gameObject.SetActive(true);
                messageObject.Play("Message In");
            }

            // If FirstNameObject meets the requirements, set NameOK to true
            if (referenceType == Reference.FIRST_NAME
               && firstNameObject.text.Length >= userManagerScript.minNameCharacter 
               && firstNameObject.text.Length <= userManagerScript.maxNameCharacter)
            {
                userManagerScript.nameOK = true;
            }

            // If not, set NameOK to false
            else
            {
                userManagerScript.nameOK = false;
                messageTextObject.text = messageText;
            }
        }

        void CheckLastName()
        {
            // If LastNameObject meets the requirements, set LastNameOK to true
            if (referenceType == Reference.LAST_NAME 
                && lastNameObject.text.Length >= userManagerScript.minNameCharacter 
                && lastNameObject.text.Length <= userManagerScript.maxNameCharacter)
            {
                userManagerScript.lastNameOK = true;
            }

            // If not, set LastNameOK to false
            else
            {
                userManagerScript.lastNameOK = false;
                messageTextObject.text = messageText;
            }
        }

        void CheckPassword()
        {
            // If everything is OK about password, then enable Apply Button
            if (userManagerScript.passwordOK == true && userManagerScript.passwordRetypeOK == true)
            {
                applyButton.interactable = true;
                messageObject.Play("Message Out");
            }

            // If not, keep it disabled
            else
            {
                applyButton.interactable = false;
                messageTextObject.gameObject.SetActive(false);
                messageTextObject.gameObject.SetActive(true);
                messageObject.Play("Message In");
            }

            // If PasswordObject meets the requirements, set PasswordOK to true
            if (passwordObject.text.Length >= userManagerScript.minPasswordCharacter && passwordObject.text.Length <= userManagerScript.maxPasswordCharacter || passwordObject.text.Length == 0)
                userManagerScript.passwordOK = true;

            // If not, set PasswordOK to false
            else
            {
                userManagerScript.passwordOK = false;
                messageTextObject.text = messageText;
            }

            // If ReType feature is enabled and it's lower than min character size, keep it clear
            if (isRetype == true && retypeObject.text.Length <= userManagerScript.minPasswordCharacter - 1)
                passwordObject.text = null;

            // If ReType feature is enabled and equals 0, keep ReType field disabled
            if (isRetype == true && retypeObject.text.Length == 0)
            {
                passwordObject.interactable = false;
                messageTextObject.text = messageText;
            }

            // If not, keep ReType field enabled
            else if (isRetype == true && retypeObject.text.Length >= userManagerScript.minPasswordCharacter)
            {
                passwordObject.interactable = true;
            }

            // If ReType and Password field doesn't contain the same characters, keep PasswordOK false
            if (isRetype == true && passwordObject.text != retypeObject.text)
                userManagerScript.passwordRetypeOK = false;

            // If ReType and Password field does contain the same characters, keep PasswordOK true
            else if (isRetype == true && passwordObject.text == retypeObject.text)
            {
                userManagerScript.passwordRetypeOK = true;
            }
        }

        void Update()
        {
            // If selected object is password, then update CheckPassword function
            if (referenceType == Reference.PASSWORD)
                CheckPassword();
            // If selected object is first name, then update CheckName function
            else if (referenceType == Reference.FIRST_NAME)
                CheckName();
            // If selected object is last name, then update CheckLastName function
            else if (referenceType == Reference.LAST_NAME)
                CheckLastName();
        }
    }
}