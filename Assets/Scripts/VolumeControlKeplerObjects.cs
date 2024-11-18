using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DamageNumbersPro;

public class VolumeControlKeplerObjects : MonoBehaviour
{
    public AudioSource audioSource;
    Vector2 screenBounds;
    Vector3 goscreen;
    public Vector3 screenSize;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.transform.position.z);
        // //Debug.Log("screenCenter " + screenCenter);

        float yPos = this.transform.localPosition.y;

        //in standalone version, if yPos <= -75. Playing with it for Labyrinth.
        if(yPos <= -1175f){
            audioSource.volume = 0;
        }
        else if(yPos > -1175 && yPos < 100){
            if (yPos < 0)
            {
                audioSource.volume = ((yPos / 10) * -1);
            }
            else
            {
                audioSource.volume = (yPos / 10);
            }
        }
        else{
            audioSource.volume = .7f;
        }

        float xPos = this.transform.position.x;
        ////Debug.Log("xPos is: " + xPos);
        
        Scene currentScene = SceneManager.GetActiveScene();
    
        if (currentScene.name != "JWROrbits"){
            try{
                goscreen = Camera.main.WorldToScreenPoint(transform.position); 
                
           }
            catch(System.NullReferenceException e){
                // do nothing
            }

            ////Debug.Log("Screen bounds are: " + screenBounds.x + " " + screenBounds.y);
            // cameraFound = true;
        }
        
        ////Debug.Log("GoPos " + goscreen);
 
        float distX = Vector3.Distance(new Vector3(Screen.width / 2, 0f, 0f), new Vector3(goscreen.x, 0f,0f));
        //Debug.Log("distX " + distX);
 
        float distY = Vector3.Distance(new Vector3(0f, Screen.height / 2, 0f), new Vector3(0f, goscreen.y, 0f));
        ////Debug.Log("distY " + distY);

        // screen center x in dev is 800.
        
        
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Debug.Log("screen center x is:" + screenCenter.x);

        float percentage = (this.gameObject.transform.localPosition.x / screenCenter.x) * 10f;
        //Debug.Log("percentage is: " + percentage);

        // in standalone scene, take out the * 10
        audioSource.panStereo = percentage * 5;
        
        //Debug.Log("pan is: " + audioSource.panStereo);
    }
}
