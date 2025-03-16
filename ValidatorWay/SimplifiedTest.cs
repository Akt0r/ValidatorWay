using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ValidatorWay;

public class SimplifiedTest
{

    public class Program
    {
        public static void Main()
        {
            var x = CreateWrapper<int>(42);

            TestType<int>(x, nameof(x));

            var foo = new Foo() { A = 42, B = 88 };
            var fooWrapper = new PropertiesWrapper<Foo>(foo);

            var wrappedA = fooWrapper.Wrap(x => x.A);
            fooWrapper.UnWrap(x => x.A, wrappedA);
            var wrappedB = fooWrapper.Wrap(x => x.B);
            fooWrapper.UnWrap(x => x.B, wrappedB);

            var foo2 = new Foo() { A = 42, B = null };
            var fooWrapper2 = new PropertiesWrapper<Foo>(foo2);

            var wrappedA2 = fooWrapper2.Wrap(x => x.A);
            fooWrapper.UnWrap(x => x.A, wrappedA2);
            var wrappedB2 = fooWrapper2.Wrap(x => x.B);
            fooWrapper.UnWrap(x => x.B, wrappedB2);
        }

        public static T? TestType<T>(Maybe x, string name)
        {
            if (x is Maybe.Wrapper<T> a)
            {
                Console.WriteLine($"{name} is wrapped in {typeof(T)} {a}");
                return a.Value;
            }

            if (x is Maybe.Wrapper<T?> b)
            {
                Console.WriteLine($"{name} is wrapped in nullable {typeof(T)} {b}");
                return b.Value;
            }
            throw new NotImplementedException();
        }

        public abstract record Maybe
        {
            public record Wrapper<T>(T Value) : Maybe;
        }

        public static Maybe CreateWrapper<T>(T? val)
        {
            return new Maybe.Wrapper<T?>(val);
        }

        public class PropertiesWrapper<T>(T instance)
        {
            public Maybe Wrap<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
            {
                if (propertyPicker.Body is not MemberExpression memberSelectorExpression ||
                    memberSelectorExpression.Member is not PropertyInfo property)
                {
                    throw new InvalidOperationException();
                }
                return new Maybe.Wrapper<TProperty?>((TProperty?)property.GetValue(instance));
            }

            public TProperty? UnWrap<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Maybe val)
            {
                if (propertyPicker.Body is not MemberExpression memberSelectorExpression ||
                    memberSelectorExpression.Member is not PropertyInfo property)
                {
                    throw new InvalidOperationException();
                }
                return TestType<TProperty>(val, property.Name);
            }
        }

        public class Foo
        {
            public int A { get; set; }
            public int? B { get; set; }
        }
    }
}
