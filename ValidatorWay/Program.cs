// See https://aka.ms/new-console-template for more information
using System.Text.Encodings.Web;
using System.Text.Json;
using ValidatorWay;

Console.WriteLine("Hello, World!");
var options = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
var (foo, errors) = ErrorCollector<Foo>
    .Build()
    .With(v => v.Name, "Duke Nukem")
    .With(v => v.Age, 27)
    .With(v => v.Score, 999)
    .With(v => v.Bar, (int?)null)
    .Result();

Console.WriteLine(JsonSerializer.Serialize(foo));
Console.WriteLine(JsonSerializer.Serialize
    (errors.Select(e => $"{e.Message}"),
     options: options));


(foo, errors) = ErrorCollector<Foo>
   .Build()
   .With(v => v.Name, "Doom Guy")
   .With(v => v.Age, 100500)
   .With(v => v.Score, -666)
   .With(v => v.Bar, (int?)900)
   .Result();

Console.WriteLine(JsonSerializer.Serialize(foo));
Console.WriteLine(JsonSerializer.Serialize
    (errors.Select(e => $"{e.Message}"),
     options: options));

(foo, errors) = ErrorCollector<Foo>
   .Build()
   .With(v => v.Name, "Test subject #42")
   .With(v => v.Age, 42)
   .With(v => v.Score, (int?)null)
   .Result();

Console.WriteLine(JsonSerializer.Serialize(foo));
Console.WriteLine(JsonSerializer.Serialize
    (errors.Select(e => $"{e.Message}"),
     options: options));