using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public Transform floor;
    Quaternion _originalRotation;

    void Awake(){

        _originalRotation = floor.rotation;
    }
    public void ResetFloor(){
        // reset the floor to its original rotation;
        floor.rotation = _originalRotation;
    }
}
