using System;

public class DHGSquareSignal : DHGSignal
{
    public DHGSquareSignal()
    {
        signalMethod = DHGSignal.SignalMethod.SQUARE;
    }

    public override double GetValue(float timet)
    {
        double returnVal = 0.0f;
        double remainingVal = timet % period;
        if (remainingVal >= (period / 2.0))
        {
            returnVal = magnitude;
        }
        return returnVal;
    }

    public override bool OnValidate()
    {
        if (period <= 0) period = 1.0;
        return (signalMethod == DHGSignal.SignalMethod.SQUARE);
    }
}
