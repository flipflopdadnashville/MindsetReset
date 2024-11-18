#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.DreamOS
{
    public class CreateMenu : Editor
    {
        static void CreateObject(string resourcePath)
        {
            try
            {
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/"
                    + EditorPrefs.GetString("DreamOS.ObjectCreator.RootFolder") + resourcePath
                    + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;

                try
                {
                    if (Selection.activeGameObject == null)
                    {
                        var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                        Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                        clone.transform.SetParent(canvas.transform, false);
                    }

                    else
                    {
                        Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                        clone.transform.SetParent(Selection.activeGameObject.transform, false);
                    }

                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                }

                catch
                {
                    Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                    CreateCanvas();
                    var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                    clone.transform.SetParent(canvas.transform, false);
                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                }

                Selection.activeObject = clone;
            }

            catch
            {
                if (EditorUtility.DisplayDialog("DreamOS", "Cannot create the object due to missing/incorrect root folder. " +
                    "You can change the root folder by clicking 'Fix' button and enabling 'Change Root Folder'.", "Fix", "Cancel"))
                    ShowManager();
            }

            if (Application.isPlaying == false)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("GameObject/DreamOS/Canvas", false, -1)]
        static void CreateCanvas()
        {
            try
            {
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/"
                    + EditorPrefs.GetString("DreamOS.ObjectCreator.RootFolder")
                    + "UI Elements/Other/Canvas" + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                clone.name = clone.name.Replace("(Clone)", "").Trim();

                if (Application.isPlaying == false)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                Selection.activeObject = clone;
            }

            catch
            {
                if (EditorUtility.DisplayDialog("DreamOS", "Cannot create the object due to missing/incorrect root folder. " +
                  "You can change the root folder by clicking 'Fix' button and enabling 'Change Root Folder'.", "Fix", "Cancel"))
                    ShowManager();
            }
        }

        static void ShowManager()
        {
            Selection.activeObject = Resources.Load("Theme/Theme Manager");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Theme Manager'. Make sure you have 'Theme Manager' asset in Resources folder. " +
                    "You can create a new Theme Manager asset or re-import the pack if you can't see the file.");
        }

        #region Button
        [MenuItem("GameObject/DreamOS/Button/Desktop Button", false, 0)]
        static void DesktopButton()
        {
            CreateObject("UI Elements/Button/Desktop Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button", false, 0)]
        static void MainButton()
        {
            CreateObject("UI Elements/Button/Main Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button (Only Icon)", false, 0)]
        static void MainButtonOnlyIcon()
        {
            CreateObject("UI Elements/Button/Main Button (Only Icon)");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button (With Icon)", false, 0)]
        static void MainButtonWithIcon()
        {
            CreateObject("UI Elements/Button/Main Button (With Icon)");
        }

        [MenuItem("GameObject/DreamOS/Button/Nav Drawer Button", false, 0)]
        static void NavDrawerButton()
        {
            CreateObject("UI Elements/Button/Nav Drawer Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Picture Selection Button", false, 0)]
        static void PictureSelectionButton()
        {
            CreateObject("UI Elements/Button/Picture Selection Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Quick Center App Button", false, 0)]
        static void QuickCenterAppButton()
        {
            CreateObject("UI Elements/Button/Quick Center App Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Task Bar Button", false, 0)]
        static void TaskBarButton()
        {
            CreateObject("UI Elements/Button/Task Bar Button");
        }
        #endregion

        #region Horizontal Selector
        [MenuItem("GameObject/DreamOS/Horizontal Selector/Standard", false, 0)]
        static void HorizontalSelector()
        {
            CreateObject("UI Elements/Horizontal Selector/Horizontal Selector");
        }
        #endregion

        #region Input Field
        [MenuItem("GameObject/DreamOS/Input Field/Fading Input Field", false, 0)]
        static void FadingInputField()
        {
            CreateObject("UI Elements/Input Field/Fading Input Field");
        }

        [MenuItem("GameObject/DreamOS/Input Field/Standard Input Field", false, 0)]
        static void StandardInputField()
        {
            CreateObject("UI Elements/Input Field/Standard Input Field");
        }
        #endregion

        #region Loader
        [MenuItem("GameObject/DreamOS/Loader/Material Spinner", false, 0)]
        static void LoaderMaterial()
        {
            CreateObject("UI Elements/Loader/Material Spinner");
        }
        #endregion

        #region Modal Window
        [MenuItem("GameObject/DreamOS/Modal Window/Standard", false, 0)]
        static void ModalWindow()
        {
            CreateObject("UI Elements/Modal Window/Standard Modal Window");
        }
        #endregion

        #region Scrollbar
        [MenuItem("GameObject/DreamOS/Scrollbar/Standard", false, 0)]
        static void Scrollbar()
        {
            CreateObject("UI Elements/Scrollbar/Scrollbar");
        }
        #endregion

        #region Slider
        [MenuItem("GameObject/DreamOS/Slider/Standard", false, 0)]
        static void Slider()
        {
            CreateObject("UI Elements/Slider/Slider");
        }
        #endregion

        #region Switch
        [MenuItem("GameObject/DreamOS/Switch/Standard", false, 0)]
        static void Switch()
        {
            CreateObject("UI Elements/Switch/Switch");
        }
        #endregion

        #region Vertical Selector
        [MenuItem("GameObject/DreamOS/Vertical Selector/Standard", false, 0)]
        static void VerticalSelector()
        {
            CreateObject("UI Elements/Vertical Selector/Vertical Selector");
        }
        #endregion
    }
}
#endif