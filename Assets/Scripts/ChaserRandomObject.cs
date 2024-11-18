using UnityEngine;
using System.Collections;
public class ChaserRandomObject : MonoBehaviour
{
    public MazecraftGameManager instance;
    public Terrain terrain;
    public int numberOfObjects = 1; // number of objects to place
    public GameObject objectToPlace; // GameObject to place
    private float distance;

/*    void Start()
    {
        InvokeRepeating("GenerateChaser", 1f, 4f);
    }

    void Update() {
        //if(Input.GetKeyDown(KeyCode.V)){
            //instance.ToggleChaserGame();
        //}
    }

    private void DeleteChaser(){
        GameObject[] chasers = GameObject.FindGameObjectsWithTag("Ball");

        if(chasers.Length > 0){
            foreach(GameObject chaser in chasers){
                distance = Vector3.Distance (instance.activePlayer.transform.position, chaser.transform.position);

                //Debug.Log("Distance from player to chaser is: " + distance);
                
                if(chaser.name != "Chaser" && distance > 75){
                    Destroy(chaser);
                    //numberOfObjects = 0;
                }
            }
        }
    }

    private void GenerateChaser(){
        if(instance.chaserGameOn == true){
            //Debug.Log("Chaser game is on");
            // destroy all objects
            DeleteChaser();

            //only do if terrain is active
            if(instance.forestTerrain.activeInHierarchy == true){
                //Debug.Log("Forest terrain is active");
                if(instance.activePlayer.activeInHierarchy == true){
                    //Debug.Log("Active player is active");
                    //Debug.Log("Number of objects is: " + numberOfObjects);
                    if((numberOfObjects == 1 && distance > 75) || (numberOfObjects == 1 && this.transform.position.y < -75)){
                        Vector3 playerPos = instance.activePlayer.transform.position;
                        GameObject newObject = (GameObject)Instantiate(objectToPlace, new Vector3(playerPos.x + Random.Range(-2, 2), Random.Range(40, 45), playerPos.z + Random.Range(-10, -20)), Quaternion.identity);
                        newObject.GetComponent<Rigidbody>().useGravity = true;
                        numberOfObjects = 1;
                        //Debug.Log("Chaser generated");
                    }
                }
            }
        }
    }
*/}