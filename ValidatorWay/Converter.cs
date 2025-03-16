using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace ValidatorWay;

public class Converter
{
    internal readonly Dictionary<(Type Raw, Type Result), Delegate> _converters = [];

    private void RegisterConverter(Type raw, Type result, Delegate converter)
    {
        _converters[(raw, result)] = converter;
    }

    public void RegisterConverters(Assembly assembly)
    {
        foreach (MethodInfo method in assembly.ExportedTypes
            .SelectMany(t => t.GetMethods())
            .Where(m => m.IsStatic && m.ReturnType == typeof(Result))
               )
        {
            // TODO: Add multiple input parameters support
            var inputParameterType = method.GetParameters().Single().ParameterType!;
            var delegateType = Expression.GetDelegateType(inputParameterType, typeof(Result));
            var invocable = method.CreateDelegate(delegateType)!;
            Type targetType = IsNullable(inputParameterType)
              ? typeof(Nullable<>).MakeGenericType(method.DeclaringType!)
              : method.DeclaringType!;
            RegisterConverter(inputParameterType, targetType, invocable);
        }
    }

    private static bool IsNullable(Type inputParameterType)
    {
        return Nullable.GetUnderlyingType(inputParameterType) != null;
    }

    public Result TryConvert<TRaw>(TRaw raw, Type target)
    {
        if (!_converters.TryGetValue((typeof(TRaw), target), out Delegate? converter))
        {
            var nullableTarget = IsNullable(target)
                ? target
                : typeof(Nullable<>).MakeGenericType(target);

            var nullableRaw = IsNullable(typeof(TRaw))
                ? typeof(TRaw)
                : typeof(Nullable<>).MakeGenericType(typeof(TRaw));

            if (!_converters.TryGetValue((nullableRaw, nullableTarget), out converter))
            {
                throw new InvalidOperationException($"There is no registered converter for convertion {typeof(TRaw)} -> {target}");
            }
            // Found converter to nullable version of TResult
        }
        var result = converter.DynamicInvoke(raw)!;
        if (result is Result.Error error)
        {
            return error;
        }

        if (result is Result.Success success)
        {
            if (success.Value == null && !IsNullable(target))
            {
                return new Result.Error("Value is mandatory");
            }
            return success;
        }

        throw new NotImplementedException();
    }
}
