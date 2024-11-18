using UnityEngine;
using TMPro;

namespace Michsky.DreamOS
{
    public class ShowPassword : MonoBehaviour
    {
        [Header("Resources")]
        public TMP_InputField inputField;

        public void TogglePassword()
        {
            if (inputField != null && inputField.contentType == TMP_InputField.ContentType.Standard)
            {
                inputField.contentType = TMP_InputField.ContentType.Password;
                inputField.ForceLabelUpdate();
            }

            else if (inputField != null && inputField.contentType == TMP_InputField.ContentType.Password)
            {
                inputField.contentType = TMP_InputField.ContentType.Standard;
                inputField.ForceLabelUpdate();
            }
        }

        public void HidePassword()
        {
            if (inputField != null && inputField.contentType == TMP_InputField.ContentType.Standard)
            {
                inputField.contentType = TMP_InputField.ContentType.Password;
                inputField.ForceLabelUpdate();
            }
        }
    }
}