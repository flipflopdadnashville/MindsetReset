using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneController : MonoBehaviour
{
    GameObject toneOne;
    GameObject toneTwo;
    GameObject toneThree;
    GameObject toneFour;
    AudioLinkDemo toneOneController;
    AudioLinkDemo toneTwoController;
    AudioLinkDemo toneThreeController;
    AudioLinkDemo toneFourController;

    // Start is called before the first frame update
    void Start()
    {
        toneOne = GameObject.Find("signal_0");
        toneTwo = GameObject.Find("signal_1");
        toneThree = GameObject.Find("signal_2");
        toneFour = GameObject.Find("signal_3");
        toneOneController = toneOne.GetComponent<AudioLinkDemo>();
        toneTwoController = toneTwo.GetComponent<AudioLinkDemo>();
        toneThreeController = toneThree.GetComponent<AudioLinkDemo>();
        toneFourController = toneFour.GetComponent<AudioLinkDemo>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
