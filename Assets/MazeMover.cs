 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMover : MonoBehaviour
{
    public float movespeed = 10f;
    public float turnspeed = 5f;
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -20){
            transform.position = new Vector3(transform.position.x, -19.9f, transform.position.z);
        }

        if(transform.position.y >= 20){
            transform.position = new Vector3(transform.position.x, 19.9f, transform.position.z);
        }

        if(transform.position.y > -20 && transform.position.y < 20){
            if (Input.GetKey (KeyCode.LeftArrow)) { 
                    transform.position -= new Vector3(.1f,0,0);
                }
                // Move forward
                if (Input.GetKey (KeyCode.RightArrow)) {
                    transform.position += new Vector3(.1f,0,0);
                } 
                // Move backward
                if (Input.GetKey (KeyCode.UpArrow)) {
                    transform.position += new Vector3(0,.1f,0);
                }
                // Strafe right     
                if (Input.GetKey (KeyCode.DownArrow)) {
                    transform.position -= new Vector3(0,.1f,0);
                }
            }  
    }
}
