using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour 
{
	public Transform pointer;

	public float StaminaMax;
	public float Stamina;
	public float StaminaRegen;

	public float MinBoostEnable;
	public float MaxBoostEnable;
	public float CurrentBoostTemp;

	public float CurrentSpeed;

	public float BoostSpeed;
	public float RegularSpeed;

	public float RotationSpeed;

	public float ChangeDirMinStep;
	public float ChangeDirMaxStep;
	public float CurrentDirStep;

	private float m_changeDirStepTemp;

	public bool catching = false;

	// Use this for initialization
	void Start () 
	{
		pointer.SetParent(null);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(catching)
		{
			Debug.Log("Catching fish... controlled by Reel");
			// changed to disable script
			this.GetComponent<Fish>().enabled = false;
		}
		else
		{
			transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed);

			var targetRotation = Quaternion.LookRotation(transform.position - pointer.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
			transform.position = new Vector3(transform.position.x, -10, transform.position.z);
			if(Time.time > m_changeDirStepTemp + CurrentDirStep)
			{
				var heading = transform.position;
				var distance = heading.magnitude;
				Debug.Log("From Fish, distance is: " + distance);
				var direction = heading / distance; // This is now the normalized direction.
				Vector3 pos = direction + Random.insideUnitSphere * (-distance * Random.Range(1.3f, 1.5f));
				pointer.position = pos;
				CurrentDirStep = Random.Range(ChangeDirMinStep, ChangeDirMaxStep);
				m_changeDirStepTemp = Time.time;
			}
		}	
	}
}
