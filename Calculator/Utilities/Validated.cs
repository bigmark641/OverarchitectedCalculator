using System;

namespace Calculator.Utilities
{

    // A functor that holds EITHER a value OR an error.
    public class Validated<T>
    {
        private T Value { get; }
        private Error Error { get; }

        public Validated(T value)
        {
            Value = value;
        }

        public Validated(Error error)
        {
            Error = error;
        }

        public override bool Equals(Object obj)
        {
            return this.Value.Equals(((Validated<T>)obj).Value)
                && ((this.Error == null && ((Validated<T>)obj).Error == null) 
                    || this.Error.Equals(((Validated<T>)obj).Error));
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() + Error.GetHashCode();
        }

        public static implicit operator Validated<T>(Error error)
            => new Validated<T>(error);

        public R Match<R>(Func<T, R> ifValue, Func<Error, R> ifError)
            => Error == null
                ? ifValue(Value)
                : ifError(Error);

        public Validated<R> Map<R>(Func<T, R> func)
            => Match<Validated<R>>(
                ifValue: x => new Validated<R>(func(x)),
                ifError: x => x);

        public Validated<R> Bind<R>(Func<T, Validated<R>> func)
            => Match(
                ifValue: x => func(x),
                ifError: x => x);
    }

    // A degenerate Validated<T> that cannot contain a Value, Map, or Match.
    public class Validated
    {
        private Error Error { get; }

        public Validated() { }

        public Validated(Error error)
        {
            Error = error;
        }

        public override bool Equals(Object obj)
        {
            return (this.Error == null && ((Validated)obj).Error == null)
                || this.Error.Equals(((Validated)obj).Error);
        }

        public override int GetHashCode()
        {
            return Error.GetHashCode();
        }

        public static implicit operator Validated(Error error)
            => new Validated(error);

        public Validated<R> Bind<R>(Func<Validated<R>> func)
            => Error == null
                ? func()
                : Error;
    }
}