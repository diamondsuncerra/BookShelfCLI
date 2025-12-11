namespace BookShelf.Application.Commands
{
    public sealed class CommandResult<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        private CommandResult(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static CommandResult<T> Ok(T data, string message = "")
            => new(true, message, data);

        public static CommandResult<T> Fail(string message)
            => new(false, message);
    }
}
