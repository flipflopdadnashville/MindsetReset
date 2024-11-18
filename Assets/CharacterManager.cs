using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

public class CharacterManager : MonoBehaviour
{
    public MazecraftGameManager instance;
    AsyncOperationHandle<Material> opMaterialHandle;

    // Start is called before the first frame update
/*    void Start()
    {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>(); 
        instance.activePlayer = instance.playerTwo; 
    }
*/
    // Update is called once per frame
/*    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            ChangePlayerBaseMaterial();
        }

        if(Input.GetKeyDown(KeyCode.N)){
            StartGame();
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.C)){
            ChangePlayerAccessoryMaterial();
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.P)){
            SelectCharacter();
        }

    }
*/
/*    void StartGame(){
        if(instance.playerTwo.activeInHierarchy == false){
            //instance.gumdrop.SetActive(true);
            instance.playerTwo.SetActive(true);
            //instance.gumdrop.GetComponent<Rigidbody>().useGravity = true;
            instance.playerTwo.GetComponent<Rigidbody>().useGravity = true;
            //instance.gumdrop.SetActive(false);
            //instance.activePlayer = instance.activePlayer;
        }
    }
*/
/*    public void SelectCharacter(){
        //if(instance.GetActivePlayer() == instance.gumdrop || instance.activePlayer == instance.glasses){ //glasses needed for first time only
        //    //Debug.Log("From SelectCharacter, activePlayer is: " + instance.activePlayer);
        //    Vector3 placementPos = instance.glasses.transform.position;
        //    //Debug.Log("Gumpdrop position is: " + placementPos);
        //    instance.gumdrop.SetActive(false);
        //    instance.playerTwo.SetActive(true);
        //    instance.playerTwo.transform.position = new Vector3(placementPos.x, placementPos.y, placementPos.z);
        //    instance.activePlayer = instance.playerTwo;
        //    instance.cameraOneTarget = instance.playerTwo.transform;
        //    if(instance.playerTwo.GetComponent<Rigidbody>().useGravity == false){
        //        instance.playerTwo.GetComponent<Rigidbody>().useGravity = true;
        //    }
        //} 
        //else if(instance.GetActivePlayer() == instance.playerTwo){
        //    //Debug.Log("From SelectCharacter, activePlayer is: " + instance.activePlayer);
        //    Vector3 placementPos = instance.playerTwo.transform.localPosition;
        //    //Debug.Log("Fidge position is: " + placementPos);
        //    instance.playerTwo.SetActive(false);
        //    instance.gumdrop.SetActive(true);
        //    instance.gumdrop.transform.localPosition = new Vector3(placementPos.x, placementPos.y + 70, placementPos.z);
        //    instance.activePlayer = instance.glasses;
        //    instance.cameraOneTarget = instance.glasses.transform;
        //    //instance.redDragon.SetActive(true);
        //    //instance.redDragon.transform.localPosition = new Vector3(placementPos.x, placementPos.y + 10, placementPos.z);
        //    //instance.activePlayer = instance.redDragon;
        //    //instance.cameraOneTarget = instance.redDragon.transform;
        //}
        //else if(instance.GetActivePlayer() == instance.redDragon){
        //    //Debug.Log("From SelectCharacter, activePlayer is: " + instance.activePlayer);
        //    Vector3 placementPos = instance.redDragon.transform.localPosition;
        //    //Debug.Log("Fidge position is: " + placementPos);
        //    instance.redDragon.SetActive(false);
        //    instance.gumdrop.SetActive(true);
        //    instance.gumdrop.transform.localPosition = new Vector3(placementPos.x, placementPos.y + 70, placementPos.z);
        //    instance.activePlayer = instance.glasses;
        //    instance.cameraOneTarget = instance.glasses.transform;
        //}
    }
*/
/*    public void ChangePlayerBaseMaterial(){
        if(instance.activePlayer.name == "glasses"){
                //instance.playerSkin.GetComponent<SkinnedMeshRenderer>().material = StartCoroutine(LoadMaterial());
            }
            else{
            StartCoroutine(LoadMaterial("base"));
        }
    }
*/
/*    public void ChangePlayerAccessoryMaterial(){
        if(instance.activePlayer.name == "glasses"){
            //GameObject gogglesOne = GameObject.Find("marco1_001");
            //GameObject gogglesTwo = GameObject.Find("marco1_002");
            //Material gogglesMaterial = StartCoroutine(LoadMaterial());
            //gogglesOne.GetComponent<MeshRenderer>().material = gogglesMaterial;
            //gogglesTwo.GetComponent<MeshRenderer>().material = gogglesMaterial;
        }
        else{
            StartCoroutine(LoadMaterial("accessories"));
        }
    }
*/
/*    public IEnumerator LoadMaterial(string target)
    {
        opMaterialHandle = Addressables.LoadAssetAsync<Material>(instance._preloadedMaterialKeys[Random.Range(0, instance._preloadedMaterialKeys.Count - 1)]);
        yield return opMaterialHandle;

        //Debug.Log("opMaterialHandle result is: " + opMaterialHandle.Result);

        if (opMaterialHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Material currentMaterial = opMaterialHandle.Result;

            if (target == "base")
            {
                instance.body.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.leftPlate.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.rightPlate.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.rearPlate.GetComponent<MeshRenderer>().material = currentMaterial;
            }
            else if (target == "accessories")
            {
                
                instance.body.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.leftPlate.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.rightPlate.GetComponent<MeshRenderer>().material = currentMaterial;
                instance.rearPlate.GetComponent<MeshRenderer>().material = currentMaterial;
            }
        }
    }
*/}
