using System;

public class DHGTangentSignal : DHGSignal
{
    public DHGTangentSignal()
    {
        signalMethod = DHGSignal.SignalMethod.TANGENT;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0f;
        double localPeroid = 15.0 / 16.0 * Math.PI;
        double shift = localPeroid / 2.0;
        double localTimeT = timet / period * localPeroid - shift;
        returnVal = magnitude * Math.Tan(localTimeT);
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (period <= 0) period = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.TANGENT);
    }
}
