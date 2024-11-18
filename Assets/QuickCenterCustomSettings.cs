using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickCenterCustomSettings : MonoBehaviour
{
    public MazecraftGameManager instance;
    //string activeCameraName;
    public GameObject toggleTerrainItem;
    public GameObject toggleChaserGameItem;
    public GameObject addMazeItem;
    public GameObject destroyMazesItem;
    public GameObject changeMazeAppearanceItem;
    public GameObject dataFileItem;
    public GameObject topicItem;
    public GameObject changeCharacterAppearanceItem;
    public GameObject toggleWeatherItem;
    public GameObject weatherSettingsItem;
    public GameObject toggleRotatingMazesItem;
    public GameObject breathBallsMuteItem;
    public GameObject breathBallsPresetItem;
    public GameObject breathBallsRandomizeItem;
    public GameObject breathBallsStopItem;
    public GameObject goFishingItem;
    public GameObject CastItem;
    public GameObject ReelItem;
    public GameObject wallpaperRandomizerItem;

    void Start()
    {
        //activeCameraName = instance.activeCamera.name;
        //Debug.Log("Active camera name in Start of QuickCenter is: " + activeCameraName);
        SetQuickCenterOptions();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (activeCameraName != instance.activeCamera.name)
        {
            //Debug.Log("tutorial index is: " + tutorialIndex);

            instance.activeCamera = GameObject.Find("MoodscapeCamera");
            activeCameraName = instance.activeCamera.name;
            Debug.Log("Active camera name in Update of QuickCenter is: " + activeCameraName);
            SetQuickCenterOptions(instance.activeCamera.name);
        }*/
    }

    private void SetQuickCenterOptions()
    {
        /*if (instance.activeCamera.name == "Main Camera")
        {
            //Debug.Log("From RunTutorial, mainCameraTutorialItems count is: " + mainCameraTutorialItems.Count);
            toggleTerrainItem.SetActive(true);
            toggleChaserGameItem.SetActive(true);
            addMazeItem.SetActive(true);
            destroyMazesItem.SetActive(true);
            changeMazeAppearanceItem.SetActive(true);
            changeCharacterAppearanceItem.SetActive(true);
            dataFileItem.SetActive(true);
            topicItem.SetActive(true);
            toggleWeatherItem.SetActive(false);
            weatherSettingsItem.SetActive(false);
            toggleRotatingMazesItem.SetActive(true);
            breathBallsMuteItem.SetActive(false);
            breathBallsPresetItem.SetActive(false);
            breathBallsRandomizeItem.SetActive(false);
            breathBallsStopItem.SetActive(false);
            goFishingItem.SetActive(false);
            CastItem.SetActive(false);
            ReelItem.SetActive(false);
            wallpaperRandomizerItem.SetActive(false);
}
        else if (instance.activeCamera.name == "CameraTwo")
        {
            //Debug.Log("From RunTutorial, cameraTwoTutorialItems count is: " + cameraTwoTutorialItems.Count);
            toggleTerrainItem.SetActive(false);
            toggleChaserGameItem.SetActive(false);
            addMazeItem.SetActive(false);
            destroyMazesItem.SetActive(false);
            changeMazeAppearanceItem.SetActive(false);
            changeCharacterAppearanceItem.SetActive(false);
            dataFileItem.SetActive(true);
            topicItem.SetActive(true);
            toggleWeatherItem.SetActive(false);
            weatherSettingsItem.SetActive(false);
            toggleRotatingMazesItem.SetActive(false);
            breathBallsMuteItem.SetActive(false);
            breathBallsPresetItem.SetActive(false);
            breathBallsRandomizeItem.SetActive(false);
            breathBallsStopItem.SetActive(false);
            goFishingItem.SetActive(true);
            CastItem.SetActive(true);
            ReelItem.SetActive(true);
            wallpaperRandomizerItem.SetActive(false);
        }
        else if (instance.activeCamera.name == "MoodscapeCamera")
        {*/
        //Debug.Log("From RunTutorial, moodscapeCameraTutorialItems count is: " + moodscapeCameraTutorialItems.Count);
        toggleTerrainItem.SetActive(false);
        toggleChaserGameItem.SetActive(false);
        addMazeItem.SetActive(false);
        destroyMazesItem.SetActive(false);
        changeMazeAppearanceItem.SetActive(false);
        changeCharacterAppearanceItem.SetActive(false);
        dataFileItem.SetActive(false);
        topicItem.SetActive(false);
        toggleWeatherItem.SetActive(false);
        weatherSettingsItem.SetActive(true);
        toggleRotatingMazesItem.SetActive(false);
        breathBallsMuteItem.SetActive(false);
        breathBallsPresetItem.SetActive(false);
        breathBallsRandomizeItem.SetActive(false);
        breathBallsStopItem.SetActive(true);
        goFishingItem.SetActive(false);
        CastItem.SetActive(false);
        ReelItem.SetActive(false);
        wallpaperRandomizerItem.SetActive(true);

        //Debug.Log("tutorialIndex is: " + tutorialIndex + ". Did it reset?");
        //}
    }
}
