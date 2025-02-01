using System;

namespace ValidatorWay;

public readonly struct PositiveInt
{
    public int Value { get; init; }

    private PositiveInt(int value)
    {
        Value = value;
    }

    public static implicit operator int(PositiveInt val) => val.Value;

    public override readonly string ToString()
    {
        return Value.ToString();
    }

    public static Result TryConvert(int raw)
    {
        if (raw <= 0)
        {
            return new Result.Error($"Value {raw} should be greater then zero");
        }
        return new Result.Success<PositiveInt>(new PositiveInt(raw));
    }

    public static Result TryConvert(int? raw)
    {
        if (raw == null)
        {
            return new Result.Success<PositiveInt?>(null);
        }
        if (raw <= 0)
        {
            return new Result.Error($"Value {raw} should be greater then zero");
        }
        return new Result.Success<PositiveInt?>(new PositiveInt(raw.Value));
    }
}
