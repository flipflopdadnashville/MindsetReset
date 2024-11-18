using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffirmationAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public List<AudioClip> currentAudioClips;
    public Image buttonIcon;
    public MazecraftGameManager instance;
    public SetNoiseGeneratorPreset preset;
    public VolumeControl breathBalls;

    public List<string> mindsets;
    public string mindset;

    bool active = false;
    int r = 0;
    int i = 3;
    int k = 0;

    public void Start()
    {
        
    }

    public void ChangeMindsetProgram()
    {
        k = k+1;
        //Debug.Log("k equals: " + k + ". mindsets.Count is: " + mindsets.Count);

        if (k == mindsets.Count)
        {
            k = 0;
        }
        else
        {
            mindset = mindsets[k];
        }

        GetAudioClips(mindsets[k]);
    }

    private void GetAudioClips(string mindset)
    {
        string clipName;

        if (active)
        {
            if (currentAudioClips.Count > 0)
            {
                currentAudioClips.Clear();
            }

            foreach (AudioClip clip in audioClips)
            {
                clipName = clip.name.Split("-")[0];
                if (String.Equals(clipName, mindset, StringComparison.InvariantCultureIgnoreCase))
                {
                    //Debug.Log(clipName + " equals " + mindset + ". Adding to currentAudioClips");
                    currentAudioClips.Add(clip);
                }
            }
        }
        else
        {
            if (currentAudioClips.Count > 0)
            {
                currentAudioClips.Clear();
            }

            foreach (AudioClip clip in audioClips)
            {
                clipName = clip.name.Split("-")[0];
                if (String.Equals(clipName, mindset, StringComparison.InvariantCultureIgnoreCase))
                {
                    //Debug.Log(clipName + " equals " + mindset + ". Adding to currentAudioClips");
                    currentAudioClips.Add(clip);
                }
            }
        }

        instance.SetNotificationSettings("New Mindset Loaded", "Name is: " + mindset, mindset, true);
    }

    public void ToggleAffirmationAudio()
    {
        active = !active;
        //Debug.Log("Affirmation audio toggled to: " + active);

        if (active)
        {
            ChangeMindsetProgram();
            InvokeRepeating("PlayRandomAffirmation", 0.001f, 15f);
            buttonIcon.color = new Color32(250, 150, 65, 255);
        }
        else
        {
            CancelInvoke();
            breathBalls.BreatheIn();
            breathBalls.StopMotion();
            buttonIcon.color = new Color32(255, 255, 255, 255);
            i = 0;
        }
    }

    public void PlayRandomAffirmation()
    {
        if (i % 3 == 0)
        {
            int newR;
            newR = r;

            r = UnityEngine.Random.Range(0, currentAudioClips.Count);

            if (r == newR)
            {
                r = UnityEngine.Random.Range(0, currentAudioClips.Count);
                //Debug.Log("r is now: " + r);
            }

            AudioClip clip = currentAudioClips[r];
            audioSource.clip = clip;
            audioSource.PlayOneShot(clip, 1f);
            StartCoroutine(WaitAndSetEQPreset());
        }
        else
        {
            AudioClip clip = currentAudioClips[r];
            audioSource.clip = clip;
            audioSource.PlayOneShot(clip, 1f);
            StartCoroutine(WaitAndSetEQPreset());
        }

        i++;
        //Debug.Log("Counter is: " + i);
    }

    IEnumerator WaitAndSetEQPreset()
    {
        yield return new WaitForSecondsRealtime(5);
        preset.SetEQPreset();
    }
}
