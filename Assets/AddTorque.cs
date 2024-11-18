using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorque : MonoBehaviour
{
    public float amount = 1.1f;
    public GameObject go;

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal") * amount * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * amount * Time.deltaTime;

        go.GetComponent<Rigidbody>().AddTorque(transform.up * h, ForceMode.VelocityChange);
        go.GetComponent<Rigidbody>().AddTorque(transform.right * v, ForceMode.VelocityChange);
    }
}
