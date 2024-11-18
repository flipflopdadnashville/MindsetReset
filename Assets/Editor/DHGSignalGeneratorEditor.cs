using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DHGSignalGenerator))]
public class DHGSignalGeneratorEditor : Editor
{
    private Texture2D sampleSignalOutput;
    private List<bool> visible;

    // Use this for initialization
    void OnEnable()
    {
        sampleSignalOutput = new Texture2D(256, 128);
        visible = new List<bool>(1);
    }

    // Here for foldout support.
    public void OnInspectorUpdate()
    {
        this.Repaint();
    } 

    // Creation of the actual GUI in Inspector
    public override void OnInspectorGUI()
    {
        string label = "";
        float max = -10000000.0f;
        float min = 10000000.0f;
        // Get local signal generator object
        DHGSignalGenerator localSC = (DHGSignalGenerator)target;
        // Initialize the signal generator (supports first time drag of script onto an object.)
        // While this adds function call overhead to all GUI operations, the impact is minimized
        // by the code within.
        localSC.EditorInit();
        
        // Print the signal generator variables
        base.OnInspectorGUI();

        // Provide a way to add additional signals
        EditorGUI.BeginChangeCheck();

        if (!localSC.limitless)
        {
            localSC.genSignalOnValidate = EditorGUILayout.Toggle(new GUIContent("Gen Signal on Validate", "WARNING:  This adds heavy load to the updating of variables."), localSC.genSignalOnValidate);
            localSC.numberOfSteps = EditorGUILayout.IntField(new GUIContent("Number of Steps", "Number of steps to calculate out [32-1024]"), localSC.numberOfSteps);
            localSC.startTimeT = EditorGUILayout.FloatField(new GUIContent("Start Time", "Starting location for ongoing processing, (0,+)"), localSC.startTimeT);
            localSC.timeStep = EditorGUILayout.FloatField(new GUIContent("Time Step", "distance from previous time for each step. (0,+)"), localSC.timeStep);
        }

        if (GUILayout.Button("Add Signal"))
        {
            localSC.AddSignal();
        }
        
        // Add a way to remove channels
        if (localSC.signals != null && localSC.signals.Count > 1)
        {
            if (GUILayout.Button("Remove Signal"))
            {
                localSC.RemoveSignal();
            }
        }

        // Print the controls for each specific signal in the list
        if (localSC.signals != null)
        {
            for (int i = 0; i < localSC.signals.Count; i++)
            {
            /*
                SerializedProperty serialsignals = serializedObject.FindProperty("signals");
                EditorGUILayout.PropertyField(serialsignals, new GUIContent("Signals", "List of applied signals"), true);*/

                visible.Add(false);
                visible[i] = EditorGUILayout.Foldout(visible[i], "Signal " + i, true);
                if (visible[i])
                {
                    localSC.signals[i].enabled = EditorGUILayout.Toggle(new GUIContent("Enabled", "Turn a Signal on or Off in computation"), localSC.signals[i].enabled);
                    localSC.signals[i].magnitude = EditorGUILayout.DoubleField(new GUIContent("Magnitude", "Maximum deviation from 0 for signal"), localSC.signals[i].magnitude);
                    localSC.signals[i].signalMethod = (DHGSignal.SignalMethod)EditorGUILayout.EnumPopup(new GUIContent("Signal Type", "Signal to use for this channel"), localSC.signals[i].signalMethod);
                    switch (localSC.signals[i].signalMethod)
                    {
                        case DHGSignal.SignalMethod.CONSTANT:
                            localSC.signals[i].constantVal = EditorGUILayout.DoubleField(new GUIContent("Constant", "Constant value provided by signal"), localSC.signals[i].constantVal);
                            break;

                        case DHGSignal.SignalMethod.SINE:
                        case DHGSignal.SignalMethod.HEMISPHERE:
                        case DHGSignal.SignalMethod.SQUARE:
                        case DHGSignal.SignalMethod.TRIANGLE:
                        case DHGSignal.SignalMethod.TANGENT:
                            localSC.signals[i].period = EditorGUILayout.DoubleField(new GUIContent("Period", "Value of timeT at a full cycle, 0 is the start (0,+)"), localSC.signals[i].period);
                            break;

                        case DHGSignal.SignalMethod.BETA:
                            localSC.signals[i].alpha = EditorGUILayout.DoubleField(new GUIContent("Alpha", "Alpha shape descriptor [.5,+)"), localSC.signals[i].alpha);
                            localSC.signals[i].beta = EditorGUILayout.DoubleField(new GUIContent("Beta", "Beta shape descriptor [.5,+)"), localSC.signals[i].beta);
                            break;

                        case DHGSignal.SignalMethod.CHI_SQ:
                            localSC.signals[i].degreesOfFreedom = EditorGUILayout.DoubleField(new GUIContent("Degrees Of Freedom", "Number of degrees of freedom [.5,+)"), localSC.signals[i].degreesOfFreedom);
                            break;

                        case DHGSignal.SignalMethod.EXPONENTIAL:
                            localSC.signals[i].lambda = EditorGUILayout.DoubleField(new GUIContent("Lambda", "Rate of Occurance [1,+)"), localSC.signals[i].lambda);
                            break;

                        case DHGSignal.SignalMethod.GAMMA:
                        case DHGSignal.SignalMethod.WEIBULL:
                            localSC.signals[i].shape = EditorGUILayout.DoubleField(new GUIContent("Shape", "Shape Descriptor for Gamma [.5,+)"), localSC.signals[i].shape);
                            localSC.signals[i].scale = EditorGUILayout.DoubleField(new GUIContent("Scale", "Scale parameter [.5,+)"), localSC.signals[i].scale);
                            break;

                        case DHGSignal.SignalMethod.GAUSSIAN:
                        case DHGSignal.SignalMethod.LOG_NORMAL:
                            localSC.signals[i].mean = EditorGUILayout.DoubleField(new GUIContent("Mean", "Stastical Mean (-,+)"), localSC.signals[i].mean);
                            localSC.signals[i].standardDeviation = EditorGUILayout.DoubleField(new GUIContent("Standard Deviation", "Statistical Standard Deviation (0,+)"), localSC.signals[i].standardDeviation);
                            break;
                    }
                }
            }
        }

        localSC.visualizationScale = EditorGUILayout.Slider(new GUIContent("Visualization Scale", "Zoom Control for Signal Visualization [.01,1]"),localSC.visualizationScale, .01f, 1.0f);
        // Check for updates to any signal
        if (EditorGUI.EndChangeCheck())
        {
            localSC.OnValidate();
        }

        // Render the signal visualization texture
        if (sampleSignalOutput != null)
        {
            // Clear the texture
            for (int x = 0; x < sampleSignalOutput.width; x++)
            {
                for (int y = 0; y < sampleSignalOutput.height; y++)
                {
                    sampleSignalOutput.SetPixel(x, y, Color.black);
                }
            }
            sampleSignalOutput.Apply();

            int result = 0;
            float value = 0.0f;
            // Now get the new signal extents (high and low)
            for (int pt = 0; pt < 256; pt++)
            {
                float lookup = 0f;
                if(localSC.limitless)
                {
                    lookup = (float)pt * localSC.visualizationScale;
                }
                else
                {
                    lookup = (float)pt / 256.0f * (float)localSC.numberOfSteps * localSC.visualizationScale + localSC.startTimeT;
                }
                value = localSC.GetSignal(lookup);
                if (value > max) max = value;
                if (value < min) min = value;
            }

            float diff = max - min;

            // using those extents plot the signal on the texture
            for (int pt = 0; pt < 256; pt++)
            {
                float lookup = 0f;
                if (localSC.limitless)
                {
                    lookup = (float)pt * localSC.visualizationScale;
                }
                else
                {
                    lookup = (float)pt / 256.0f * (float)localSC.numberOfSteps * localSC.visualizationScale + localSC.startTimeT;
                }
                value = localSC.GetSignal(lookup);
                result = (int)((value - min) / diff * 127);
                sampleSignalOutput.SetPixel(pt,
                                            result,
                                            Color.white);
            }
            // make sure you update the texture
            sampleSignalOutput.Apply();
        }

        // Print all calculated information
        if (sampleSignalOutput != null)
        {
            label = "Visualization of Signal";
            GUILayout.Label(label);
            label = "Max Value : " + max;
            GUILayout.Label(label);
            GUILayout.Label(sampleSignalOutput);
            label = "Min Value : " + min;
            GUILayout.Label(label);

        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();

        // If we don't have the signals auto generated, allow user to generate their signal.
        if (!localSC.genSignalOnValidate)
        {
            if (GUILayout.Button("Generate Signal"))
            {
                localSC.GenerateSignal();
            }
        }
    }
}
