using System;

class DHGLogNormalSignal : DHGSignal
{
    public DHGLogNormalSignal()
    {
        signalMethod = DHGSignal.SignalMethod.LOG_NORMAL;
    }


    public override double GetValue(float timet)
    {
        double returnVal = 0.0;
        double variance = standardDeviation * standardDeviation;
        returnVal = magnitude * 1 / (timet * standardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Pow(Math.E, (-Math.Pow((Math.Log(timet) - mean), 2.0) / (2.0 * variance)));
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (standardDeviation <= 0) standardDeviation = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.LOG_NORMAL);
    }
}
