using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LightRays2DAbstract:MonoBehaviour{

	

	private void Start(){
		
	}
	
	

	
	protected abstract void GetReferences();
	
	protected abstract Material GetMaterial();
	
	protected abstract void ApplyMaterial(Material material);

	
}