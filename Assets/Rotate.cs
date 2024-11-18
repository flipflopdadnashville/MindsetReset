using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Rotate left
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0, -10f * Time.deltaTime, 0, Space.Self);
        }
        
        //Rotate right
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0, 10f * Time.deltaTime, 0, Space.Self);
        }
    }
}
