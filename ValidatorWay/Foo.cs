using System;

namespace ValidatorWay;

public class Foo
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public PositiveInt Score { get; set; }
    public PositiveInt? Bar { get; set; }
    public Baz Baz { get; set; } = null!;
}
