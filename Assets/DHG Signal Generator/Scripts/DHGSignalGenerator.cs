using UnityEngine;
using System.Collections.Generic;

public class DHGSignalGenerator : MonoBehaviour {

    // Public Variables
    [Tooltip("If true the signal value is calculated on the fly.  False then the signal value is precalculated to a limited step and length.")]
    public bool limitless = true;
    [HideInInspector]
    public int numberOfSteps = 32;
    [HideInInspector]
    public float startTimeT = 0.00001f;
    [HideInInspector]
    public float timeStep = .1f;
    [HideInInspector]
    public bool genSignalOnValidate = false;
    [HideInInspector]
    public float visualizationScale = 1f;
    // Signals to utilize
    [HideInInspector]
    public List<DHGSignal> signals; 
    
    // Output Variables
    private float[] signalOutput;

    // Private Controls
    [SerializeField]
    [HideInInspector]
    private int numberOfSignals = 1;
    private bool editInitRequired = true;

    // SYSTEM FUNCTIONS
    // Users should never call these directly.

    // Validates the inputs as they change, and potentially updates the arrays to match.
    public void OnValidate()
    {
        if (numberOfSignals < 1) numberOfSignals = 1;
        if (numberOfSteps < 32) numberOfSteps = 32;
        if (numberOfSteps > 1024) numberOfSteps = 1024;
        if (limitless)
        {
            numberOfSteps = 256;
            genSignalOnValidate = false;
        }
        if (startTimeT <= 0) startTimeT = 0.000001f; // some signals are unavilable at exact 0
        if (timeStep < 0) timeStep = .01f;
        if (signals == null)
        {
            signals = new List<DHGSignal>();
            for (int i = 0; i < numberOfSignals; i++)
            {
                signals.Add(ScriptableObject.CreateInstance<DHGSineSignal>());
            }
        }
        
        if (numberOfSignals != signals.Count)
        {
            int diff = numberOfSignals - signals.Count;
            // If diff > 0 then we're adding signals, otherwise removing.
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    signals.Add(ScriptableObject.CreateInstance<DHGSineSignal>());
                }
            }
            else if (diff < 0)
            {
                signals.RemoveRange(numberOfSignals, signals.Count - numberOfSignals);
            }
            // Garbage Collection will handle any lost signals.
        }

        // Update signal types.
        for (int i = 0; i < signals.Count; i++) 
        {
            if (!(signals[i].OnValidate()))
            {
                switch (signals[i].signalMethod)
                {
                    case DHGSignal.SignalMethod.CONSTANT:
                        signals[i] = ScriptableObject.CreateInstance<DHGConstantSignal>();
                        break;
                    case DHGSignal.SignalMethod.SINE:
                        signals[i] = ScriptableObject.CreateInstance<DHGSineSignal>();
                        break;
                    case DHGSignal.SignalMethod.TANGENT:
                        signals[i] = ScriptableObject.CreateInstance<DHGTangentSignal>();
                        break;
                    case DHGSignal.SignalMethod.TRIANGLE:
                        signals[i] = ScriptableObject.CreateInstance<DHGTriangleSignal>();
                        break;
                    case DHGSignal.SignalMethod.SQUARE:
                        signals[i] = ScriptableObject.CreateInstance<DHGSquareSignal>();
                        break;
                    case DHGSignal.SignalMethod.HEMISPHERE:
                        signals[i] = ScriptableObject.CreateInstance<DHGHemisphericalSignal>();
                        break;
                    case DHGSignal.SignalMethod.GAUSSIAN:
                        signals[i] = ScriptableObject.CreateInstance<DHGGaussianSignal>();
                        break;
                    case DHGSignal.SignalMethod.EXPONENTIAL:
                        signals[i] = ScriptableObject.CreateInstance<DHGExponentialSignal>();
                        break;
                    case DHGSignal.SignalMethod.LOG_NORMAL:
                        signals[i] = ScriptableObject.CreateInstance<DHGLogNormalSignal>();
                        break;
                    case DHGSignal.SignalMethod.CHI_SQ:
                        signals[i] = ScriptableObject.CreateInstance<DHGChiSqSignal>();
                        break;
                    case DHGSignal.SignalMethod.GAMMA:
                        signals[i] = ScriptableObject.CreateInstance<DHGGammaSignal>();
                        break;
                    case DHGSignal.SignalMethod.BETA:
                        signals[i] = ScriptableObject.CreateInstance<DHGBetaSignal>();
                        break;
                    case DHGSignal.SignalMethod.WEIBULL:
                        signals[i] = ScriptableObject.CreateInstance<DHGWeibullSignal>();
                        break;
                }
            }
        }

        if (genSignalOnValidate)
        {
            GenerateSignal();
        }
    }

    // Called at start of program execution.
    void Start()
    {
        OnValidate();
        GenerateSignal();
    }

	// Update is called once per frame.  We don't need it..  yet
	void Update ()
    {
		// Update noiseValue based on selected noise method and elapsed time
	}

    // GUI FUNCTIONS
    // USERS may use thse programatically but should beware that they allocate and destroy memory components.

    // Adds a signal onto the end
    public void AddSignal()
    {
        numberOfSignals++;
        OnValidate();
    }

    // Removes the last signal
    public void RemoveSignal()
    {
        numberOfSignals--;
        OnValidate();
    }

    // Use this for initialization. Called by GUI directly.
    public void EditorInit()
    {
        if (editInitRequired)
        {
            editInitRequired = false;
            OnValidate();
            GenerateSignal();
        }
    }

    // Executes the selection signal/probability function to query the value at step
    private float GetLocalSignalValue(float timet)
    {
        float retVal = 0f;
        if(signals == null)
        {
            OnValidate();
        }

        if (signals != null)
        {
            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i].enabled)
                {
                    retVal += (float)signals[i].GetValue(timet);
                }
            }
        }

        return retVal;
    }

    // USER Functional Interface

    // Signal Generation Function
    // Used to create the selected signal/combination with input values from 0 to numberOfSteps
    public void GenerateSignal()
    {
        if (!limitless)
        {
            // Create the signal array to specs.        
            signalOutput = new float[numberOfSteps];

            for (int x = 0; x < signalOutput.Length; x++)
            {
                signalOutput[x] = GetLocalSignalValue(x * timeStep + startTimeT);
            }
        }
    }

    // OUTPUT FUNCTION
    // Use this to get the resulting calculated signal on a per step basis.
    public float GetSignal(float timet)
    {
        float retVal = 0.0f;
        if (limitless)
        {
            retVal = GetLocalSignalValue(timet);
        }
        else
        {
            int index = Mathf.RoundToInt((timet - startTimeT)/ timeStep);
            if ((signalOutput != null) && (index >= 0 && index < signalOutput.Length))
            {
                retVal = signalOutput[index];
            }
        }
        return retVal;
    }
}
