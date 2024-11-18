using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSineWaveForPrefab : MonoBehaviour
{
	public float frequencyLeftChannel;
	public float frequencyRightChannel;
	public float sampleRate = 44100;
	public float waveLengthInSeconds = 1.0f;
    public Color32 color_;

	AudioSource audioSource;

	int timeIndex = 0;
	bool start_ = false;
	bool soundGeneratorIsWorking = false;


	//<Setting initial parameters>
	void Start()
	{
        this.GetComponent<Renderer>().material.color = color_;
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
		audioSource.spatialBlend = 1; //makes the sound full 3D
        audioSource.Stop();
    }

    //<Setting parameters for awake>
    private void Awake()
    {
        this.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        this.GetComponent<Renderer>().material.SetColor("_EmissionColor", color_);
    }


	//<for play sound>
	void OnAudioFilterRead(float[] data, int channels)
	{
        for (int i = 0; i < data.Length; i += channels)  //i = i + channels
        {
            data[i] = CreateSine(timeIndex, frequencyLeftChannel, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequencyRightChannel, sampleRate);

            timeIndex++;

            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }

    }


	//<Create a sinewave>
	public float CreateSine(int timeIndex, float frequency, float sampleRate)
	{
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }


    //<Create a saw>
    public double Saw(int index, float frequency)
    {
        return 2.0 * (index * frequency - Mathf.Floor(index * frequency)) - 1.0;
    }


    //<Create a square>
    public static double Square(int index, float frequency)
    {
        if (Mathf.Sin((float)(frequency * index)) > 0) return 1;
        else return -1;
    }


    //<for catch collisions>
    void OnTriggerEnter(Collider other)
    {
        {
            this.GetComponent<Renderer>().material.SetColor("_EmissionColor", color_);
            this.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            this.transform.GetChild(0).GetComponent<Light>().color = color_;
            this.transform.GetChild(0).GetComponent<Light>().enabled = true;

            if (!audioSource.isPlaying)
            {
                timeIndex = 0;  //timer resets before playing sound
                audioSource.Play();
                soundGeneratorIsWorking = true;
            }
        }
    }
}


