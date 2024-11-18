using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeoCamera : MonoBehaviour {
    private MazecraftGameManager instance;
    public Camera cameraOne;
    public Camera cameraTwo;
    public Camera eqCamera;
    public Camera bgCamera;
    public GraphicRaycaster raycaster;
    // public GameObject target;//the target object
    private bool orbitMode = true;
    public float speedMod = 5.0f;//a speed modifier
    private Vector3 point;//the coord to the point where the camera looks at
    public bool orbit = true;
    public GameObject portal;
 
/*    void Start () {//Set up things on the start method
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        orbit = false;
        eqCamera.enabled = false;
        cameraTwo.enabled = false;
        cameraOne.enabled = false;
        cameraOne.enabled = true;
        point = instance.cameraOneTarget.position;//get target's coords
        transform.LookAt(point);//makes the camera look to it
    }
 
    void Update () {//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
        // if(instance.gameMode == MazecraftGameManager.GameMode.Monkey){
        //     orbit = false;
        // }

        if(Input.GetKeyDown(KeyCode.Alpha0)){
            if(orbit == true){
                orbit = false;
                instance.distance = 32;
            }
            else if(orbit == false){
                orbit = true;
            }
        }

        //Debug.Log("orbit state is: " + orbit);
        if(orbit == false){  
            // 6,15,30 is the original position. Now need to move the camera with the character         
            //cameraOne.transform.position = new Vector3(6,15,30);

            if(Input.GetKey(KeyCode.J)){
                ZoomOut();
            }

            if(Input.GetKey(KeyCode.L)){
                ZoomIn();
            }

            cameraOne.transform.position = new Vector3(instance.cameraOneTarget.position.x, instance.cameraOneTarget.position.y + 2, instance.cameraOneTarget.position.z + instance.distance);
            cameraOne.transform.rotation = new Quaternion(0,180,0,0);
        }
        
        if(orbit == true){
                //Debug.Log("Orbit equals: " + orbit + ". Did we make it here?");
                instance.distance = Vector3.Distance(instance.cameraOneTarget.position, transform.position);
                //Debug.Log("distance from camera to ball is " + distance);

                if(instance.distance > 35){
                    transform.position = new Vector3(instance.cameraOneTarget.position.x, instance.cameraOneTarget.position.y, instance.cameraOneTarget.position.z);
                }

                // if(distance < 2){
                //     transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z + 15f);
                // }

                point = instance.cameraOneTarget.position;//get target's coords
                transform.position = new Vector3(transform.position.x, instance.cameraOneTarget.position.y +3.5f, transform.position.z);
                transform.LookAt(point);//makes the camera look to it
                //target.transform.LookAt(transform.position);
                transform.RotateAround (instance.cameraOneTarget.position, Vector3.up, speedMod * Time.deltaTime);
                transform.RotateAround (instance.cameraOneTarget.position, Vector3.left, speedMod * Time.deltaTime);
        }  

        if(Input.GetKeyDown(KeyCode.Z)){
            GeoCamera geoCamera = cameraOne.GetComponent<GeoCamera>();
            if(geoCamera.enabled == false){
                geoCamera.enabled = true;
            }
        } 
    }

    public void ZoomOut(){
        instance.distance = instance.distance + .3f;
        cameraOne.transform.position = new Vector3(instance.cameraOneTarget.position.x,instance.cameraOneTarget.position.y,instance.distance);

    }

    public void ZoomIn(){
        instance.distance = instance.distance - .3f;
        cameraOne.transform.position = new Vector3(instance.cameraOneTarget.position.x,instance.cameraOneTarget.position.y,instance.distance);
    }
*/}