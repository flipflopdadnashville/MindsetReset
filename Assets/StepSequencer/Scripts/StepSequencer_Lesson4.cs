using UnityEngine;
using System.Collections.Generic;
using System;

public class StepSequencer_Lesson4 : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public bool Active;
        public int MidiNoteNumber;
        public double Duration;
    }

    public delegate void HandleTick(double tickTime, int midiNoteNumber, double duration);

    public event HandleTick Ticked;

    [SerializeField] private Metronome_Lesson4 _metronome;
    [SerializeField] private List<Step> _steps;

    private int _currentTick = 0;

#if UNITY_EDITOR
    public List<Step> GetSteps()
    {
        return _steps;
    }
#endif

    private void OnEnable()
    {
        foreach(Step step in _steps)
        {
            if (UnityEngine.Random.value >= 0.5)
            {
                step.Active = true;
            }
            else
            {
                step.Active = false;
            }

            step.MidiNoteNumber = UnityEngine.Random.Range(1, 120);

            step.Duration = UnityEngine.Random.Range(1, 5);
        }

        if (_metronome != null)
        {
            _metronome.Ticked += HandleTicked;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            foreach (Step step in _steps)
            {
                if (UnityEngine.Random.value >= 0.5)
                {
                    step.Active = true;
                }
                else
                {
                    step.Active = false;
                }

                step.MidiNoteNumber = UnityEngine.Random.Range(1, 120);

                step.Duration = UnityEngine.Random.Range(1, 4);
            }

            if (_metronome != null)
            {
                _metronome.Ticked -= HandleTicked;
                _metronome.Ticked += HandleTicked;
            }
        }
    }

    private void OnDisable()
    {
        if (_metronome != null)
        {
            _metronome.Ticked -= HandleTicked;
        }
    }

    public void HandleTicked(double tickTime)
    {
        int numSteps = _steps.Count;

        if (numSteps == 0)
        {
            return;
        }

        Step step = _steps[_currentTick];

        if (step.Active)
        {
            if (Ticked != null)
            {
                Ticked(tickTime, step.MidiNoteNumber, step.Duration);
            }
        }

        _currentTick = (_currentTick + 1) % numSteps;
    }
}
