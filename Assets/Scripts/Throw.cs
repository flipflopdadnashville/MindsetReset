using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    public Camera camera;
    public GameObject ball;
    private float xroat, yroat = 0f;
    public float rotatespeed = 5f;
    public //LineRenderer //Line;
    float holdDownStartTime;
	public float shootpower = 300f;
    private Vector3 aim;
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("Mouse hold");
            //float holdDownTime = Time.time - holdDownStartTime;
            //Debug.Log("holdDownTime is: " + holdDownTime);
            float shootpower = 200;
            //Debug.Log("MouseButtonHold shootpower is: " + shootpower);

            //xroat = Input.GetAxis("Mouse X") * rotatespeed;
            //yroat = Input.GetAxis("Mouse Y") * rotatespeed;
            //ball.transform.rotation = Quaternion.Euler(0f, xroat, 0f);

            //Line.gameObject.SetActive(true);
            //Line.SetPosition(0, ball.transform.position);
            //Line.SetPosition(1, ball.transform.position - ball.transform.forward * (holdDownTime * 20)); 
        }       

        if(Input.GetMouseButtonDown(0)){
            //holdDownStartTime = Time.time;
            //mousePressDownPos = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            //shootpower = 5f;
            //float holdDownTime = Time.time - holdDownStartTime;
            //shootpower = holdDownTime * 20;

            //mouseReleasePos = Input.mousePosition;
            
            //Shoot(Force:mouseReleasePos - mousePressDownPos);
            
            // transform.GetComponent<Rigidbody>().AddForce();
            //Debug.Log("Shootpower is: " + shootpower);
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = ball.transform.up * shootpower;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            //Line.gameObject.SetActive(false);
        }

        // void Shoot(Vector3 Force){
        //         transform.GetComponent<Rigidbody>().AddForce(new Vector3(Force.x, 2, 100));
        // }
    }
}
