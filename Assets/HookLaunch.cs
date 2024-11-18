using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;

public class HookLaunch : MonoBehaviour
{
    //public GameObject pole;
    //private Quaternion oPoleRot;

    public Rigidbody rbHook;
    public Transform bone;
    public Transform target;
    public GameObject fish;
    public GameObject bendablePole;
    public GameObject line;

    public float h = 25;
    public float gravity = -18;
    public bool casting = false;
    public bool catching = false;

    // Start is called before the first frame update
    void Start()
    {   
        rbHook.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Hook").transform.position.y < -100){
            casting = false;
        }

        if(casting == false){
            Vector3 newHookPos = new Vector3(bone.position.x, bone.position.y, bone.position.z);
            GameObject.Find("Hook").transform.position = newHookPos;
        }

         if(catching == true){
            //bendablePole.GetComponent<MegaBend>().angle -= Random.Range(-6f, 6f);
            Vector3 newHookPos = new Vector3(target.position.x, target.position.y, target.position.z);
            GameObject.Find("Hook").transform.position = newHookPos;
        }

        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				//if(hit.collider.tag == "Fish")
				// {
					target = GameObject.Find(hit.collider.name).transform;
                    Debug.Log("Landing spot is: " + target.name);
                    //casting = true;
				//}
			}
        }

        if(Input.GetMouseButtonUp(0))
		{
            // for now, just set casting to true always. Maybe update later to not cast if you're not going to hit a fish.
            casting = true;
            
            if(casting == true){
                Launch();
            }
		}
    }

    void Launch(){
        Physics.gravity = Vector3.up * gravity;
        rbHook.useGravity = true;
        rbHook.velocity = CalculateLaunchVelocity();
    }

    private Vector3 CalculateLaunchVelocity(){
        float displacementY = (target.position.y - rbHook.position.y);
        //Debug.Log("displacementY is: " + displacementY);
        Vector3 displacementXZ = new Vector3(target.position.x - rbHook.position.x, 0, target.position.z - rbHook.position.z);
        //Debug.Log("displacementXZ is: " + displacementXZ);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        //Debug.Log("velocityY is: " + velocityY);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h/gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
        //Debug.Log("velocityXZ is: " + velocityXZ);

        return velocityXZ + velocityY;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Fish"){
            rbHook.useGravity = false;
            rbHook.velocity = new Vector3(0,0,0);
            other.gameObject.transform.Rotate(15, -140, 15);
            line.GetComponent<Line>().Regenerate();
            //fish.GetComponent<Fish>().catching = true;
            catching = true;
            StartCoroutine(WaitForIt());
        }
    }

    IEnumerator WaitForIt(){
        Debug.Log("Wait for it, wait for it...");
        // turn off fish movement by setting "catching" to true
        yield return new WaitForSeconds(Random.Range(1.5f,4f));
        // fish.GetComponent<Fish>().catching = true;
        // catching = true;
        yield break;
    }

}
