namespace BookShelf.Application.Commands
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public string? Message { get; }
        public T? Value { get; }

        private Result(bool isSuccess, T? value, string? error, string? message)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            Message = message;
        }

        public static Result<T> Ok(T value, string message = "")
            => new(true, value, null, message);

        public static Result<T> Fail(string error)
            => new(false, default, error, null);
    }
}
