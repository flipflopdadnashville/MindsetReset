using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSGeometry : MonoBehaviour
{
    // Public variables show up in the Inspector
	public Vector3 RotateSpeed = new Vector3 (3.0F, 3.0F, 3.0F);
	public Vector3 WobbleAmount = new Vector3 (0.0F, 0.0F, 0.0F);
	public Vector3 WobbleSpeed = new Vector3 (0.0F, 0.0F, 0.0F);

	// Private variables do not show up in the Inspector
	private Transform tr;
	private Vector3 BasePosition;
	private Vector3 NoiseIndex = new Vector3();
    private bool rotateClockwise = true;

	// Use this for initialization
	void Start () {
        
		// https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
		tr = GetComponent ("Transform") as Transform;

		BasePosition = tr.position;

		NoiseIndex.x = Random.value;
		NoiseIndex.y = Random.value;
		NoiseIndex.z = Random.value;
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(DetermineRotationDirection());
		// 1. ROTATE
		// Rotate the cube by RotateSpeed, multiplied by the fraction of a second that has passed.
		// In other words, we want to rotate by the full amount over 1 second
		tr.Rotate (Time.deltaTime * RotateSpeed);

		// 2. WOBBLE
		// Calculate how much to offset the cube from it's base position using PerlinNoise
		Vector3 offset = new Vector3 ();
		offset.x = Mathf.PerlinNoise (NoiseIndex.x, 0) - 0.5F;
		offset.y = Mathf.PerlinNoise (NoiseIndex.y, 0) - 0.5F;
		offset.z = Mathf.PerlinNoise (NoiseIndex.z, 0) - 0.5F;

		offset.Scale (WobbleAmount);

		// Set the position to the BasePosition plus the offset
        if(rotateClockwise == true){
		    transform.position = BasePosition + offset;
        }
        else{
            transform.position = BasePosition - offset;
        }

		// Increment the NoiseIndex so that we get a new Noise value next time.
		NoiseIndex += WobbleSpeed * Time.deltaTime;
	}

    IEnumerator DetermineRotationDirection(){
        int randomInt = Random.Range(1,10);

        if(randomInt % 2 == 0){
            rotateClockwise = true;
        }
        else{
            rotateClockwise = false;
        }

        yield return new WaitForSeconds(10);
    }
}
