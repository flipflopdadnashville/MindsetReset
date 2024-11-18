using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour
{
    public AudioSource audio;

    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButtonUp(1) || (Input.GetMouseButton(1) && Input.GetMouseButtonUp(0)))
        {
            audio.PlayOneShot(audio.clip, .7f);
        }
    }
}
