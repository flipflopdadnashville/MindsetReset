using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatController : MonoBehaviour
{
    public GameObject soundscapeHolder;
    public AudioSource heartbeatTrack;
    public AudioSource master;
    public AudioSource slave;
    public bool beating = true;
    public Image buttonIcon;

    // Start is called before the first frame update
    void Start()
    {
        beating = false;
        soundscapeHolder.GetComponent<BeatController>().enabled = false;
        heartbeatTrack.volume = 0;
    }

    void Update()
    {
        slave.timeSamples = master.timeSamples;
    }

    public void ToggleBeating()
    {
        beating = !beating;

        if (beating)
        {
            soundscapeHolder.GetComponent<BeatController>().enabled = true;
            heartbeatTrack.volume = 1f;
            buttonIcon.color = new Color32(250, 150, 65, 255);
        }
        else
        {
            soundscapeHolder.GetComponent<BeatController>().enabled = false;
            heartbeatTrack.volume = 0;
            buttonIcon.color = new Color32(255, 255, 255, 255);
        }
    }
}
