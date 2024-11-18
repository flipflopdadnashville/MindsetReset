using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class AudioLinkDemo : MonoBehaviour {

    public MazecraftGameManager instance;
    public AudioSource src;
    public ProceduralSignalGenerator psg;
    public AudioClip clip;
    public DHGSignalGenerator mySG;

    // Based on the AudioClip Demo in Unity 
    // https://docs.unity3d.com/ScriptReference/AudioClip.Create.html

    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 32;

    public void ValueChangeCheck()
    {
        if (psg.selectedSignal.name == this.gameObject.name)
        {
            if (psg.selectedSignal.name == "signal_0")
            {
                src.volume = instance.volumeSlider.value;
                frequency = float.Parse(instance.frequencyText.text, CultureInfo.InvariantCulture.NumberFormat);
                psg.sliderValueOne.text = instance.volumeSlider.value.ToString("0.0");
                instance.frequencySlider.value = float.Parse(instance.frequencyText.text, CultureInfo.InvariantCulture.NumberFormat);
                psg.frequencyValueOne.text = instance.frequencyText.text;
                //src.panStereo = instance.volumeSlider.value;
            }
            else if(psg.selectedSignal.name == "signal_1")
            {
                src.volume = instance.volumeSliderTwo.value;
                frequency = float.Parse(instance.frequencyTextTwo.text, CultureInfo.InvariantCulture.NumberFormat);
                psg.sliderValueTwo.text = instance.volumeSliderTwo.value.ToString("0.0");
                instance.frequencySliderTwo.value = float.Parse(instance.frequencyTextTwo.text, CultureInfo.InvariantCulture.NumberFormat);
                psg.frequencyValueTwo.text = instance.frequencySliderTwo.value.ToString("0");
                //src.panStereo = instance.volumeSliderTwo.value * -1f;
            }
            else if (psg.selectedSignal.name == "signal_2")
            {
                src.volume = instance.volumeSliderThree.value;
                frequency = instance.frequencySliderThree.value;
                psg.sliderValueThree.text = instance.volumeSliderThree.value.ToString("0.0");
                psg.frequencyValueThree.text = instance.frequencySliderThree.value.ToString("0");
                //src.panStereo = instance.volumeSliderThree.value;
            }
            else if (psg.selectedSignal.name == "signal_3")
            {
                src.volume = instance.volumeSliderFour.value;
                frequency = instance.frequencySliderFour.value;
                psg.sliderValueFour.text = instance.volumeSliderFour.value.ToString("0.0");
                psg.frequencyValueFour.text = instance.frequencySliderFour.value.ToString("0");
                //src.panStereo = instance.volumeSliderFour.value * -1f;
            }
        }
    }

    // Use this for initialization
    void Start () {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();

        instance.volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencySlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencyText.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.volumeSliderTwo.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencySliderTwo.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencyTextTwo.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.volumeSliderThree.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencySliderThree.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.volumeSliderFour.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        instance.frequencySliderFour.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        SetPan();
    }

    public void EnableSignal(GameObject goSignal){
        psg = GameObject.Find("SoundscapeHolder").GetComponent<ProceduralSignalGenerator>();
        //Debug.Log("number of signals created is: " + psg.numberOfSignalsCreated);

        frequency = UnityEngine.Random.Range(32,440);
        if(frequency % 2 != 0){
            frequency++;
        }

        position = 0;
        mySG = goSignal.GetComponent<DHGSignalGenerator>();
        src = goSignal.GetComponent<AudioSource>();
        
        clip = AudioClip.Create("Signal Demo_" + psg.numberOfSignalsCreated, samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        
        src.loop = true; 
        src.clip = clip;
        src.volume = 0f;
        // if(psg.numberOfSignalsCreated % 2 != 0){
        //     src.panStereo = -.5f;
        // }
        // else if(psg.numberOfSignalsCreated % 2 == 0){
        //     src.panStereo = .5f;
        // }
        // else if(psg.numberOfSignalsCreated == 1){
        //     src.panStereo = 0f;
        // }

        src.Play();
    }


    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = mySG.GetSignal(frequency * position / samplerate);
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }

    // Update is called once per frame
    void Update () {
       CheckKey();
	}

    void CheckKey(){
        if(psg.selectedSignal.name == this.gameObject.name){
            //Volume
            if(Input.GetKey(KeyCode.Period)){
                ChangeVolume(.005f);
            } 

            if(Input.GetKey(KeyCode.Comma)){
                ChangeVolume(-.005f); 
            }

            //Frequency
            if(Input.GetKey(KeyCode.LeftBracket)){
                ChangeFrequency(-.5f);  
            } 

            if(Input.GetKey(KeyCode.RightBracket)){
                ChangeFrequency(.5f);  
            }

            if(Input.GetKeyUp(KeyCode.LeftBracket) || Input.GetKeyUp(KeyCode.RightBracket)){
                frequency = Mathf.RoundToInt(frequency);
                instance.SetNotificationSettings("Tone Frequency Set to: " + frequency, "", "", true); 
            }
        }
    }

    public void ChangeVolume(float amountToChange){
        src.volume = src.volume + amountToChange;  
    }

    public void ChangeFrequency(float amountToChange){
        frequency = frequency + amountToChange; 
    }

    public void SetPan(){
        //src.panStereo = src.panStereo + amountToChange; 
        
        if (this.gameObject.name == "signal_0")
        {
            src.panStereo = 1f;
        }
        else if (this.gameObject.name == "signal_1")
        {
            src.panStereo = -1f;
        }
        else if (this.gameObject.name == "signal_2")
        {
            src.panStereo = .8f;
        }
        else if (this.gameObject.name == "signal_3")
        {
            src.panStereo = -.8f;
        }
    }

}
