namespace BookShelf.Application.Commands
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public string Message { get; }

        protected Result(bool isSuccess, string error, string message)
        {
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }
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
            => new Result<T>(value, true, null, message);

        public static Result<T> Fail(string error)
            => new Result<T>(default!, false, error, null);
    }

}
