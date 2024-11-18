using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoiSpawnSystem : MonoBehaviour
{
    private bool _stopSpawn;

    void Start()
    {
      //target = GameObject.Find("glasses");
      //transform.position = new Vector3(Random.Range(13, - 13), Random.Range(2, -8), Random.Range(-30, -20));
      StartCoroutine(InstantiateKoi());
    }

    //void Update()
    //{  
      // Vector3 nPos = transform.position;
      // nPos = new Vector3(Random.Range(nPos.x + .1f, nPos.x - .1f), Random.Range(nPos.y + .1f, nPos.y - .1f), Random.Range(nPos.z + .1f, nPos.z - .1f));

      //transform.position += new Vector3(0,0,Random.Range(.01f, .1f));
      //float zDistance = transform.position.z - target.transform.position.z;

      //if(zDistance > 20){
      //  Destroy(this.gameObject);
      //}

      // if(Input.GetButtonDown("Fire1"))
      //   {
      //       RaycastHit hit;
      //       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);           
      //       if (Physics.Raycast(ray, out hit))
      //       {
      //           target = hit.transform.gameObject;
      //           if(target.tag == "MazeTwoWall")
      //           {
      //             Transform parentT = target.transform.parent;
      //             GameObject parent = GameObject.Find(parentT.name);
      //             parent.transform.position = new Vector3(Random.Range(parent.transform.position.x - 3, parent.transform.position.x + 3), parent.transform.position.y, parent.transform.position.z);
      //           }
      //       }          
      //   }
    //}

    IEnumerator InstantiateKoi(){
      while(_stopSpawn == false){
        string koiVariant = "Koinobori_" + Random.Range(0,7);
        Debug.Log("Spawning " + koiVariant);
        Instantiate(Resources.Load(koiVariant), new Vector3(Random.Range(8, - 23), Random.Range(2, -8), Random.Range(-30, -20)), transform.rotation);

        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
      }
    }
}
