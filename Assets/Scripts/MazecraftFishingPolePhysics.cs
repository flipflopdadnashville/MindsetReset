using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;

public class MazecraftFishingPolePhysics : MonoBehaviour
{
    public GameObject pole;
    public GameObject bendablePole;
    private Quaternion oPoleRot;

    public GameObject fish;

    // Start is called before the first frame update
    void Start()
    {   
        oPoleRot = pole.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion newPoleRot = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        //Debug.Log("pole rotation is: " + newPoleRot);
    
        //if(newPoleRot.x > .1){
            if(Input.GetKey(KeyCode.C))
            {
                pole.transform.Rotate(-5f, 0, 0);
            }
        //}

        if(Input.GetKeyUp(KeyCode.C))
		{
			pole.transform.localRotation = Quaternion.Slerp(newPoleRot, oPoleRot, 2.75f);
		}
    }
}
