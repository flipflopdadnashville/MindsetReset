using UnityEngine;
using System.Collections;
public class RandomRockets : MonoBehaviour
{

    public Terrain terrain;
    public int numberOfObjects; // number of objects to place
    public int currentObjects; // number of placed objects
    public GameObject objectToPlace; // GameObject to place
    private int terrainWidth; // terrain size (x)
    private int terrainLength; // terrain size (z)
    private int terrainPosX; // terrain position x
    private int terrainPosZ; // terrain position z

    void Start()
    {
        InvokeRepeating("GenerateCoins", 1, 30);
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
        
    }

    void GenerateCoins()
    {
        // generate objects
        if (currentObjects <= numberOfObjects)
        {
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + (terrainWidth * 2));
            // generate random z position
            int posz = Random.Range(-200, 200);
            // get the terrain height at the random position
            float posy = 4; //Terrain.activeTerrain.SampleHeight(new Vector3(posx, 4, posz));
            // create new gameObject on random position

            //GameObject pickups = GameObject.Find("FidgeFries");

            GameObject newObject = (GameObject)Instantiate(objectToPlace, new Vector3(posx, posy + 2, posz), Quaternion.identity);//, pickups.transform);
            newObject.transform.localScale = new Vector3(.75f, .75f, .75f);

            currentObjects += 1;
        }

        if (currentObjects == numberOfObjects)
        {
            Debug.Log("Generate objects complete!");
        }
    }
}