using UnityEngine;

public abstract class DHGSignal : ScriptableObject
{
    /*
     * Inherit from this class if you wish to create your own signals.
     * Update the enum below with your signal values.
     * You will also have to update SignalGeneratorEditor and add your 
     * signal  specific variables to the list below to allow them to show
     * up in the editor.
     */
    public enum SignalMethod
    {
        CONSTANT,
        SINE,
        TANGENT,
        TRIANGLE,
        SQUARE,
        HEMISPHERE,
        GAUSSIAN,
        EXPONENTIAL,
        LOG_NORMAL,
        CHI_SQ,
        GAMMA,
        BETA,
        WEIBULL
    };

    public bool enabled = true;
    public double magnitude = 1;
    public SignalMethod signalMethod;
    public double constantVal = 1.0;
    public double period = 1.0;
    public double alpha = 0.5;
    public double beta = 0.5;
    public double degreesOfFreedom = 0.5;
    public double lambda = 0.5;
    public double shape = 0.5;
    public double scale = 1.0;
    public double mean = 0.0;
    public double standardDeviation = 1.0;

    abstract public double GetValue(float timet);

    abstract public bool OnValidate();
}
