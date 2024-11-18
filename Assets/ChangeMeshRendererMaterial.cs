using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMeshRendererMaterial : MonoBehaviour
{
    public Material[] materials;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeMaterial", 60, 60);
    }

    // Update is called once per frame
    void ChangeMaterial()
    {
        i++;
        if(i < materials.Length){
            this.GetComponent<MeshRenderer>().material = materials[i];
        }
        else{
            i = 0;
            this.GetComponent<MeshRenderer>().material = materials[i];
        }
    }
}
