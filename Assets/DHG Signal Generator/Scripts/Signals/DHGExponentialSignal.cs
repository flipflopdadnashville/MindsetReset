using System;

class DHGExponentialSignal : DHGSignal
{
    public DHGExponentialSignal()
    {
        signalMethod = DHGSignal.SignalMethod.EXPONENTIAL;
    }


    public override double GetValue(float timet)
    {
        double pos = timet * lambda;
        double returnVal = magnitude * lambda * Math.Pow(Math.E, -pos);
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (lambda <= 0.5) lambda = 0.5;
        return (signalMethod == DHGSignal.SignalMethod.EXPONENTIAL);
    }
}
