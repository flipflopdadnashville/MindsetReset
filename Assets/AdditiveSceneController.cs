using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneController : MonoBehaviour
{
    public List<string> availableScenes = new List<string>();
    int i = 0;
    string currentSceneName;

    void Start(){
        this.gameObject.GetComponent<AdditiveSceneController>().enabled = false;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 30, 150, 30), "Home"))
        {
            //The SceneManager loads your new Scene as a single Scene (not overlapping). This is Single mode.
            foreach(string sceneName in availableScenes){
                if(sceneName != "ThoughtBlocker"){
                    try{
                        SceneManager.UnloadSceneAsync(sceneName);
                    }
                    catch(Exception ex){
                        // do nothing with the exception
                    }
                }
            }
            Resources.UnloadUnusedAssets();
        }

        if (GUI.Button(new Rect(20, 60, 150, 30), "NextScene"))
        {
            // Get a count of the open scenes
            int openSceneCount = SceneManager.sceneCount;
            //Debug.Log("Open scene count is: " + openSceneCount);

            if(openSceneCount < 2){

                Scene scene = SceneManager.GetActiveScene();
                if(i == availableScenes.Count - 1){
                    i = 0;
                }
                else{
                    i = i + 1;
                }

                currentSceneName = availableScenes[i];
                SceneManager.LoadScene(currentSceneName, LoadSceneMode.Additive);
            }
        }
    }
}
