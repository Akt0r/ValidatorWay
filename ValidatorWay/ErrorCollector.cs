using System.Linq.Expressions;
using System.Reflection;

namespace ValidatorWay;

public class ErrorCollector<T> where T : new()
{
    private T? _value;
    private readonly List<Error> _errors = [];

    private ErrorCollector()
    {
    }

    public static ErrorCollector<T> Build()
    {
        return new ErrorCollector<T>() { _value = new() };
    }

    public ErrorCollector<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory)
    {
        if (propertyPicker.Body is MemberExpression memberSelectorExpression)
        {
            if (memberSelectorExpression.Member is PropertyInfo property)
            {
                try
                {
                    property.SetValue(_value, valueFactory(), null);
                }
                catch (Exception ex)
                {
                    _errors.Add(new Error(Message: $"Error when setting property [{property.Name}]", Exception: ex));
                }
            }
        }
        return this;
    }

    public (T?, List<Error>) Result()
    {
        return _errors.Count switch
        {
            0 => (_value, _errors),
            _ => (default, _errors),
        };
    }
}
