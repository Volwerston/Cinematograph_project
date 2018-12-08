using System;

namespace Common.Domain
{
    public class Result
    {
        private static readonly Result CachedOk = new Result();

        public Error Error { get; set; }

        public bool IsSuccess => Error is null;

        protected Result() { }

        private Result(Error error)
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public static Result Ok() => CachedOk;
        public static Result Fail(Error error) => new Result(error);

        public static implicit operator Result(Error error) => Fail(error);
    }

    public sealed class Result<T> : Result
    {
        private readonly T _value;

        private Result(T value)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (IsSuccess)
                {
                    return _value;
                }

                throw new InvalidOperationException(
                    $"Cannot access '{nameof(Value)}' of failed '{nameof(Result)}'");
            }
        }

        public static Result<T> Ok(T value) => new Result<T>(value);

        public static implicit operator Result<T>(T value) => Ok(value);
    }
}
