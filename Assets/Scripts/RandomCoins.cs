using UnityEngine;
using System.Collections;
public class RandomCoins : MonoBehaviour
{
    private MazecraftGameManager instance;
    public Terrain terrain;
    public GameObject goTerrain;
    public int numberOfObjects; // number of objects to place
    public int currentObjects; // number of placed objects
    public GameObject objectToPlace; // GameObject to place
    private int terrainWidth; // terrain size (x)
    private int terrainLength; // terrain size (z)
    private int terrainPosX; // terrain position x
    private int terrainPosZ; // terrain position z

/*    void Start()
    {
        instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        // find terrain
        // terrain size x
        terrainWidth = (int)terrain.terrainData.size.x;
        //Debug.Log("terrainWidth is: " + terrainWidth);
        // terrain size z
        terrainLength = (int)terrain.terrainData.size.z;
        //Debug.Log("terrainLength is: " + terrainLength);
        // terrain x position
        terrainPosX = (int)terrain.transform.position.x;
        //Debug.Log("terrainPosX is: " + terrainPosX);
        // terrain z position
        terrainPosZ = (int)terrain.transform.position.z;
        //Debug.Log("terrainPosZ is: " + terrainPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        if(goTerrain.activeInHierarchy == true && currentObjects < numberOfObjects)
        {
            InvokeRepeating("GenerateCoins", 1, 180);
        }
        else
        {
            CancelInvoke();
        }
    }

    void GenerateCoins()
    {
        // generate objects
        //Debug.Log("Checking to see if coins are needed. Current coins is: " + currentObjects + ". Number needed is: " + numberOfObjects);

        
        int objectsNeeded = numberOfObjects - currentObjects;
        //Debug.Log("Number of objects needed is: " + objectsNeeded);

        if (objectsNeeded > 0)
        {
            for (int i = 0; i < objectsNeeded; i++)
            {
                //Debug.Log("In for loop generating coins");

                // generate random x position
                float posx = instance.playerTwo.transform.position.x + Random.Range(-100, 100);
                // generate random z position
                float posz = instance.playerTwo.transform.position.z + Random.Range(-100, 100);
                // get the terrain height at the random position
                float posy = 4; //Terrain.activeTerrain.SampleHeight(new Vector3(posx, 4, posz));
                                // create new gameObject on random position

                //GameObject pickups = GameObject.Find("FidgeFries");

                GameObject newObject = (GameObject)Instantiate(objectToPlace, new Vector3(posx, posy + .5f, posz), Quaternion.identity);//, pickups.transform);
                newObject.transform.localScale = new Vector3(.75f, .75f, .75f);
                newObject.tag = "Coin";
                newObject.transform.SetParent(goTerrain.transform);

                currentObjects += 1;
            }
        }

        //Debug.Log("Generate objects complete!");
    }
*/}