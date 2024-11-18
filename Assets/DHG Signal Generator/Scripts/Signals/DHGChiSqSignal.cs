using System;

class DHGChiSqSignal : DHGSignal
{
    public DHGChiSqSignal()
    {
        signalMethod = DHGSignal.SignalMethod.CHI_SQ;
    }


    private double Gamma(double n)
    {
        // using Stirling's approximation
        double returnVal = 1.0;
        if (n >= .5)
        {
            returnVal = Math.Sqrt(2.0 * Math.PI * n) * Math.Pow(n / Math.E, n);
        }
        return returnVal;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0;
        if (timet > 0)
        {
            double numerator = Math.Pow(timet, degreesOfFreedom / 2.0 - 1.0) * Math.Pow(Math.E, -(timet / 2.0));
            double denominator = Math.Pow(2.0, degreesOfFreedom / 2.0) * Gamma(degreesOfFreedom / 2.0);
            returnVal = magnitude * numerator / denominator;
        }
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (degreesOfFreedom < 0.5) degreesOfFreedom = 0.5;
        return (signalMethod == DHGSignal.SignalMethod.CHI_SQ);
    }
}
