using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTouchy : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        //Debug.Log(other.gameObject.tag);
        string parentName = transform.parent.name;
        //Debug.Log(parentName);
        GameObject parent = GameObject.Find(parentName);
        string otherParentName = other.transform.parent.name;

        if(other.gameObject.tag == "Player"){
        }
        else if(otherParentName != parentName){
            parent.transform.position = new Vector3(parent.transform.position.x + 20, parent.transform.position.y - 30, parent.transform.position.z);
        }
    }
}
