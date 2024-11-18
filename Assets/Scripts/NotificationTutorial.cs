using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationTutorial : MonoBehaviour
{
    public MazecraftGameManager instance;
    string activeCameraName;
    public List<string> mainCameraTutorialItems = new List<string>();
    public List<string> cameraTwoTutorialItems = new List<string>();
    public List<string> moodscapeCameraTutorialItems = new List<string>();
    private int tutorialIndex = -1;

    void Start()
    {
        //activeCameraName = instance.activeCamera.name;
    }

    // Update is called once per frame
    void Update()
    {
        //if(activeCameraName != instance.activeCamera.name)
        //{
        //    //Debug.Log("tutorial index is: " + tutorialIndex);

        //    tutorialIndex = 0;
        //}

        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Tutorial index is: " + tutorialIndex);
            tutorialIndex = SetTutorialIndex(tutorialIndex);
            //Debug.Log("Now tutorial index is: " + tutorialIndex);

            RunTutorial(instance.activeCamera.name, tutorialIndex);
        }*/
    }

    /*private int SetTutorialIndex(int currentTutorialIndex)
    {
        int newTutorialIndex = currentTutorialIndex + 1;

        if (instance.activeCamera.name == "Main Camera")
        {
            //Debug.Log("From RunTutorial, mainCameraTutorialItems count is: " + mainCameraTutorialItems.Count);

            if (newTutorialIndex >= mainCameraTutorialItems.Count)
            {
                newTutorialIndex = 0;
            }
        }
        else if (instance.activeCamera.name == "CameraTwo")
        {
            //Debug.Log("From RunTutorial, cameraTwoTutorialItems count is: " + cameraTwoTutorialItems.Count);

            if (newTutorialIndex >= cameraTwoTutorialItems.Count)
            {
                newTutorialIndex = 0;
            }
        }
        else if (instance.activeCamera.name == "MoodscapeCamera")
        {
            //Debug.Log("From RunTutorial, moodscapeCameraTutorialItems count is: " + moodscapeCameraTutorialItems.Count);

            if (newTutorialIndex >= moodscapeCameraTutorialItems.Count)
            {
                //Debug.Log("Resetting tutorialIndex to zero.");
                newTutorialIndex = 0;
            }

            //Debug.Log("tutorialIndex is: " + tutorialIndex + ". Did it reset?");
        }

        return newTutorialIndex;
    }

    private void RunTutorial(string cameraName, int tutorialIndex)
    {
        //Debug.Log("From RunTutorial, tutorial index is: " + tutorialIndex);
        
        if (instance.activeCamera.name == "Main Camera")
        {
            //Debug.Log("From RunTutorial, mainCameraTutorialItems count is: " + mainCameraTutorialItems.Count);

            if (tutorialIndex >= mainCameraTutorialItems.Count)
            {
                tutorialIndex = 0;
            }

            instance.SetNotificationSettings(mainCameraTutorialItems[tutorialIndex], "", "", instance.GetNotificationStatus());
        }
        else if (instance.activeCamera.name == "CameraTwo")
        {
            //Debug.Log("From RunTutorial, cameraTwoTutorialItems count is: " + cameraTwoTutorialItems.Count);

            if (tutorialIndex >= cameraTwoTutorialItems.Count)
            {
                tutorialIndex = 0;
            }

            instance.SetNotificationSettings(cameraTwoTutorialItems[tutorialIndex], "", "", instance.GetNotificationStatus());
        }
        else if (instance.activeCamera.name == "MoodscapeCamera")
        {
            //Debug.Log("From RunTutorial, moodscapeCameraTutorialItems count is: " + moodscapeCameraTutorialItems.Count);

            if (tutorialIndex >= moodscapeCameraTutorialItems.Count)
            {
                //Debug.Log("Resetting tutorialIndex to zero.");
                tutorialIndex = 0;
            }

            //Debug.Log("tutorialIndex is: " + tutorialIndex + ". Did it reset?");

            instance.SetNotificationSettings(moodscapeCameraTutorialItems[tutorialIndex], "", "", instance.GetNotificationStatus());
        }

    }

    IEnumerator Wait()
    {
        //Print the time of when the function is first called.
        ////Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        ////Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }*/
}
