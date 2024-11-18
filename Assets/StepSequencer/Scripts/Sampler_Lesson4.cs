using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Sampler_Lesson4 : MonoBehaviour
{
    [SerializeField] private StepSequencer_Lesson4 _sequencer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    [SerializeField] private SamplerVoice_Lesson4 _samplerVoicePrefab;
    [SerializeField] private List<AudioClip> audioClips;

    public SamplerVoice_Lesson4[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new SamplerVoice_Lesson4[_numVoices];

        //for (int i = 0; i < _numVoices; ++i)
        //{
        //    SamplerVoice_Lesson4 samplerVoice = Instantiate(_samplerVoicePrefab);
        //    samplerVoice.transform.parent = transform;
        //    samplerVoice.transform.localPosition = Vector3.zero;
        //    _samplerVoices[i] = samplerVoice;
        //}
    }

    //private void OnEnable()
    //{
    //    if (_sequencer != null)
    //    {
    //        _sequencer.Ticked += HandleTicked;
    //    }
    //}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightAlt))
        {
            UpdateSamplerVoices();

            //if (_sequencer != null)
            //{
            //    _sequencer.Ticked -= HandleTicked;
            //    _sequencer.Ticked += HandleTicked;
            //}

        }
    }

    private void OnDisable()
    {
        if (_sequencer != null)
        {
            _sequencer.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime, int midiNoteNumber, double duration)
    {
        float pitch = MusicMathUtils_Lesson4.MidiNoteToPitch(midiNoteNumber, MusicMathUtils_Lesson4.MidiNoteC4);
        //Debug.Log("Trying to play nextVoiceIndex number: " + _nextVoiceIndex);
        try
        {
            _samplerVoices[_nextVoiceIndex].Play(_audioClip, pitch, tickTime, _attackTime, duration, _releaseTime);
        }
        catch (IndexOutOfRangeException ex) { }

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }

    public void UpdateSamplerVoices()
    {
        _audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
        _attackTime = UnityEngine.Random.value;
        _releaseTime = UnityEngine.Random.value;

        if (_samplerVoices.Length > 0)
        {
            GameObject[] samplerVoicesToDestroy;
            samplerVoicesToDestroy = GameObject.FindGameObjectsWithTag("samplerVoice");
            foreach (GameObject voice in samplerVoicesToDestroy)
            {
                Destroy(voice);
            }

            Array.Clear(_samplerVoices, 0, _samplerVoices.Length);
        }

        CreateSamplerVoices();

        if (_sequencer != null)
        {
            _sequencer.Ticked -= HandleTicked;
            _sequencer.Ticked += HandleTicked;
        }
    }

    private SamplerVoice_Lesson4[] CreateSamplerVoices()
    {
        _numVoices = UnityEngine.Random.Range(1, 8);

        _samplerVoices = new SamplerVoice_Lesson4[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice_Lesson4 samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = new Vector3(UnityEngine.Random.Range(-400,400), UnityEngine.Random.Range(-400, 400), UnityEngine.Random.Range(-400, 400));
            samplerVoice.tag = "samplerVoice";
            _samplerVoices[i] = samplerVoice;
        }

        return _samplerVoices;
    }
}
