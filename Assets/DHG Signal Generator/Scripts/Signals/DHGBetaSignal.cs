using System;

class DHGBetaSignal : DHGSignal
{
    public DHGBetaSignal()
    {
        signalMethod = DHGSignal.SignalMethod.BETA;
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
            double numerator = Math.Pow(timet, alpha - 1.0) * Math.Pow(1.0 - timet, beta - 1.0);
            double denominator = Gamma(alpha) * Gamma(beta) / Gamma(alpha + beta);
            returnVal = magnitude * numerator / denominator;
        }

        return returnVal;
    }

    public override bool OnValidate()
    {
        if (alpha < 0.5) alpha = 0.5;
        if (beta < 0.5) beta = 0.5;
        return (signalMethod == DHGSignal.SignalMethod.BETA);
    }

}
