using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// currently this class is just doing an automatic timer that turns off the app after 30 minutes of
// inactivity.
public class Timer : MonoBehaviour
{
    MazecraftGameManager instance;
    public Slider sessionTimerSlider;
    public TextMeshProUGUI sliderTimerText;
    int sessionTime;
    float timeLeft = 36000.0f;
    private Vector3 prevMousePosition = Vector3.zero;

    void Start(){
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        sessionTime = Mathf.RoundToInt(timeLeft / 60);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        //Debug.Log(timeLeft);
        // divide by 60 to convert to minutes
        sessionTimerSlider.value = Mathf.RoundToInt(timeLeft / 60);

        if(timeLeft < 15 && timeLeft > 14.95f){
            instance.SetNotificationSettings("Goodbye!", "", "", true);
        }
    }

    public void SetSessionTime(float newTimeRemaining){
        // multiply by 60 to get number of seconds
        timeLeft = newTimeRemaining * 60;
        sliderTimerText.text = sessionTimerSlider.value.ToString();

        if(sessionTimerSlider.value == 5){
            instance.SetNotificationSettings("Five minutes left!", "", "", true);
        }

        if(sessionTimerSlider.value == 1){
            instance.SetNotificationSettings("One minute remaining...", "", "", true);
        }

        if (sessionTimerSlider.value == 0)
        {
            Application.Quit();
        }
    }
}