using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public MazecraftGameManager instance;
    public AudioSource audioSource;
    public GameObject[] channels;
    public List<string> reminderDynamicStrings = new List<string>();
    public bool randomEQ = false;
    int randomEQCount = -1;
    bool cameraFound = false;
    Vector2 screenBounds;
    Vector3 goscreen;
    public Vector3 screenSize;
    public GameObject affirmationScreen;
    public GameObject flickeringLight;
    public TMP_Text affirmationTitle;
    public TMP_Text affirmationProgress;
    public TMP_Text reminderDynamic;
    public MeshRenderer one;
    public MeshRenderer two;
    public MeshRenderer three;
    public MeshRenderer four;
    public MeshRenderer five;
    public MeshRenderer six;
    public MeshRenderer seven;
    public MeshRenderer eight;
    public MeshRenderer nine;
    public MeshRenderer ten;
    public MeshRenderer sun;
    [Range(5f, 10f)] // 2nd value should be the duration of the audio in seconds
    public float loopStart, loopEnd;
    public Image buttonIcon;

    //private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        channels = GameObject.FindGameObjectsWithTag("GameController");
        audioSource = this.GetComponent<AudioSource>();
        BreatheOut();
        //ToggleRandomEQ();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying && audioSource.time > loopEnd)
        {
            audioSource.time = loopStart;
        }
        // Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.transform.position.z);
        // //Debug.Log("screenCenter " + screenCenter);

        float zPos = this.transform.localPosition.z;

        if(zPos <= 5f){
            audioSource.volume = 0;
        }
        else if(zPos > 5 && zPos < 100){
            audioSource.volume = (zPos / 170);
        }
        else{
            audioSource.volume = .4f;
        }

        float xPos = this.transform.position.x;
        ////Debug.Log("xPos is: " + xPos);
        
        Scene currentScene = SceneManager.GetActiveScene();
    
        if (currentScene.name != "LoadScene" && cameraFound == false){
            try{
                goscreen = Camera.main.WorldToScreenPoint(transform.position); 
                
           }
            catch(System.NullReferenceException e){
                // do nothing
            }

            ////Debug.Log("Screen bounds are: " + screenBounds.x + " " + screenBounds.y);
            cameraFound = true;
        }
        
        ////Debug.Log("GoPos " + goscreen);
 
        float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f,0f));
        //Debug.Log("distX " + distX);
 
        float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));
        ////Debug.Log("distY " + distY);

        // screen center x in dev is 800.
        ////Debug.Log("screen center x is:" + instance.screenCenter.x);
        float percentage = (this.gameObject.transform.localPosition.x / instance.screenCenter.x) * 10f;
        //Debug.Log("percentage is: " + percentage);
        if(xPos > (instance.screenCenter.x / -8) && xPos < (instance.screenCenter.x / 8)){
            audioSource.panStereo = 0;
        }
        if(xPos < (instance.screenCenter.x / -8)){
            audioSource.panStereo = percentage; 
        }
        else if(xPos > (instance.screenCenter.x / 8)){
            audioSource.panStereo = percentage;
        }

        //Debug.Log("pan is: " + audioSource.panStereo);

        /*if(Input.GetMouseButtonDown(0)){
             this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if(Input.GetMouseButtonDown(1)){
            StopMotion();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            ToggleRandomEQ();
        }*/

        // if(Input.GetKeyDown(KeyCode.Z)){
        //     EQReset();
        // }

        /*if(Input.GetKeyDown(KeyCode.Alpha0)){
            SetToZero();
        }*/
    }

    public IEnumerator LerpPosition(Transform channelTransform, Vector3 targetPosition, float duration)
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
        //StopMotion();
    }

    public void StopMotion(){
        CancelInvoke();
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            channel.GetComponent<Rigidbody>().velocity = Vector3.zero;
            channel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void SetToZero(){
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().velocity = Vector3.zero;
            channel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            channel.transform.localPosition = new Vector3(channel.transform.localPosition.x, channel.transform.localPosition.y, 4);
        }
    }

    public void ToggleRandomEQ(){
        randomEQ = !randomEQ;

        foreach (GameObject channel in channels)
        {
            if (randomEQ == true)
            {
                InvokeRepeating("BreatheIn", 0, instance.breathSpeedIn + instance.breathSpeedOut);
                InvokeRepeating("BreatheOut", instance.breathSpeedIn, instance.breathSpeedIn + instance.breathSpeedOut);
                buttonIcon.color = new Color32(250, 150, 65, 255);
                //InvokeRepeating("BreathHold", instance.breathSpeedIn + instance.breathSpeedOut, instance.breathSpeedIn + instance.breathSpeedOut);
            }

            if (randomEQ == false)
            {
                ////Debug.Log("Cancelled random EQ");
                StopCoroutine("LerpPosition");
                StopMotion();
                buttonIcon.color = new Color32(255, 255, 255, 255);
            }

            randomEQCount = 1;
        }

        SendNotification("channelZero", instance.GetNotificationStatus());
    }

    public void BreatheIn(){
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            StartCoroutine(LerpPosition(channel.transform, new Vector3(Random.Range(-300, 300), 0, Random.Range(-100, 100)), instance.breathSpeedIn));
        }
    }

    public void BreatheOut(){
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            var random = new System.Random();
            bool setToZero = random.Next(2) == 1;
            //Debug.Log("setToZero equals: " + setToZero + ". Setting " + channel.name + "to new position.");

            if(setToZero == true){
                StartCoroutine(LerpPosition(channel.transform, new Vector3(UnityEngine.Random.Range(-650,650), UnityEngine.Random.Range(-100,800), -Screen.height / 3f), instance.breathSpeedOut));
            }
            
            if(setToZero == false){
                //Debug.Log("Screen height is: " + Screen.height);
                StartCoroutine(LerpPosition(channel.transform, new Vector3(Random.Range(-300, 300), Random.Range(10, 800), Random.Range(-100, 100)), instance.breathSpeedOut));
            }
        }
    }

    public void BreathHold()
    {
        // we're not doing anything here
    }

    public void RandomizeEQ(){
        foreach(GameObject channel in channels){
            channel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            if(randomEQCount % 2 != 0){
                var random = new System.Random();
                bool setToZero = random.Next(2) == 1;
                //Debug.Log("setToZero equals: " + setToZero + ". Setting " + channel.name + "to new position.");

                if(setToZero == true){
                    StartCoroutine(LerpPosition(channel.transform, new Vector3(UnityEngine.Random.Range(-650,650), UnityEngine.Random.Range(-100,800), -Screen.height / 3f), instance.breathSpeedOut));
                }
                
                if(setToZero == false){
                    //Debug.Log("Screen height is: " + Screen.height);
                    StartCoroutine(LerpPosition(channel.transform, new Vector3(Random.Range(-300, 300), Random.Range(10, 800), Random.Range(-100, 100)), instance.breathSpeedOut));
                }
            }
            else if(randomEQCount % 2 == 0){
                StartCoroutine(LerpPosition(channel.transform, new Vector3(Random.Range(-300, 300), 0, Random.Range(-100, 100)), instance.breathSpeedIn));
            }
        }

        randomEQCount++;
        //Debug.Log("RandomEQ Count now " + randomEQCount);
    }

    public void SetColliders()
    {
        if (instance.activeCamera.name == "MoodscapeCamera") {
            foreach (GameObject channel in channels)
            {
                channel.GetComponent<SphereCollider>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject channel in channels)
            {
                channel.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    private void SendNotification(string hack, bool enablePopupNotification){
        if(this.gameObject.name == hack){
            if(randomEQ == true){
                instance.SetNotificationSettings("Breath Balls Starting", "", "", enablePopupNotification); 
            }
            else if(randomEQ == false){
                instance.SetNotificationSettings("Breathing Ending", "", "", enablePopupNotification);
            }
        }
    }

    IEnumerator SetAffirmationScreen()
    {
        affirmationTitle.text = instance.notificationDescriptions[Random.Range(0, instance.notificationDescriptions.Count - 1)];
        reminderDynamic.text = reminderDynamicStrings[Random.Range(0, reminderDynamicStrings.Count - 1)];
        //Debug.Log("Progress calculation returns: " + ((i / (instance.notificationDescriptions.Count + 1))) * 100);
        // Placeholder
        affirmationProgress.text = "";
        affirmationScreen.SetActive(true);
        flickeringLight.SetActive(false);
        one.enabled = false;
        two.enabled = false;
        three.enabled = false;
        four.enabled = false;
        five.enabled = false;
        six.enabled = false;
        seven.enabled = false;
        eight.enabled = false;
        nine.enabled = false;
        ten.enabled = false;
        sun.enabled = false;
        
        yield return new WaitForSeconds(10);
        affirmationScreen.SetActive(false);
        flickeringLight.SetActive(true);
        one.enabled = true;
        two.enabled = true;
        three.enabled = true;
        four.enabled = true;
        five.enabled = true;
        six.enabled = true;
        seven.enabled = true;
        eight.enabled = true;
        nine.enabled = true;
        ten.enabled = true;
        sun.enabled = true;
    }

    private void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(0)){
            if (affirmationScreen.activeInHierarchy == false)
            {
                //Debug.Log("Mouse click on this object");

                //Debug.Log("descriptions count is: " + (instance.notificationDescriptions.Count - 1));
                //Debug.Log("old i is: " + i);
                //if (i >= instance.notificationDescriptions.Count - 1)
                //{
                //    i = 0;
                //}
                //else
                //{
                //    i = i + 1;
                //    Debug.Log("set i to: " + i);
                //}

                //Debug.Log("i is now: " + i);

            // Need to add back in
                //StartCoroutine(SetAffirmationScreen());
            }
        //}
    }
}
