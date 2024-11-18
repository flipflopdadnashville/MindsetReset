using System;

class DHGGammaSignal : DHGSignal
{
    public DHGGammaSignal()
    {
        signalMethod = DHGSignal.SignalMethod.GAMMA;
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
            double numerator = Math.Pow(timet, shape - 1.0) * Math.Pow(Math.E, -(timet / scale));
            double denominator = Math.Pow(scale, shape) * Gamma(shape);
            returnVal = magnitude * numerator / denominator;
        }

        return returnVal;
    }

    public override bool OnValidate()
    {
        if (scale < 0.5) scale = 0.5;
        if (shape < 0.5) shape = 0.5;
        return (signalMethod == DHGSignal.SignalMethod.GAMMA);
    }
}
