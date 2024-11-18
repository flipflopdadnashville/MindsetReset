using System;

class DHGWeibullSignal : DHGSignal
{
    public DHGWeibullSignal()
    {
        signalMethod = DHGSignal.SignalMethod.WEIBULL;
    }


    public override double GetValue(float timet)
    {
        double returnVal = 0.0;

        if (timet > 0)
        {
            returnVal = magnitude * (shape / scale) * Math.Pow(timet / scale, shape - 1.0) * Math.Pow(Math.E, -Math.Pow(timet / scale, shape));
        }

        return returnVal;
    }

    public override bool OnValidate()
    {
        if (scale < 0.5) scale = 0.5;
        if (shape < 0.5) shape = 0.5;
        return (signalMethod == DHGSignal.SignalMethod.WEIBULL);
    }
}
