namespace BookShelf.ConsoleUI.UIMessages
{
    public sealed class UiResult
    {
        public bool IsSuccess { get; }
        public string? Output { get; }
        public string? Error { get; }
        public bool ShouldExit { get; }

        private UiResult(bool isSuccess, string? output, string? error, bool shouldExit)
        {
            IsSuccess = isSuccess;
            Output = output;
            Error = error;
            ShouldExit = shouldExit;
        }

        public static UiResult Ok(string? output = null)
            => new(true, output, null, false);

        public static UiResult Fail(string error)
            => new(false, null, error, false);

        public static UiResult Exit(string? farewell = "GoodBye!")
            => new(true, farewell, null, true);
    }
}
