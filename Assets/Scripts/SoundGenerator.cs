using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manufaktura.Music.Model;

public class SoundGenerator : MonoBehaviour
{
    public AudioSource audioSource;
    public GetAvailableSounds samples;
    public GameObject[] orbiters;
    public List<string> filteredSampleFiles = new List<string>();
    public bool forceAudioChange = false;

    void Awake(){
        orbiters = GameObject.FindGameObjectsWithTag("Orbiter");

        if(!samples){
            samples = Camera.main.GetComponent<GetAvailableSounds>();
        }

        InvokeRepeating("UnloadClips", 10, 30);

        // audioSource = GetComponent<AudioSource>();
        // StartCoroutine(LoadAudio(audioSource)); 
        // audioSource.Stop();
    }

    void FixedUpdate()
    {
        // if(filteredSampleFiles.Count == 0){
        //     //the match is case sensitive
        //     filteredSampleFiles = samples.FilterSoundsByName("DRU");
        //     //Debug.Log("Filtered sample file count is: " + filteredSampleFiles.Count);
        // }
        // else{
        //     //Debug.Log("Filtered sample file count is: " + filteredSampleFiles.Count);
        // }
    }

    void OnCollisionEnter(Collision other){
        //Debug.Log("Collision occured!");
        //if(other.gameObject.name == "Hills"){
            // if (!audioSource.isPlaying)
            // {
                StartCoroutine(LoadAudio(audioSource, false)); 
            //}
        //}
    }

    void OnCollisionEnter2D(Collision2D other){
        //Debug.Log("Collision occured!");
        // if (!audioSource.isPlaying)
        // {
            if(other.gameObject.tag != "Ball"){
                StartCoroutine(LoadAudio(audioSource, forceAudioChange)); 
            }
        //}
    }

    void OnTriggerEnter(){
        //Debug.Log("Trigger entered!");
        // if (!audioSource.isPlaying)
        // {
            StartCoroutine(LoadAudio(audioSource, forceAudioChange)); 
        //}
    }

    public void LoadAudio(){
        Destroy(audioSource.clip);
        StartCoroutine(LoadAudio(audioSource, false));
    }


    public void LoadAudio(bool force){
        Destroy(audioSource.clip);
        StartCoroutine(LoadAudio(audioSource, force));
    }

    IEnumerator LoadAudio(AudioSource audioSource, bool force) 
	{
        //Debug.Log("From LoadAudio Coroutine, force audio change is: " + force);
        string url;
	    WWW www;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			url = samples.AudioSampleFiles[UnityEngine.Random.Range(0, samples.AudioSampleFiles.Count)];
#elif UNITY_STANDALONE_OSX || UNITY_IOS
			url = ("file://" + samples.AudioSampleFiles[UnityEngine.Random.Range(0, samples.AudioSampleFiles.Count)]).Replace(@"\", "/");
#elif UNITY_WEBGL
            url = samples.AudioSampleFiles[UnityEngine.Random.Range(0, samples.AudioSampleFiles.Count)];
#endif

        www = new WWW(url);
        yield return www;

        if(!audioSource.isPlaying){
            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
        }

        if(audioSource.isPlaying && force == true){
            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
        }
    }

    public void LoadAudioByPitch(bool force, string pitch){
        Destroy(audioSource.clip);
        StartCoroutine(LoadAudioByPitch(audioSource, force, pitch));
    }

    IEnumerator LoadAudioByPitch(AudioSource audioSource, bool force, string pitch) 
	{
        //Debug.Log("From LoadAudio Coroutine, force audio change is: " + force);
        string url;
	    WWW www;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			url = samples.FilterSoundsByPitch(pitch);
#elif UNITY_STANDALONE_OSX || UNITY_IOS
			url = ("file://" + samples.FilterSoundsByPitch(pitch).Replace(@"\", "/"));
#elif UNITY_WEBGL
            url = samples.FilterSoundsByPitch(pitch);
#endif

        www = new WWW(url);
        yield return www;

        if(!audioSource.isPlaying){
            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
        }

        if(audioSource.isPlaying && force == true){
            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
        }
    }

    void UnloadClips(){
        Resources.UnloadUnusedAssets();
    }
}
