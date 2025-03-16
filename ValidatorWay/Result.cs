namespace ValidatorWay;

public abstract record Result
{
    public record Success(object? Value) : Result;
    public record Error(string Message) : Result;
}
