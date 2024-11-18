using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class GetAvailableSounds : MonoBehaviour
{
    public MazecraftGameManager instance;
    public List<string> AudioSampleFiles = new List<string>();
    public List<string> filteredSampleFiles = new List<string>();
    public string fileType = "mp3";
    public bool filtered = false;
    public string filter = "";
    public List<AudioSource> audioSources = new List<AudioSource>();

    void Awake()
    {
        AudioSampleFiles = GetAvailableSoundsFromStreamingAssets(fileType);
        
        if(filtered == true){
            AudioSampleFiles = FilterSoundsByName(filter);
        }

        StartCoroutine(LoadAudio());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnloadAudio();
            StartCoroutine(LoadAudio());
        }
    }

    public List<string> GetAvailableSoundsFromStreamingAssets(string fileType){
        IEnumerable<string> eAudioSampleFiles = Enumerable.Empty<string>();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        eAudioSampleFiles = Directory.EnumerateFiles(Application.streamingAssetsPath + "\\Sound\\AudioSamples", "*." + fileType, SearchOption.AllDirectories);
#elif UNITY_STANDALONE_OSX || UNITY_IOS
        eAudioSampleFiles = Directory.EnumerateFiles(Application.streamingAssetsPath + "/Sound/AudioSamples", "*." + fileType, SearchOption.AllDirectories);
#endif

        AudioSampleFiles = eAudioSampleFiles.ToList();

        return AudioSampleFiles;
    }

    public List<string> FilterSoundsByName(string match){
        //Debug.Log("match is: " + match);
        filteredSampleFiles = AudioSampleFiles.FindAll(x => x.Contains(match)).ToList();

        return filteredSampleFiles;
    }

    public string FilterSoundsByPitch(string match){
        filteredSampleFiles = AudioSampleFiles.FindAll(x => x.Contains(match)).ToList();

        // hackity hack
        return filteredSampleFiles[0];
    }

    IEnumerator LoadAudio()
    {
        string url;
        WWW www;

        foreach (AudioSource source in audioSources)
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_WIN
							            url = AudioSampleFiles[UnityEngine.Random.Range(0, AudioSampleFiles.Count)];
            #elif UNITY_STANDALONE_OSX || UNITY_IOS
							            url = ("file://" + AudioSampleFiles[UnityEngine.Random.Range(0, AudioSampleFiles.Count)]).Replace(@"\", "/");
            #elif UNITY_WEBGL
							            url = AudioSampleFiles[UnityEngine.Random.Range(0, AudioSampleFiles.Count)];
            #endif

            //Debug.Log("Trying to load and play clip: " + url + " on " + source.name);

            www = new WWW(url);
            yield return www;

            source.clip = www.GetAudioClip(false, false);
            source.Play();
        }
    }

    private void UnloadAudio()
    {
        foreach (AudioSource source in audioSources)
        {
            source.clip.UnloadAudioData();
        }
    }
}
