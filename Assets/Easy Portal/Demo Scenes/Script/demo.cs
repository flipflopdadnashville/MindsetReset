using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{
    public GameObject portalParticle;
    public GameObject spakleParticle;

    float portalScale = 0;
    float closeSpeed;

    // Use this for initialization
    void Start()
    {
        Invoke("ClosePortal", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (portalScale > 0)
        {
            portalScale -= Time.deltaTime * closeSpeed;
            portalParticle.transform.localScale = new Vector3(portalScale, portalScale, portalScale);

            if (portalScale <= 0)
            {
                portalParticle.SetActive(false);
                if (spakleParticle!=null) spakleParticle.SetActive(true);
            }
        }
    }

    void ClosePortal()
    {
        portalScale = portalParticle.transform.localScale.x;
        closeSpeed = 2.5f * portalScale;
    }

}
