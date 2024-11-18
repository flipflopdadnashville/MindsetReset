using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraAudioController : MonoBehaviour
{
    public AudioSource mainCameraAudioSource;
    
    private void Awake() {
        mainCameraAudioSource.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.A)){
            mainCameraAudioSource.enabled = !mainCameraAudioSource.enabled;
        }
    }

    public void SetVolume(float volume){
        //Debug.Log("volume is: " + volume);
        if(mainCameraAudioSource.enabled == false){
            mainCameraAudioSource.enabled = true;
        }
        
        mainCameraAudioSource.volume = (float)volume;
    }
}
