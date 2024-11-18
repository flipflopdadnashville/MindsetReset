using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbotWanderAI : MonoBehaviour
{
    private Vector3 oPos;
    private bool _stopSpawn;

    void Start()
    {
      transform.position = new Vector3(Random.Range(63, -23), Random.Range(-1, 10), Random.Range(4, 40));
      oPos = transform.position;
    }

    void Update()
    {  
      // Vector3 nPos = transform.position;
      // nPos = new Vector3(Random.Range(nPos.x + .1f, nPos.x - .1f), Random.Range(nPos.y + .1f, nPos.y - .1f), Random.Range(nPos.z + .1f, nPos.z - .1f));

      transform.position += new Vector3(0,0,Random.Range(.1f, .3f));
      
      if(transform.position.z > 225 || transform.position.z < -6){
        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
        transform.position = oPos;
      }
    }
}
