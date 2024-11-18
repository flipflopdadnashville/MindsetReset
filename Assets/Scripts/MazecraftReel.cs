using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;
using DamageNumbersPro;

public class MazecraftReel : MonoBehaviour 
{
	public MazecraftGameManager instance;
	public GameObject goFish;
	public GameObject hook;
	public GameObject pole;
	public GameObject bendablePole;
	public GameObject line;
	public Transform fish;
	public Transform catchLocation;
	public float handleSpeed;
	public Transform handle;
	public DamageNumber numberPrefab;

	private bool reeling = false;
	private float startTime;

/*	void Update () 
	{
		if(instance.isFishing == true){
			if(Input.GetKeyDown(KeyCode.LeftControl))
			{
				Reel();
			}

			if(reeling == true){
				handle.Rotate(Vector3.right * handleSpeed);

				int random = Random.Range(1,17);
				
				if(random % 2 == 0){
					HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Soft);
				}
				else if(random %2 != 0){
					HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Rigid);
				}

				//if(reeling = true){
				FishLerp();
				//}

				if(Vector3.Distance(fish.position, catchLocation.position) < 3f)
				{
					//Debug.Log("Caught him!");
					bendablePole.GetComponent<MegaBend>().angle = -17f;
					//bendablePole.GetComponent<MegaBend>().dir = 0;
					line.GetComponent<LineRenderer>().SetVertexCount(0);
					goFish.GetComponent<Rigidbody>().AddForce(Random.Range(-1,1), Random.Range(-3, -1), 1);
					reeling = false;
					hook.GetComponent<MazecraftHookLaunch>().casting = false;
					hook.GetComponent<MazecraftHookLaunch>().catching = false;
					DamageNumber damageNumber = numberPrefab.Spawn(transform.position, instance.notificationDescriptions[Random.Range(0, instance.notificationDescriptions.Count - 1)]);
				}
			}
		}
	}

	public void Reel(){
		if(instance.pole.activeInHierarchy == true){
			reeling = true;
		}
		else{
			//instance.ToggleVesicaPlane();
		}
	}

	void FishLerp(){
		startTime = Time.time;
		//Debug.Log("startTime is: " + startTime);
		float journeyLength = Vector3.Distance (fish.position, catchLocation.position);
		//Debug.Log("From Reel, journeyLength is: " + journeyLength);
		float distCovered = 1; //(Time.time - Random.Range(1,2)) * Random.Range(.01f, .015f);
		//Debug.Log("From Reel, distCovered is: " + distCovered);
		float fracJourney = distCovered / journeyLength;
		//Debug.Log("From Reel, fracJourney is: " + fracJourney);
		fish.position = Vector3.Lerp(fish.position, new Vector3(catchLocation.position.x, catchLocation.position.y, catchLocation.position.z), fracJourney);

		//Debug.Log("From Reel, distance between fish and catchLocation is: " + Vector3.Distance(fish.position, catchLocation.position));
	}
*/}
