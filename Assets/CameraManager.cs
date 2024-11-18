using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    private MazecraftGameManager instance;
    //public Camera cameraOne;
    //public Camera cameraTwo;
    //public Camera eqCamera;
    //public Camera bgCamera;
    public Camera moodscapeCamera;
    //public Camera blackoutCamera;
    //public GraphicRaycaster raycaster;
    //public GeoCamera cameraOrbit;

    // Start is called before the first frame update
    void Start()
    {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        //cameraOne.enabled = false;
        //cameraTwo.enabled = false;
        //eqCamera.enabled = false;
        moodscapeCamera.enabled = true;
        //instance.GetActiveCamera();
        //raycaster.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Alpha1)){
        //     cameraOne.enabled = true;
        //     cameraTwo.enabled = false;
        //     eqCamera.enabled = false;
        //     moodscapeCamera.enabled = false;
        //     raycaster.enabled = false;

        //     if(instance.portal.activeInHierarchy == true){
        //         instance.portal.SetActive(false);
        //     }

        //     if(cameraOrbit.enabled == false){
        //         cameraOrbit.enabled = true;
        //     }
            
        //     foreach(GameObject sGeometryObject in instance.sGeometryObjects){
        //             sGeometryObject.SetActive(false);
        //     }
        // } 

        // if(Input.GetKeyDown(KeyCode.Alpha2)){
        //     cameraOne.enabled = false;
        //     cameraTwo.enabled = true;
        //     eqCamera.enabled = false;
        //     moodscapeCamera.enabled = false;
        //     raycaster.enabled = true;
        //     if(instance.portal.activeInHierarchy == true){
        //         instance.portal.SetActive(false);
        //     }

        //     cameraTwo.transform.position = new Vector3(instance.activePlayer.transform.position.x, 10, 0);
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha3)){
        //     cameraOne.enabled = false;
        //     cameraTwo.enabled = false;
        //     eqCamera.enabled = true;
        //     moodscapeCamera.enabled = false;
        //     raycaster.enabled = true;
        //     if(instance.portal.activeInHierarchy == false){
        //         instance.portal.SetActive(true);
        //     }

        //     GameObject goCameraThree = GameObject.Find("EQ Camera");
        //     goCameraThree.transform.position = new Vector3(goCameraThree.transform.position.x, 20, -100);
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha1)){
        //     bgCamera.enabled = !bgCamera.enabled;
        //     raycaster.enabled = false;
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha5)){
        //     cameraOne.enabled = false;
        //     cameraTwo.enabled = false;
        //     eqCamera.enabled = false;
        //     moodscapeCamera.enabled = true;
        //     // if(instance.portal.activeInHierarchy == false){
        //     //     instance.portal.SetActive(true);
        //     // }

        //     instance.game.SetActive(false);
        //     raycaster.enabled = true;
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha2)){
        //     cameraOne.enabled = true;
        //     cameraTwo.enabled = true;
        //     eqCamera.enabled = true;
        //     moodscapeCamera.enabled = true;           
        //     raycaster.enabled = true;
        //     if(instance.portal.activeInHierarchy == false){
        //         instance.portal.SetActive(true);
        //     }
        // }
    }
}
