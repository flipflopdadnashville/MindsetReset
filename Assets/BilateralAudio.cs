using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BilateralAudio : MonoBehaviour
{
    private AudioSource source;
    private float beforeSample = 1;
    bool active = false;
    public Image buttonIcon;


    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        //source.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (beforeSample < source.timeSamples)
            {
                // Still Playing Audio, Keep Following Value!
                beforeSample = source.timeSamples - 1f;
            }
            else if (beforeSample > source.timeSamples)
            {
                // If 'beforeSample' bigger than timeSamples,
                // It means timeSamples set '0'.

                //Debug.Log("Do Something when Replaying Music!);

                /*if (source.panStereo > 0)
                {
                    StartCoroutine(StartPan(source, .2f, -1f));
                }
                else if (source.panStereo <= 0)
                {
                    StartCoroutine(StartPan(source, .2f, 1f));
                }*/

                source.panStereo = source.panStereo * -1;

                beforeSample = source.timeSamples - 1f;
            }
        }
    }

    public IEnumerator StartPan(AudioSource audioSource, float duration, float targetPan)
    {
        //Debug.Log("Started pan");
        float currentTime = 0;
        float start = audioSource.panStereo;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.panStereo = Mathf.Lerp(source.panStereo, targetPan, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void ToggleBilateralAudio()
    {
        active = !active;

        if (active == true) {
            source.Play();
            buttonIcon.color = new Color32(250, 150, 65, 255);
        }
        if (active == false)
        {
            source.Stop();
            buttonIcon.color = new Color32(255, 255, 255, 255);
        }
    }
}
