using DigitalRuby.SimpleLUT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoAlarm : MonoBehaviour
{
    SimpleLUT lightController;
    VolumeControl volumeController;
    public int minutesBeforeSleeping = 1;
    float timeLeftBeforeWaking = 60.0f; // multiply this by 100
    public int wakeupThrottle = 65;
    public bool setPseudoAlarm =false;
    private bool pseudoAlarmSet = false;

    // Start is called before the first frame update
    void Start()
    {
        lightController = GameObject.Find("MoodscapeCamera").GetComponent<SimpleLUT>();
        volumeController = GameObject.Find("channelZero").GetComponent<VolumeControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(setPseudoAlarm == true && pseudoAlarmSet == false){
            SetPseudoAlarm();
        }

        if(setPseudoAlarm == true && pseudoAlarmSet == true){
            timeLeftBeforeWaking -= Time.deltaTime;
        }

        if(timeLeftBeforeWaking < 0){
            setPseudoAlarm = false;
            SetPseudoAlarm();
            timeLeftBeforeWaking = 60.0f;
        }
    }

    public void SetPseudoAlarm(){
        pseudoAlarmSet = !pseudoAlarmSet;

        if(pseudoAlarmSet == true){
            GoToSleep();
        }
        else if(pseudoAlarmSet == false){
            WakeUp();
        }
    }

    public void GoToSleep(){
        // need to slowly turn down all the volume and dim the screen
        Debug.Log("Time to go to sleep");
        foreach(GameObject channel in volumeController.channels){
            StartCoroutine(volumeController.LerpPosition(channel.transform, new Vector3(UnityEngine.Random.Range(-650,650), 0, -500), minutesBeforeSleeping * 60));
        }

        StartCoroutine(lightController.LerpBrightness(lightController.Brightness, -1, false, minutesBeforeSleeping * 60));
    }

    public void WakeUp(){
        Debug.Log("Wakey, wakey!");
        foreach(GameObject channel in volumeController.channels){
            StartCoroutine(volumeController.LerpPosition(channel.transform, new Vector3(UnityEngine.Random.Range(-650,650), UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(250,550)), wakeupThrottle));
        }
        
        StartCoroutine(lightController.LerpBrightness(lightController.Brightness, 0, true, wakeupThrottle * 100));
    }
}
