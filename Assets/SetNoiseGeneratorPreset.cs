using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SetNoiseGeneratorPreset : MonoBehaviour
{
    public MazecraftGameManager instance;
    private int i = 0;
    public CSVReader csv;
    public VolumeControl channelController;
    public GameObject channelZero;
    public GameObject channelOne;
    public GameObject channelTwo;
    public GameObject channelThree;
    public GameObject channelFour;
    public GameObject channelFive;
    public GameObject channelSix;
    public GameObject channelSeven;
    public GameObject channelEight;
    public GameObject channelNine;
    public bool presetOn = false;
    public float changePresetInterval = 60f;


    void Start(){
        // SetNoiseGeneratorPreset[] components = GameObject.FindObjectsOfType<SetNoiseGeneratorPreset>();
        
        // foreach(SetNoiseGeneratorPreset comp in components){
        //     //components.Add(comp.gameObject);
        //     //Debug.Log("SetNoiseGeneratorPreset is attached: " + comp.gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if (presetOn == false)
            {
                presetOn = true;
                // disable random EQ if needed
                // if(channelController.randomEQ == true){
                //     channelController.ToggleRandomEQ();
                // }

                // Stop the motion and momentum
                channelController.StopMotion();

                // set the preset
                InvokeRepeating("SetEQPreset", .1f, changePresetInterval);
            }
            else
            {
                presetOn = false;
                CancelInvoke();
            }
        }
    }

    public void SetEQPreset(){
        foreach(GameObject channel in channelController.channels){
            var random = new System.Random();
            bool setToZero = random.Next(2) == 1;
            if(setToZero == true){
                StartCoroutine(LerpPosition(channel.transform,new Vector3(UnityEngine.Random.Range(-650,650), UnityEngine.Random.Range(-100,500), UnityEngine.Random.Range(-200,300)), 5));
            }
            
            if(setToZero == false){
                //Debug.Log("Screen height is: " + Screen.height);
                StartCoroutine(LerpPosition(channel.transform,new Vector3(UnityEngine.Random.Range(-650,650), 0, -Screen.height / 3f), 5)); 
            }
        }
    }

    IEnumerator LerpPosition(Transform channelTransform, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = channelTransform.position;
        while (time < duration)
        {
            channelTransform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        channelTransform.position = targetPosition;
        channelController.StopMotion();
    }
}
