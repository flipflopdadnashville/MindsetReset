using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBehavior : MonoBehaviour
{
    public bool isVisible = true;
    MazecraftGameManager instance;
    public float distanceToAppear = 105.0f;
    float distance;

    // Update is called once per frame
/*    void Update()
    {
        if(instance == null){
            instance = GameObject.Find("_GameScope").GetComponent<MazecraftGameManager>();
        }

        distance = Vector3.Distance(instance.activePlayer.transform.position, transform.position);
		//Debug.Log("distance between activePlayer and maze is: " + distance);

        if(distance < distanceToAppear){
            isVisible = true;
        }
        if(distance > distanceToAppear){
            isVisible = false;
        }
        // if maze gets a certain distance from player, disable the renderer (hoping this improves lag)
        if(this.gameObject.name != "MazeSpawner_1"){
	        SetVisibility(isVisible); 
        }
    }

    private void SetVisibility(bool isVisible)
    {

        // We have reached the distance to Enable Object
        if (isVisible == true)
        {
			for (int i = 0; i < transform.childCount; i++){
				if(transform.GetChild(i).gameObject.name != "Easy Portal(Clone)"){
                    //if(transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled=false){
					    transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled=true;
                    //}
				}
			}
        }
        // need to add in a condition - if the name of the parent doesn't match the name of the parent of the wall the active player is touching
        else if(isVisible == false)
        {
			for (int i = 0; i < transform.childCount; i++){
				if(transform.GetChild(i).gameObject.name != "Easy Portal(Clone)"){
                    if(transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled=true){
					    transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled=false;
                    }
				}
                else if(transform.GetChild(i).gameObject.name == "Easy Portal(Clone)"){
                    Destroy(transform.GetChild(i).gameObject);
                }
			}
        }
    }
*/}
