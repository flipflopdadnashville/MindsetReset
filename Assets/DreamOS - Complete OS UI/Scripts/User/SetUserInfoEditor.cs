using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
namespace Michsky.DreamOS
{
    [CustomEditor(typeof(SetUserInfo))]
    public class SetUserInfoEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var SetUserInfo = target as SetUserInfo;

            SetUserInfo.enableMessage = EditorGUILayout.Toggle("Enable Error Message", SetUserInfo.enableMessage);
            using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(SetUserInfo.enableMessage)))
            {
                if (group.visible == true)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PrefixLabel("Message object:");
                    SetUserInfo.messageObject = EditorGUILayout.ObjectField(SetUserInfo.messageObject, typeof(Animator), true) as Animator;
                    EditorGUILayout.PrefixLabel("Message text object:");
                    SetUserInfo.messageTextObject = EditorGUILayout.ObjectField(SetUserInfo.messageTextObject, typeof(TMPro.TextMeshProUGUI), true) as TMPro.TextMeshProUGUI;
                    EditorGUILayout.PrefixLabel("Message text:");
                    SetUserInfo.messageText = EditorGUILayout.TextField(SetUserInfo.messageText) as string;
                    EditorGUI.indentLevel--;
                }
            }

            SetUserInfo.isRetype = EditorGUILayout.Toggle("Is Retype", SetUserInfo.isRetype);
            using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(SetUserInfo.isRetype)))
            {
                if (group.visible == true)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PrefixLabel("Retype of:");
                    SetUserInfo.retypeObject = EditorGUILayout.ObjectField(SetUserInfo.retypeObject, typeof(TMPro.TMP_InputField), true) as TMPro.TMP_InputField;
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
#endif