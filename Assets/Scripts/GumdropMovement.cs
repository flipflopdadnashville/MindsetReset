using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.OneBadDad.Fidgetopia{
    public class GumdropMovement : MonoBehaviour
    {  
        Vector3 Vec;
        private float moveSpeed = .3f;
        public Transform lookAtTarget;

    void Update() {
            // Strafe left
            if (Input.GetKey (KeyCode.A)) { 
                transform.position -= new Vector3(.3f,0,0);
            }
            // Move forward
            if (Input.GetKey (KeyCode.W)) {
                transform.position -= transform.forward*moveSpeed;
            } 
            // Move backward
            if (Input.GetKey (KeyCode.S)) {
                transform.position += transform.forward*moveSpeed;
            }
            // Strafe right     
            if (Input.GetKey (KeyCode.D)) {
                transform.position += new Vector3(.3f,0,0);
            } 
            // Jump     
            if (Input.GetKey (KeyCode.Space)) {
                transform.position += new Vector3(Time.deltaTime * UnityEngine.Random.Range(-20,25),Time.deltaTime * UnityEngine.Random.Range(20,25),Time.deltaTime * UnityEngine.Random.Range(-20,40));
                //transform.LookAt(lookAtTarget, Vector3.forward);
            } 

            //strike
            // if (Input.GetKey (KeyCode.E)) {
            //     transform.position -= transform.forward*(moveSpeed * 2);
            // } 

            }
        }
}