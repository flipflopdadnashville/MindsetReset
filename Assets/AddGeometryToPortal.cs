using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGeometryToPortal : MonoBehaviour
{
    public GameObject[] sGeometryObjects;
    public GameObject portal;
    
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, sGeometryObjects.Length);
        GameObject geo = Instantiate(sGeometryObjects[i], new Vector3(0,0,0), new Quaternion(0,0,0,0));
        geo.transform.parent = portal.transform;
        geo.transform.localPosition = new Vector3(0, 0, 0);
        geo.transform.localScale = new Vector3(.75f, .75f, .75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
