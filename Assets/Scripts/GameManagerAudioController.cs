using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerAudioController : MonoBehaviour
{
    private MazecraftGameManager instance;
    public AudioSource mainCameraAudioSource;
    public GameObject soundscapeHolder;

    private void Awake() {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        mainCameraAudioSource.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey (KeyCode.LeftArrow)) { 
            mainCameraAudioSource.panStereo -= .01f;
         }

         if (Input.GetKey (KeyCode.RightArrow)) { 
            mainCameraAudioSource.panStereo += .005f;
         }


    //   if(Input.GetKeyDown(KeyCode.A)){
    //         mainCameraAudioSource.enabled = !mainCameraAudioSource.enabled;
    //     }
    }

    public void SetVolume(float volume){
        //Debug.Log("volume is: " + volume);
        if(mainCameraAudioSource.enabled == false){
            mainCameraAudioSource.enabled = true;
        }
        
        mainCameraAudioSource.volume = (float)volume;
    }

    public void ChangeToneState(){
        //Debug.Log("Hit ChangeToneState");
        soundscapeHolder.GetComponent<ProceduralTone>().enabled = !soundscapeHolder.GetComponent<ProceduralTone>().enabled;
    }
}
