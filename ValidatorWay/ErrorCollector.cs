using System.Linq.Expressions;
using System.Reflection;

namespace ValidatorWay;

public class ErrorCollector<T>
where T : new()
{
    private readonly T? _underConstruction;
    private readonly List<Error> _errors;
    private readonly List<string> _pathPieces;
    private readonly Converter _converter;

    private ErrorCollector(Converter converter, List<Error> errors, List<string> pathPieces)
    {
        _errors = errors;
        _pathPieces = pathPieces;
        _underConstruction = new();
        _converter = converter;
    }

    public static ErrorCollector<T> Build(Converter converter)
    {
        return new ErrorCollector<T>(converter, [], []);
    }

    public ErrorCollector<T> With<TProperty, TRaw>(Expression<Func<T, TProperty?>> propertyPicker, TRaw raw)
    {
        if (propertyPicker.Body is not MemberExpression memberSelectorExpression ||
            memberSelectorExpression.Member is not PropertyInfo property)
        {
            throw new InvalidOperationException();
        }
        string? errorMsg = null;
        var conversionResult = _converter.TryConvert(raw, typeof(TProperty));

        if(conversionResult is Result.Error error)
        {
            errorMsg = error.Message;
        } 
        else if(conversionResult is Result.Success success)
        {
            property.SetValue(_underConstruction, success.Value, null);
        }

        if (errorMsg != null)
        {
            string propertyPath = string.Join('.', _pathPieces.Concat([property.Name]));
            _errors.Add(new Error(Message: $"Error when setting property [{propertyPath}].\r\n{errorMsg}"));
        }
        return this;
    }

    public ErrorCollector<T> WithNested<TProperty>(
        Expression<Func<T, TProperty>> propertyPicker, Action<ErrorCollector<TProperty>> callback)
        where TProperty : new()
    {
        if (propertyPicker.Body is not MemberExpression memberSelectorExpression ||
            memberSelectorExpression.Member is not PropertyInfo property)
        {
            throw new InvalidOperationException();
        }
        //TODO use linked list of sorts to avoid copying
        var childPieces = _pathPieces.ToList();
        childPieces.Add(property.Name);
        ErrorCollector<TProperty> nested = new(_converter, _errors, childPieces);

        property.SetValue(_underConstruction, nested._underConstruction, null);
        callback(nested);

        return this;
    }

    public (T?, List<Error>) Result()
    {
        return _errors.Count switch
        {
            0 => (_underConstruction, _errors),
            _ => (default, _errors),
        };
    }
}
