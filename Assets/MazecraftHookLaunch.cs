using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MegaFiers;

public class MazecraftHookLaunch : MonoBehaviour
{
    public MazecraftGameManager instance;
    public Rigidbody rbHook;
    public Transform bone;
    public Transform target;
    public GameObject fish;
    public GameObject bendablePole;
    public GameObject line;
    public GameObject reel;

    public float h = 45;
    public float gravity = -18;
    public bool casting = false;
    public bool catching = false;

    public float speed = 6.5f;
    public float radius = 1f;
    float radiusSq;

    Vector3 currentPosition;
    float distanceTravelled;
    public float arcFactor = 15.5f; // Higher number means bigger arc.
    Vector3 origin; // To store where the projectile first spawned.

    // Start is called before the first frame update
/*    void Start()
    {   
        radiusSq = radius * radius;
        origin = currentPosition = transform.position;
        rbHook.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(instance.isFishing == true){
            if(GameObject.Find("Hook").transform.position.y < -100){
                casting = false;
            }

            if(casting == true){
                Vector3 direction = target.position - currentPosition;
                //Debug.Log("From HookLaunch Update (casting == true), direction is: " + direction);
                currentPosition += direction.normalized * speed * Time.deltaTime;
                //Debug.Log("From HookLaunch Update (casting == true), currentPosition is: " + currentPosition);
                distanceTravelled += speed * Time.deltaTime;
                //Debug.Log("From HookLaunch Update (casting == true), distanceTravelled is: " + distanceTravelled);
                float totalDistance = Vector3.Distance(origin, target.position);
                //Debug.Log("From HookLaunch Update (casting == true), totalDistance is: " + totalDistance);
                float heightOffset = arcFactor * totalDistance * Mathf.Sin(distanceTravelled * Mathf.PI / totalDistance);
                //Debug.Log("From HookLaunch Update (casting == true), heightOffset is: " + heightOffset);
                transform.position = currentPosition + new Vector3(0,heightOffset, heightOffset);
            }

            if(casting == false){
                Vector3 newHookPos = new Vector3(bone.position.x, bone.position.y, bone.position.z);
                GameObject.Find("Hook").transform.position = newHookPos;
            }

            if(catching == true){
                bendablePole.GetComponent<MegaBend>().angle += Random.Range(-3.5f, 3f);
                //bendablePole.GetComponent<MegaBend>().dir = 160;
                Vector3 newHookPos = new Vector3(target.position.x, target.position.y, target.position.z);
                GameObject.Find("Hook").transform.position = newHookPos;
                line.GetComponent<Line>().Regenerate();
            }

            if(Input.GetKeyDown(KeyCode.LeftAlt))
            {
                PickAFish();
            }

            //if(Input.GetKeyUp(KeyCode.LeftAlt))
            //{
            //    // for now, just set casting to true always. Maybe update later to not cast if you're not going to hit a fish.
            //    Cast();
            //    // if(casting == true){
            //    //     Launch();
            //    // }
            //}
        }
    }

    public void PickAFish(){
        //Pick a random fish and make them the target
        if(instance.pole.activeInHierarchy == true){
            GameObject[] allFish = GameObject.FindGameObjectsWithTag("Fish");
            //Debug.Log("allFish length is: " + allFish.Length);

            int i = 0;
            foreach(GameObject fish in allFish){
                //Debug.Log("Fish " + i + " name is: " + fish.name);
                i++;
            }

            target = allFish[Random.Range(0, allFish.Length)].transform;

            if (target.transform.position.z > instance.playerTwo.transform.position.z)
            {
                GameObject goFish = GameObject.Find(target.name);
                reel.GetComponent<MazecraftReel>().goFish = goFish;
                //Debug.Log("Reel goFish is: " + reel.GetComponent<MazecraftReel>().goFish.name);
                reel.GetComponent<MazecraftReel>().fish = target.transform;
                //Debug.Log("Reel goFish is: " + reel.GetComponent<MazecraftReel>().fish.name);
                line.GetComponent<Line>().p2 = target.transform;
                //Debug.Log("Target fish is: " + target.name);
                Cast();
            }
        }
    }

    public void Cast(){
        if(instance.pole.activeInHierarchy == true){
            origin = currentPosition = transform.position;
            casting = true; 
        }
        instance.SetNotificationSettings("Press Left Ctrl once you catch one!", "", "", instance.GetNotificationStatus());
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Fish"){
            //Debug.Log("Collided with " + other.gameObject.name);
            casting = false;
            catching = true;
            rbHook.useGravity = false;
            rbHook.velocity = new Vector3(0,0,0);
            other.gameObject.transform.Rotate(15, -140, 15);
            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            line.GetComponent<Line>().Regenerate();
            
            HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Rigid);
        }
    }
*/}
