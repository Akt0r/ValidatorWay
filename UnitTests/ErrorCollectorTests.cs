using System.Text.Encodings.Web;
using System.Text.Json;
using ModelApp;
using ValidatorWay;

namespace UnitTests;

public class ErrorCollectorTests
{
    [Fact]
    public void SimplePositiveTest()
    {
        Converter converter = new();
        converter.RegisterConverters(typeof(PositiveInt).Assembly);
        var (baz, errors) = ErrorCollector<Baz>
            .Build(converter)
            .With(v => v.SomeInt, 42)
            .Result();
        Assert.Empty(errors);
        Assert.NotNull(baz);
        Assert.Equal(42, baz.SomeInt.Value);
    }

    [Fact]
    public void NullablePositiveTest()
    {
        Converter converter = new();
        converter.RegisterConverters(typeof(PositiveInt).Assembly);
        var (baz, errors) = ErrorCollector<Baz>
            .Build(converter)
            .With(v => v.SomeInt, (int?)42)
            .Result();
        Assert.Empty(errors);
        Assert.NotNull(baz);
        Assert.Equal(42, baz.SomeInt.Value);
    }

    [Fact]
    public void SimpleNegativeTest()
    {
        Converter converter = new();
        converter.RegisterConverters(typeof(PositiveInt).Assembly);
        var (baz, errors) = ErrorCollector<Baz>
            .Build(converter)
            .With(v => v.SomeInt, -42)
            .Result();
        Assert.NotEmpty(errors);
        Assert.Null(baz);
        Console.WriteLine(errors.Single());
    }

    [Fact]
    public void NullableNegativeTest()
    {
        Converter converter = new();
        converter.RegisterConverters(typeof(PositiveInt).Assembly);
        var (baz, errors) = ErrorCollector<Baz>
            .Build(converter)
            .With(v => v.SomeInt, (int?)null)
            .Result();
        Assert.NotEmpty(errors);
        Assert.Null(baz);
        Console.WriteLine(errors.Single());
    }

    [Fact]
    public void ComplexTest()
    {
        Converter converter = new();
        converter.RegisterConverters(typeof(PositiveInt).Assembly);
        var options = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        var (foo, errors) = ErrorCollector<Foo>
            .Build(converter)
            .With(v => v.Name, "Duke Nukem")
            .With(v => v.Age, 27)
            .With(v => v.Score, 999)
            .With(v => v.Bar, (int?)null)
            .WithNested(v => v.Baz, (nested) =>
            {
                nested.With(v => v.SomeInt, 900);
            })
            .Result();

        Console.WriteLine(JsonSerializer.Serialize(foo));
        Console.WriteLine(JsonSerializer.Serialize
            (errors.Select(e => $"{e.Message}"),
             options: options));


        (foo, errors) = ErrorCollector<Foo>
           .Build(converter)
           .With(v => v.Name, "Doom Guy")
           .With(v => v.Age, 100500)
           .With(v => v.Score, -666)
           .With(v => v.Bar, (int?)900)
           .WithNested(v => v.Baz, (nested) =>
                {
                    nested.With(v => v.SomeInt, -1);
                })
           .Result();

        Console.WriteLine(JsonSerializer.Serialize(foo));
        Console.WriteLine(JsonSerializer.Serialize
            (errors.Select(e => $"{e.Message}"),
             options: options));

        (foo, errors) = ErrorCollector<Foo>
           .Build(converter)
           .With(v => v.Name, "Test subject #42")
           .With(v => v.Age, 42)
           .With(v => v.Score, (int?)null)
           .Result();

        Console.WriteLine(JsonSerializer.Serialize(foo));
        Console.WriteLine(JsonSerializer.Serialize
            (errors.Select(e => $"{e.Message}"),
             options: options));
    }
}