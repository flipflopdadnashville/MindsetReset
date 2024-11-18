using UnityEngine;
using System.Collections;

public class CustomCameraShake : MonoBehaviour
{
	public float duration = 1f;
    public bool start = false;
    private Camera backgroundCamera;

    void Start(){
        backgroundCamera = GameObject.Find("BackgroundCamera").GetComponent<Camera>();
    }

    void Update(){
        if(start){
            start = false;
            StartCoroutine(Shaking());
        }
    }
    
    IEnumerator Shaking(){
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        backgroundCamera.enabled = false;

        while(elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = startPosition;
        backgroundCamera.enabled = true;
    }
}
