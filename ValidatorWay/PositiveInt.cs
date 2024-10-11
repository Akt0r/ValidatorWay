using System;

namespace ValidatorWay;

public struct PositiveInt
{
    public int Value { get; private set; }

    public PositiveInt(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, nameof(value));
        Value = value;
    }

    public static implicit operator int(PositiveInt val) => val.Value;
    public static implicit operator PositiveInt(int val) => new(val);

    public override readonly string ToString()
    {
        return Value.ToString();
    }
}
