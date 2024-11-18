using UnityEngine;
using System.Collections;
public class RandomMazes : MonoBehaviour
{
public MazecraftGameManager instance;
public Terrain terrain;
public int numberOfObjects; // number of objects to place
private int currentObjects; // number of placed objects
public GameObject objectToPlace; // GameObject to place
private int terrainWidth; // terrain size (x)
private int terrainLength; // terrain size (z)
private int terrainPosX; // terrain position x
private int terrainPosZ; // terrain position z
public MazeSpawner mazeSpawner;
public GameObject rotatingMaze;

/* void Start()
{
    InvokeRepeating("GenerateMazes", 2f, 20f);
}
// Update is called once per frame
void Update()
{
// generate objects
    if(currentObjects <= numberOfObjects)
{
        // generate random x position
        int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
        // generate random z position
        int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
        // get the terrain height at the random position
        //float posy = 4; //Terrain.activeTerrain.SampleHeight(new Vector3(posx, 4, posz));
        // create new gameObject on random position

        //GameObject pickups = GameObject.Find("FidgeFries");

        mazeSpawner.CreateNewMaze();

        Instantiate(rotatingMaze, new Vector3(instance.activePlayer.transform.position.x + Random.Range(-200, 200), instance.forestTerrain.transform.position.y + Random.Range(10, 150), instance.activePlayer.transform.position.x + Random.Range(-200, 200)), new Quaternion(0,0,0,0));

        currentObjects += 1;
    }

        //if(currentObjects == numberOfObjects){
        //    Debug.Log("Generate objects complete!");
        //}
    }

    private void GenerateMazes(){
        GameObject[] rotatingMazes = GameObject.FindGameObjectsWithTag("rotatingMaze");

        if(instance.forestTerrain.activeInHierarchy == true && rotatingMazes.Length < numberOfObjects){
            mazeSpawner.CreateNewMaze();
            Instantiate(rotatingMaze, new Vector3(instance.activePlayer.transform.position.x + Random.Range(-200, 200), instance.forestTerrain.transform.position.y + Random.Range(10, 150), instance.activePlayer.transform.position.x + Random.Range(-200, 200)), new Quaternion(0,0,0,0));
        }
    }
*/}