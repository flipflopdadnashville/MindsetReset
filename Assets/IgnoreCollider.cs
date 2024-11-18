using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        Debug.Log("colliding with " + other.gameObject.tag);
        if(other.gameObject.tag == "eqWall"){
            Debug.Log("Hit a wall I want to ignore. Did it work?");
            Physics.IgnoreCollision(other.gameObject.GetComponent<BoxCollider>(), this.transform.GetComponent<SphereCollider>());
       }

    }
}
