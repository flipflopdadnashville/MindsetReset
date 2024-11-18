using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    [AddComponentMenu("DreamOS/User/Get User Info")]
    public class GetUserInfo : MonoBehaviour
    {
        [Header("Settings")]
        public Reference getInformation;
        public bool getInfoAtStart;

        TextMeshProUGUI textObject;
        Image imageObject;

        // Information variables
        string firstName;
        string lastName;
        string password;
        string securityHint;
        string securityAnswer;

        // Reference list
        public enum Reference
        {
            FULL_NAME,
            FIRST_NAME,
            LAST_NAME,
            SECURITY_HINT,
            SECURITY_ASNWER,
            PASSWORD,
            PROFILE_PICTURE
        }

        void OnEnable()
        {
            // If it's true, then get the requested information at start
            if (getInfoAtStart == true)
                GetInformation();
        }

        public void GetInformation()
        {
            // If attached object is full name, then get FirstName and LastName value
            if (getInformation == Reference.FULL_NAME)
            {
                firstName = PlayerPrefs.GetString("FirstName");
                lastName = PlayerPrefs.GetString("LastName");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = firstName + " " + lastName;
            }

            // If attached object is first name, then get FirstName value
            else if (getInformation == Reference.FIRST_NAME)
            {
                firstName = PlayerPrefs.GetString("FirstName");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = firstName;
            }

            // If attached object is last name, then get LastName value
            else if (getInformation == Reference.LAST_NAME)
            {
                lastName = PlayerPrefs.GetString("LastName");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = lastName;
            }

            // If attached object is security hint, then get SecurityQuestionHint value
            else if (getInformation == Reference.SECURITY_HINT)
            {
                securityHint = PlayerPrefs.GetString("SecurityQuestionHint");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = securityHint;
            }

            // If attached object is security question answer, then get SecurityAnswer value
            else if (getInformation == Reference.SECURITY_ASNWER)
            {
                securityAnswer = PlayerPrefs.GetString("SecurityAnswer");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = securityAnswer;
            }

            // If attached object is password, then get Password value
            else if (getInformation == Reference.PASSWORD)
            {
                password = PlayerPrefs.GetString("Password");
                textObject = gameObject.GetComponent<TextMeshProUGUI>();
                textObject.text = password;
            }

            // If attached object is Picture, then load picture
            else if (getInformation == Reference.PROFILE_PICTURE && gameObject.activeInHierarchy == true)
            {
                StartCoroutine("WaitForProcessPP");
            }       
        }

        IEnumerator WaitForProcessPP()
        {
            yield return new WaitForSeconds(0.1f);
            imageObject = gameObject.GetComponent<Image>();
            imageObject.sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("ProfilePicture"));
            StopCoroutine("WaitForProcessPP");
        }
    }
}