using System;

public class DHGConstantSignal : DHGSignal
{
    public DHGConstantSignal()
    {
        signalMethod = DHGSignal.SignalMethod.CONSTANT;
    }

    public override double GetValue(float timet)
    {
        return constantVal;
    }

    public override bool OnValidate()
    {
        return (signalMethod == DHGSignal.SignalMethod.CONSTANT);
    }
}
