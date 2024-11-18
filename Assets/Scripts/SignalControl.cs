using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalControl : MonoBehaviour
{
    AudioSource audioSource;
    public DHGSignalGenerator signalGenerator;
    public AudioLinkDemo audioLink;
    public List<GameObject> channels = new List<GameObject>();
    public GameObject channel0;
    public GameObject channel1;
    public GameObject channel2;
    public GameObject channel3;
    public GameObject channel4;
    public GameObject channel5;
    public GameObject channel6;
    public GameObject channel7;
    public GameObject channel8;
    public GameObject channel9;
    Vector3 oPos0;
    Vector3 oPos1;
    Vector3 oPos2;
    Vector3 oPos3;
    Vector3 oPos4;
    Vector3 oPos5;
    Vector3 oPos6;
    Vector3 oPos7;
    Vector3 oPos8;
    Vector3 oPos9;
    bool randomEQ = false;
    int randomEQCount = 0;

    // Start is called before the first frame update
    void Start()
    {
      audioSource = this.GetComponent<AudioSource>(); 
      signalGenerator = this.GetComponent<DHGSignalGenerator>();
      audioLink = this.GetComponent<AudioLinkDemo>();
      oPos0 = channel0.transform.position;
      oPos1 = channel1.transform.position;
      oPos2 = channel2.transform.position;
      oPos3 = channel3.transform.position;
      oPos4 = channel4.transform.position;
      oPos5 = channel5.transform.position;
      oPos6 = channel6.transform.position;
      oPos7 = channel7.transform.position;
      oPos8 = channel8.transform.position;
      oPos9 = channel9.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        float zPos = this.transform.localPosition.z;

        if(zPos <= 10f){
            audioSource.volume = 0;
        }
        else{
            audioSource.volume = (zPos /100);
        }

        float xPos = this.transform.localPosition.x;

        audioSource.panStereo = xPos / 100; 
        
        if(Input.GetKey(KeyCode.Period) && Input.GetKey(KeyCode.LeftShift)){
            audioLink.frequency = audioLink.frequency + 1f;  
        } 

        if(Input.GetKey(KeyCode.Comma) && Input.GetKey(KeyCode.LeftShift)){
            audioLink.frequency = audioLink.frequency - 1f;  
        }

        if(Input.GetMouseButtonDown(1)){
            //StopMotion();
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if(Input.GetMouseButtonUp(0)){
            if(this.gameObject.name.Contains("SG")){
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        if(Input.GetKeyDown(KeyCode.R)){
            ToggleRandomEQ();  
        }

        if(Input.GetKeyDown(KeyCode.Z)){
            EQReset();
        }

        // if(Input.GetKeyDown(KeyCode.Alpha0)){
        //     SetToZero();
        // }
    }

    public void ToggleRandomEQ(){
        if(randomEQ == true){
            randomEQ = false;
        }
        else if(randomEQ == false){
            randomEQ = true;
        }
        
        if(randomEQ){
            randomEQCount++;
            InvokeRepeating("RandomizeEQ", .1f, 3);
        }
        else if(!randomEQ){
            CancelInvoke();
        }
    }

    public void RandomizeEQ(){
        StopMotion();
        //EQReset();
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            if(randomEQCount % 2 != 0){
                if(channel.transform.localPosition.x < -100 || channel.transform.localPosition.x > 150 || channel.transform.localPosition.z < 15 || channel.transform.localPosition.z > 150){
                    channel.transform.localPosition = new Vector3(Random.Range(-10,80),Random.Range(-10,30), Random.Range(10,30));
                }

                //channel.GetComponent<Rigidbody>().AddForce(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-.5f,.5f));
            }
            else if(randomEQCount % 2 == 0){
                Vector3.MoveTowards(channel.transform.position, new Vector3(channel.transform.position.x,channel.transform.position.y,0), 25f);
            }
        }
    }

    public void StopMotion(){
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void EQReset(){
        channel0.transform.position = oPos0;
        channel0.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel0.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel1.transform.position = oPos1;
        channel1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel2.transform.position = oPos2;
        channel2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel3.transform.position = oPos3;
        channel3.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel3.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel4.transform.position = oPos4;
        channel4.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel4.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel5.transform.position = oPos5;
        channel5.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel5.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel6.transform.position = oPos6;
        channel6.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel6.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel7.transform.position = oPos7;
        channel7.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel7.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel8.transform.position = oPos8;
        channel8.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel8.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel9.transform.position = oPos9;
        channel9.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel9.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void SetToZero(){
        channel0.transform.position = oPos0;
        channel0.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel0.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel1.transform.position = oPos1;
        channel1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel2.transform.position = oPos2;
        channel2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel2.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel3.transform.position = oPos3;
        channel3.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel3.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel4.transform.position = oPos4;
        channel4.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel4.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel5.transform.position = oPos5;
        channel5.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel5.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel6.transform.position = oPos6;
        channel6.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel6.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel7.transform.position = oPos7;
        channel7.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel7.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel8.transform.position = oPos8;
        channel8.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel8.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        channel9.transform.position = oPos9;
        channel9.GetComponent<Rigidbody>().velocity = Vector3.zero;
        channel9.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        foreach(GameObject channel in channels){
            channel.transform.localPosition = new Vector3(channel.transform.localPosition.x, channel.transform.localPosition.y, 9);
        }
    }
}
