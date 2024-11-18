using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectChildOf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent != null){
            transform.parent = null;
            transform.localPosition = new Vector3(Random.Range(-3,3), Random.Range(3, 6), Random.Range(-5, 5));
        }
    }
}
