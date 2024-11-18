using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOneObjectBehavior : MonoBehaviour
{
    public bool canMove = true;

    private void OnCollisionEnter(Collision other) {
        //Debug.Log("MazeOneObjectBehavior collider tag is: " + other.gameObject.tag);
        // if(other.gameObject.tag == "Player"){
        //     if(GameObject.Find("Gumdrop").activeInHierarchy == true){
        //         canMove = false;
        //         GameObject.Find("Gumdrop").SetActive(false);
        //         GameObject.Find("FrozenGumdrop").SetActive(true);
        //     }
        // }
        // else{
        //     canMove = true;
        //     GameObject.Find("Gumdrop").GetComponent<BoneSphere>().enabled = true;
        //     GameObject.Find("Gumdrop").GetComponent<MazeCharacterMover>().enabled = true;
        // }
    }
}
