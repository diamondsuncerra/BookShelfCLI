namespace BookShelf.Application.Commands
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public string? Message { get; }

        protected Result(bool isSuccess, string error, string message)
        {
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }

        public static Result Ok(string message = "")
            => new(true, null, message);

        public static Result Fail(string error)
            => new(false, error, null);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool isSuccess, string error, string message)
            : base(isSuccess, error, message)
        {
            Value = value;
        }

        public static Result<T> Ok(T value, string message = "")
            => new(value, true, null, message);

        public static Result<T> Fail(string error)
            => new(default!, false, error, null);
    }
}
