using System;

namespace ValidatorWay;

public readonly struct PositiveInt
{
    public int Value { get; init; }

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
