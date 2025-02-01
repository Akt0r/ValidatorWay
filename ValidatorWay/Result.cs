namespace ValidatorWay;

public abstract record Result
{
    public record Success<T>(T Value):Result;
    public record Error(string Message):Result;
}
