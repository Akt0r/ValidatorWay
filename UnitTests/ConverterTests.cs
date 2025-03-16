using ModelApp;
using ValidatorWay;

namespace UnitTests;

public class ConverterTests
{
    private readonly Converter _sut = new();

    public ConverterTests()
    {
        _sut.RegisterConverters(typeof(PositiveInt).Assembly);
    }

    [Fact]
    public void ScraperPositiveTest()
    {
        Assert.True(_sut._converters.Count == 1);
    }

    [Fact]
    public void ConvertNull_ToNullableResult_Success()
    {
        var result = _sut.TryConvert<int?>(null, typeof(PositiveInt?));
        Assert.True(result is Result.Success);
    }

    [Fact]
    public void ConvertValue_ToNullableResult_Success()
    {
        var result = _sut.TryConvert<int?>(42, typeof(PositiveInt?));
        Assert.True(result is Result.Success);
        Assert.Equal(42, (PositiveInt)((Result.Success)result).Value!);
    }

    [Fact]
    public void ConvertValue_FromNullableToNonNullableResult_Success()
    {
        var result = _sut.TryConvert<int?>(42, typeof(PositiveInt));
        Assert.True(result is Result.Success);
        Assert.Equal(42, (PositiveInt)((Result.Success)result).Value!);
    }

    [Fact]
    public void ConvertNull_ToNonNullableResult_Error()
    {
        var result = _sut.TryConvert<int?>(null, typeof(PositiveInt));
        Assert.True(result is Result.Error);
    }

    [Fact]
    public void ConvertValue_FromNonNullableToNonNullableResult_Success()
    {
        var result = _sut.TryConvert(42, typeof(PositiveInt));
        Assert.True(result is Result.Success);
        Assert.Equal(42, (PositiveInt)((Result.Success)result).Value!);
    }
}
