using BookShelf.Application.Commands;

namespace BookShelf.ConsoleUI
{
    public class CommandRouter
    {
        public Result<object> Route(string input)
        {
            char[] delimiters = { ',', ';', '|', ' ' };
            string[] tokens = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (tokens is null)
                return Result<object>.Fail("No command. Type 'help' for usage.");

            var commandType = tokens[1];
            switch (commandType)
            {
                case "add": return HandleAdd(tokens);

                case "list":
                    return HandleList(tokens);

                case "find":
                    return HandleFind(tokens);

                case "sort":
                    return HandleSort(tokens);

                case "remove":
                    return HandleRemove(tokens);

                case "report":
                    return HandleReport(tokens);

                case "help":
                    return Result<object>.Ok(null, "Available commands: add, list, find, sort, remove, report, help, exit");

                case "exit":
                    // UI will handle actual exit; router just returns success
                    return Result<object>.Ok(null, "exit");

                default:
                    return Result<object>.Fail($"Unknown command '{commandType}'. Type 'help' for usage.");
            }
    
        }

        private Result<object> HandleReport(string[] tokens)
        {
            throw new NotImplementedException();
        }

        private Result<object> HandleRemove(string[] tokens)
        {
            throw new NotImplementedException();
        }

        private Result<object> HandleSort(string[] tokens)
        {
            throw new NotImplementedException();
        }

        private Result<object> HandleFind(string[] tokens)
        {
            throw new NotImplementedException();
        }

        private Result<object> HandleList(string[] tokens)
        {
            throw new NotImplementedException();
        }

        private Result<object> HandleAdd(string[] tokens)
        {
            throw new NotImplementedException();
        }
    }
}
