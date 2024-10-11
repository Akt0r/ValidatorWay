// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using ValidatorWay;

Console.WriteLine("Hello, World!");
var (foo, errors) = ErrorCollector<Foo>
    .Build()
    .With(v => v.Name, () => "Duke Nukem")
    .With(v => v.Age, () => 27)
    .With<PositiveInt>(v => v.Score, () => 999)
    .With(v => v.Bar, () => null)
    .Result();

Console.WriteLine(JsonSerializer.Serialize(foo));
Console.WriteLine(JsonSerializer.Serialize(errors.Select(e => e.Message)));


 (foo, errors) = ErrorCollector<Foo>
    .Build()
    .With(v => v.Name, () => "Doom Guy")
    .With(v => v.Age, () => 100500)
    .With<PositiveInt>(v => v.Score, () => -666)
    .With(v => v.Bar, () => null)
    .Result();

Console.WriteLine(JsonSerializer.Serialize(foo));
Console.WriteLine(JsonSerializer.Serialize(errors.Select(e => e.Message)));