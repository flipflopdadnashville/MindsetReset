using System;

class DHGGaussianSignal : DHGSignal
{
    public DHGGaussianSignal()
    {
        signalMethod = DHGSignal.SignalMethod.GAUSSIAN;
    }


    public override double GetValue(float timet)
    {
        double posSq = (timet - mean) * (timet - mean);
        double variance = standardDeviation * standardDeviation;
        double returnVal = magnitude * 1 / Math.Sqrt(2 * Math.PI * variance) * Math.Pow(Math.E, -(posSq / (2.0 * variance)));
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (standardDeviation <= 0) standardDeviation = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.GAUSSIAN);
    }
}
