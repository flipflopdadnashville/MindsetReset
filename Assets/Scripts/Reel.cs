using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;

public class Reel : MonoBehaviour 
{
	// public float BarabanSpeed;
	// public float Speed;
	// public bool Casting = false;

	// public Transform caster;

	public GameObject goFish;
	public GameObject hook;
	public GameObject pole;
	public GameObject bendablePole;
	public GameObject line;
	public Transform fish;
	public Transform catchLocation;
	public float handleSpeed;
	public Transform handle;

	private bool reeling = false;
	private float startTime;

	void Update () 
	{
		if(Input.GetMouseButton(1))
		{
			reeling = true;

			handle.Rotate(Vector3.right * handleSpeed);
			// if(pole.transform.localRotation.x > 0){
			// 	pole.transform.Rotate(-.1f,0,0);
			// }

			startTime = Time.time;
			float journeyLength = Vector3.Distance (fish.position, catchLocation.position);
			//Debug.Log("From Reel, journeyLength is: " + journeyLength);
			float distCovered = (Time.time - Random.Range(12,15)) * Random.Range(.01f, .015f);
			//Debug.Log("From Reel, distCovered is: " + distCovered);
			float fracJourney = distCovered / journeyLength;
			//Debug.Log("From Reel, fracJourney is: " + fracJourney);
			fish.position = Vector3.Lerp(fish.position, new Vector3(catchLocation.position.x, catchLocation.position.y, catchLocation.position.z), fracJourney);

			//Debug.Log("From Reel, distance between fish and catchLocation is: " + Vector3.Distance(fish.position, catchLocation.position));
			
			if(Vector3.Distance(fish.position, catchLocation.position) < 40f)
			{
				//Debug.Log("Caught him!");
				//bendablePole.GetComponent<MegaBend>().angle = 5f;
				//bendablePole.GetComponent<MegaBend>().dir = 0;
				line.GetComponent<LineRenderer>().SetVertexCount(0);
				//fish.parent = GameObject.Find("Bone.005_end").transform;
				//fish.position = new Vector3(Random.Range(-100, 100), Random.Range(-20,-1), Random.Range(0, 200));
				
				//fish.parent = null;
				reeling = false;
				goFish.GetComponent<Fish>().enabled = true;
				goFish.GetComponent<Fish>().catching = false;
				hook.GetComponent<HookLaunch>().casting = false;
				hook.GetComponent<HookLaunch>().catching = false;
			}
		}
	}

}
