using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public Animator m_bombAnimator;
    private MazeSpawner ms;

/*    void Start(){
        ms = GameObject.Find("MazeSpawner_1").GetComponent<MazeSpawner>();
    }

    void Update(){
        if((transform.localPosition.y < -350 && transform.localPosition.y > -600000)){
            transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(25, 120), transform.localPosition.z);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "bombTarget"){
            //Debug.Log("Collided with " + other.gameObject.name);
            StartCoroutine(ExplodeBomb(this.gameObject, m_bombAnimator));
        }
    }

    
    IEnumerator ExplodeBomb(GameObject bomb, Animator m_bombAnimator)
    {
        GameObject[] allMazeCells = GameObject.FindGameObjectsWithTag("bombTarget");

		// get name of object player is touching
		List<Transform> mazeCellTransforms = new List<Transform>();

		foreach(GameObject mazeCell in allMazeCells){
			mazeCellTransforms.Add(mazeCell.transform);
		}

        //Wait for 1 second
        yield return new WaitForSeconds(2.9f);

        m_bombAnimator.SetBool("attack01", true);
        Transform closestWall = ms.GetClosestWall(mazeCellTransforms, this.gameObject.transform.position);
        //Debug.Log("Closest wall is: " + closestWall.name);
        if(closestWall.gameObject.activeInHierarchy == true){
            Destroy(closestWall.gameObject);
        }

        Destroy(bomb);        
    }
*/}
