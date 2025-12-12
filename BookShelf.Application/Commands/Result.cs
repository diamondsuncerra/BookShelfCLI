namespace BookShelf.Application.Commands
{
    public sealed class Result<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        private Result(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static Result<T> Ok(T data, string message = "")
            => new(true, message, data);

        public static Result<T> Fail(string message)
            => new(false, message);
    }
}
