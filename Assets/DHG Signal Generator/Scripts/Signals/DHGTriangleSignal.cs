using System;

public class DHGTriangleSignal : DHGSignal
{
    public DHGTriangleSignal()
    {
        signalMethod = DHGSignal.SignalMethod.TRIANGLE;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0f;
        double numLoops = Math.Truncate((timet) / period);
        double remainingVal = (timet) - (period * numLoops);
        if (remainingVal < (period / 2.0))
        {
            returnVal = magnitude * (remainingVal * 2.0 / period);
        }
        else
        {
            returnVal = magnitude * (2 - (remainingVal * 2.0 / period));
        }
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (period <= 0) period = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.TRIANGLE);
    }
}
