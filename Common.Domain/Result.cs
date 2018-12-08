namespace Common.Domain
{
    public class Result<T>
    {
        public Error Error { get; }
        public T Value { get; }

        public bool IsSuccess => Error is null;

        private Result(T value)
        {
            Value = value;
        }

        private Result(Error error)
        {
            Error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(value);
        public static Result<T> Fail(Error error) => new Result<T>(error);

        public static implicit operator Result<T>(T value) => Ok(value);
        public static implicit operator Result<T>(Error error) => Fail(error);
    }
}
