namespace ValidatorWay;

public static class Converter
{
    private static readonly Dictionary<(Type Raw, Type Result), Delegate> _converters = new()
    {
        [(typeof(int), typeof(PositiveInt))] = new Func<int, Result>(PositiveInt.TryConvert),
        [(typeof(int?), typeof(PositiveInt?))] = new Func<int?, Result>(PositiveInt.TryConvert),
        [(typeof(int?), typeof(PositiveInt))] = new Func<int?, Result>(
            (raw) =>
            {
                if(raw == null)
                {
                    return new Result.Error("Property mush have a non-null value");
                }
                return PositiveInt.TryConvert(raw.Value);
            }
        ),
        [(typeof(string), typeof(string))] = new Func<string, Result>((raw) => new Result.Success<string>(raw)),
        [(typeof(int), typeof(int))] = new Func<int, Result>((raw) => new Result.Success<int>(raw)),
    };
    public static Result TryConvert<TResult, TRaw>(TRaw raw)
    {
        if(!_converters.TryGetValue((typeof(TRaw), typeof(TResult)), out Delegate? converter))
        {
            throw new InvalidOperationException($"There is no registered converter for convertion {typeof(TRaw)} -> {typeof(TResult)}");
        }
        var result = converter.DynamicInvoke(raw)!;
        return (Result)result;
    }
}
