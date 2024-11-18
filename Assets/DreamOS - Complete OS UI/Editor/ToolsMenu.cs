#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.DreamOS
{
    public class ContextMenu : MonoBehaviour
    {
        [MenuItem("Tools/DreamOS/Create World Space Resources", false, 12)]
        static void CreateWorldSpaceResources()
        {
            try
            {
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/" 
                    + EditorPrefs.GetString("DreamOS.ObjectCreator.RootFolder")
                    + "World Space/World Space Resources" 
                    + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                clone.name = clone.name.Replace("(Clone)", "").Trim();

                if (Application.isPlaying == false)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            catch
            {
                if (EditorUtility.DisplayDialog("DreamOS", "Cannot create the resources due to missing/incorrect root folder. " +
                  "You can change the root folder by clicking 'Fix' button and enabling 'Change Root Folder'.", "Fix", "Cancel"))
                    ShowManager();
            }
        }

        [MenuItem("Tools/DreamOS/Show Chat List")]
        static void ShowChatList()
        {
            Selection.activeObject = Resources.Load("Chats/Example Chat");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Example Chat'. Make sure you have 'Example Chat' asset in Resources/Chats folder.");
        }

        [MenuItem("Tools/DreamOS/Show Game Hub List")]
        static void ShowGameHubList()
        {
            Selection.activeObject = Resources.Load("Game Hub/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Game Hub folder.");
        }

        [MenuItem("Tools/DreamOS/Show Music Playlists")]
        static void ShowMusicLibrary()
        {
            Selection.activeObject = Resources.Load("Music Player/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Music Player folder.");
        }

        [MenuItem("Tools/DreamOS/Show Notepad Library")]
        static void ShowNotepadLibrary()
        {
            Selection.activeObject = Resources.Load("Notepad/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Notepad folder.");
        }

        [MenuItem("Tools/DreamOS/Show Photo Library")]
        static void ShowPhotoLibrary()
        {
            Selection.activeObject = Resources.Load("Photo Gallery/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Gallery folder.");
        }

        [MenuItem("Tools/DreamOS/Show Theme Manager")]
        static void ShowManager()
        {
            Selection.activeObject = Resources.Load("Theme/Theme Manager");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Theme Manager'. Make sure you have 'Theme Manager' asset in Resources folder. " +
                    "You can create a new Theme Manager asset or re-import the pack if you can't see the file.");
        }

        [MenuItem("Tools/DreamOS/Show Video Library")]
        static void ShowVideoLibrary()
        {
            Selection.activeObject = Resources.Load("Video Player/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Video Player folder.");
        }

        [MenuItem("Tools/DreamOS/Show Web Library")]
        static void ShowWebLibrary()
        {
            Selection.activeObject = Resources.Load("Web Browser/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Web Browser folder.");
        }

        [MenuItem("Tools/DreamOS/Show Widget Library")]
        static void ShowWidgetLibrary()
        {
            Selection.activeObject = Resources.Load("Widgets/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Widgets folder.");
        }
    }
}
#endif