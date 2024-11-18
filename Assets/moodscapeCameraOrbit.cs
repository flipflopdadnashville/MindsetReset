//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moodscapeCameraOrbit : MonoBehaviour
{

    public Transform startPos;
    public bool repeatable = true;
    public float speed = 1.1f;
    public float duration = 15.0f;

    float startTime, totalDistance;

    //public GameObject target;//the target object
    //private Vector3 point;//the coord to the point where the camera looks at
    //private bool cameraOrbiting = false;

    // Use this for initialization
    IEnumerator Start()
    {
        startTime = Time.time;
        totalDistance = Vector3.Distance(startPos.position, new Vector3(0, 30, 0));

        while (repeatable)
        {
            yield return RepeatLerp(startPos.position, new Vector3(0, Random.Range(20, 50), 0), Random.Range(2, 5));
            yield return new WaitForSeconds(Random.Range(30, 60));
            yield return RepeatLerp(new Vector3(0, 30, 0), new Vector3(0, Random.Range(300,800),0), Random.Range(2, 5));
            yield return new WaitForSeconds(Random.Range(30, 60));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!repeatable)
        {
            float currentDuration = (Time.time - startTime) * speed;
            float journeyFraction = currentDuration / totalDistance;
            this.transform.position = Vector3.Lerp(startPos.position, new Vector3(0, 30, 0), journeyFraction);
            //if (Input.GetMouseButtonDown(1))
            //{
            //    if(cameraOrbiting == false)
            //    {
            //        cameraOrbiting = true;
            //    }
            //    else
            //    {
            //        cameraOrbiting = false;
            //    }
            //}

            //if (cameraOrbiting == true)
            //{
            //    cameraOrbit();
            //}
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    //void cameraOrbit()
    //{
    //    if (cameraOrbiting == true)
    //    {
    //        point = target.transform.position;//get target's coords
    //        transform.LookAt(point);//makes the camera look to it
    //        transform.RotateAround(point, new Vector3(25.0f, 25.0f, 25.0f), 3 * Time.deltaTime * speed);
    //    }
    //}
}