using System.Linq.Expressions;
using System.Reflection;

namespace ValidatorWay;

public class ErrorCollector<T>
where T : new()
{
    private readonly T? _underConstruction;
    private List<Error> _errors;
    private string _path;

    private ErrorCollector(List<Error> errors, string path)
    {
        _errors = errors;
        _path = path;
        _underConstruction = new();
    }

    public static ErrorCollector<T> Build()
    {
        return new ErrorCollector<T>([], string.Empty);
    }

    public ErrorCollector<T> With<TProperty, TRaw>(Expression<Func<T, TProperty>> propertyPicker, TRaw raw)
    {
        if (propertyPicker.Body is not MemberExpression memberSelectorExpression ||
            memberSelectorExpression.Member is not PropertyInfo property)
        {
            throw new InvalidOperationException();
        }

        var conversionResult = Converter.TryConvert<TProperty, TRaw>(raw);
        if (conversionResult is Result.Success<TProperty> success)
        {
            property.SetValue(_underConstruction, success.Value, null);
        }
        else
        {
            var errorMsg = (Result.Error)conversionResult;
            string propertyPath = string.IsNullOrEmpty(_path) ? property.Name: $"{_path}.{property.Name}";
            _errors.Add(new Error(Message: $"Error when setting property [{propertyPath}].\r\n{errorMsg.Message}"));
        }
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
