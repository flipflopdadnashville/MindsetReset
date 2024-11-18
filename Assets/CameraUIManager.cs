using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUIManager : MonoBehaviour
{
    private MazecraftGameManager instance;
    public Dropdown cameraDropdown;
    private Camera cameraOne;
    private Camera cameraTwo;
    private Camera eqCamera;
    private Camera bgCamera;
    private GraphicRaycaster raycaster;

    void Start(){
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
    }

/*    void Update(){
        if(cameraOne == null){
            GetCameras();
        }
    }
*/
/*    public void ChangeCamera(Dropdown change)
    {
        Debug.Log(change.value);
        if(change.value == 0){
            cameraOne.enabled = true;
            cameraTwo.enabled = false;
            eqCamera.enabled = false;
            raycaster.enabled = false;

            if(instance.portal.activeInHierarchy == true){
                instance.portal.SetActive(false);
            }
            
            foreach(GameObject sGeometryObject in instance.sGeometryObjects){
                    sGeometryObject.SetActive(false);
            }
        }
        if(change.value == 1){
            cameraOne.enabled = false;
            cameraTwo.enabled = true;
            eqCamera.enabled = false;
            raycaster.enabled = true;
            if(instance.portal.activeInHierarchy == true){
                instance.portal.SetActive(false);
            }

            cameraTwo.transform.position = new Vector3(instance.activePlayer.transform.position.x, 10, 0);
        }
        if(change.value == 2){
            cameraOne.enabled = false;
            cameraTwo.enabled = false;
            eqCamera.enabled = true;
            raycaster.enabled = true;
            if(instance.portal.activeInHierarchy == false){
                instance.portal.SetActive(true);
            }

            GameObject goCameraThree = GameObject.Find("EQ Camera");
            goCameraThree.transform.position = new Vector3(goCameraThree.transform.position.x, 20, -100);
        }
        if(change.value == 3){
            cameraOne.enabled = true;
            cameraTwo.enabled = true;
            eqCamera.enabled = true;
            raycaster.enabled = true;
            if(instance.portal.activeInHierarchy == false){
                instance.portal.SetActive(true);
            }
        }
        else{
            Debug.Log("Invalid value on camera change dropdown");
        }
    }
*/
/*    private void GetCameras(){
        cameraOne = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTwo = GameObject.Find("CameraTwo").GetComponent<Camera>();
        eqCamera = GameObject.Find("eqCamera").GetComponent<Camera>();
        bgCamera = GameObject.Find("bgCamera").GetComponent<Camera>();
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }
*/}
