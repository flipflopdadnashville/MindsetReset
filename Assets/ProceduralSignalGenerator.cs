using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralSignalGenerator : MonoBehaviour {

    //public AudioSource src;
    //public ProceduralSignalGenerator psg;
    AudioLinkDemo ald;
    public GameObject soundscapeHolder;
    public Text sliderValueOne;
    public Text sliderValueTwo;
    public Text sliderValueThree;
    public Text sliderValueFour;
    public Text frequencyValueOne;
    public Text frequencyValueTwo;
    public Text frequencyValueThree;
    public Text frequencyValueFour;
    //public AudioClip clip;
    //public DHGSignalGenerator mySG;
    public int numberOfSignalsToCreate = 1;
    GameObject goSignal;
    public int numberOfSignalsCreated = 0;
    public List<GameObject> signals = new List<GameObject>();
    public GameObject selectedSignal;
    int i = 0;

    // Based on the AudioClip Demo in Unity 
    // https://docs.unity3d.com/ScriptReference/AudioClip.Create.html

    // Use this for initialization
    void Start () {  
              
        for(int i = 0; i < numberOfSignalsToCreate; i++){            
            //spawn object
            numberOfSignalsCreated++;
            CreateSignal(i);
        }

        //Debug.Log("signals count is: " + signals.Count);
        if(signals.Count > 0){
            // hard set for just the first time
            selectedSignal = signals[i];
        }
	}

    void CreateSignal(int i){
        goSignal = new GameObject("signal_" + i);
        goSignal.transform.SetParent(soundscapeHolder.transform);
        goSignal.AddComponent<AudioSource>();
        goSignal.AddComponent<AudioLinkDemo>();
        goSignal.AddComponent<DHGSignalGenerator>();
        ald = goSignal.GetComponent<AudioLinkDemo>();

        ald.EnableSignal(goSignal);
        signals.Add(goSignal);
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            i++;
            if(i <= signals.Count - 1){
                SetSelectedSignal(i);
            }
            else if(i == signals.Count){
                i = 0;
                SetSelectedSignal(i);
            }
        }
	}

    public void SetSelectedSignal(int index){
        //Debug.Log("Selected signal is: " + index);
        selectedSignal = signals[index];
    }
}
