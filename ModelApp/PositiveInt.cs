using ValidatorWay;

namespace ModelApp;

public readonly struct PositiveInt
{
    public int Value { get; init; }

    static PositiveInt()
    {
/*         Converter.RegisterConverter(typeof(int), typeof(PositiveInt), new Func<int, Result>(TryConvert));
        Converter.RegisterConverter(typeof(int?), typeof(PositiveInt?), new Func<int?, Result>(TryConvert));
        Converter.RegisterConverter(typeof(int?), typeof(PositiveInt), new Func<int?, Result>(
            static (raw) =>
            {
                if(raw == null)
                {
                    return new Result.Error("Property mush have a non-null value");
                }
                return TryConvert(raw.Value);
            }
        )); */
    }

    private PositiveInt(int value)
    {
        Value = value;
    }

    public static implicit operator int(PositiveInt val) => val.Value;

    public override readonly string ToString()
    {
        return Value.ToString();
    }

/*     public static Result TryConvert(int raw)
    {
        if (raw <= 0)
        {
            return new Result.Error($"Value {raw} should be greater then zero");
        }
        return new Result.Success<PositiveInt>(new PositiveInt(raw));
    } */

    public static Result TryConvert(int? raw)
    {
        if (raw == null)
        {
            return new Result.Success(null);
        }
        if (raw <= 0)
        {
            return new Result.Error($"Value {raw} should be greater then zero");
        }
        return new Result.Success(new PositiveInt(raw.Value));
    }
}
