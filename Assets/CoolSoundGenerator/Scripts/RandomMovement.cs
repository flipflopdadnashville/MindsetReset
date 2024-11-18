using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       transform.position += new Vector3(Random.Range(-5f, 5f),Random.Range(-5f, 5f),Random.Range(-5f, 5f)); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)){
            transform.position += new Vector3(Random.Range(-15f, 15f),Random.Range(-15f, 15f),Random.Range(-15f, 15f));
        }
    }
}
