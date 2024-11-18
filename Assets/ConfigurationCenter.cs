using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfigurationCenter : MonoBehaviour
{
    MazecraftGameManager instance;
    //moodscapeCameraOrbit moodscapeCameraOrbit;
    ChangeWallpaperShader backgroundRandomizer;
    [Header("Breath Settings")]
    [SerializeField]
    int breathSpeedIn;
    [SerializeField]
    int breathSpeedOut;
    [SerializeField]
    int breathOutRetention;
    public TMP_InputField breathIn;
    public TMP_InputField breathOut;
    public TMP_InputField breathHold;
    //[Header("Camera Settings")]
    //[SerializeField]
    //bool moodscapeCameraAutoZoom = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        //moodscapeCameraOrbit = GameObject.Find("MoodscapeCamera").GetComponent<moodscapeCameraOrbit>();
        breathSpeedIn = instance.breathSpeedIn;
        breathIn.text = instance.breathSpeedIn.ToString();
        breathSpeedOut = instance.breathSpeedOut;
        breathOut.text = instance.breathSpeedOut.ToString();
        breathOutRetention = instance.breathOutRetention;
        breathHold.text = instance.breathOutRetention.ToString();
        
        //moodscapeCameraAutoZoom = moodscapeCameraOrbit.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        //instance.breathSpeedIn = breathSpeedIn;
        //instance.breathSpeedOut = breathSpeedOut;
        //instance.breathOutRetention = breathOutRetention;
        //moodscapeCameraOrbit.enabled = moodscapeCameraAutoZoom;
    }

    public void SetBreathInSpeed()
    {
        breathSpeedIn = int.Parse(breathIn.text);
        instance.breathSpeedIn = breathSpeedIn;
    }

    public void SetBreathOutSpeed()
    {
        breathSpeedOut = int.Parse(breathOut.text);
        instance.breathSpeedOut = breathSpeedOut;
    }

    public void SetBreathHoldSpeed()
    {
        breathOutRetention = int.Parse(breathHold.text);
        instance.breathOutRetention = breathOutRetention;
    }
}
