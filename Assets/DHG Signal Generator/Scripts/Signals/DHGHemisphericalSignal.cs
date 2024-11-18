using System;

public class DHGHemisphericalSignal : DHGSignal
{
    public DHGHemisphericalSignal()
    {
        signalMethod = DHGSignal.SignalMethod.HEMISPHERE;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0f;
        while (timet > period) timet -= (float)period;
        double localTimeT = 2.0 * timet / period - 1;
        // localTimeT is X
        double theta = Math.Acos(localTimeT);
        returnVal = magnitude * Math.Sin(theta);
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (period <= 0) period = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.HEMISPHERE);
    }
}
