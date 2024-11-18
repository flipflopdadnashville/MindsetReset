using System;

public class DHGSineSignal : DHGSignal
{
    public DHGSineSignal()
    {
        signalMethod = DHGSignal.SignalMethod.SINE;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0f;
        double localTimeT = timet / period * 2.0 * Math.PI;
        returnVal = magnitude * Math.Sin(localTimeT);
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (period <= 0) period = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.SINE);
    }
}