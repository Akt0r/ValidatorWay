using ValidatorWay;

namespace ModelApp;

public class Foo
{
    static Foo()
    {
/*         Converter.RegisterConverter(typeof(string), typeof(string), 
            new Func<string, Result>((raw) => new Result.Success<string>(raw)));
        Converter.RegisterConverter(typeof(int), typeof(int), 
            new Func<int, Result>((raw) => new Result.Success<int>(raw))); */
    }
    public string? Name { get; set; }
    public int Age { get; set; }
    public PositiveInt Score { get; set; }
    public PositiveInt? Bar { get; set; }
    public Baz Baz { get; set; } = null!;
}
