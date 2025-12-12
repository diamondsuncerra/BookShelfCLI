using BookShelf.Application;
using BookShelf.Application.Commands;
using BookShelf.Application.Commands.Handlers;
using BookShelf.Application.Commands.Models;
using BookShelf.ConsoleUI.UIMessages;

namespace BookShelf.ConsoleUI
{
    public class CommandRouter
    {
        IBookService _bookService = new BookService();
        public Result<object> Route(string input)
        {
            char[] delimiters = { ',', ';', '|', ' ' };
            string[] tokens = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (tokens is null)
                return Result<object>.Fail("No command. Type 'help' for usage.");

            var commandType = tokens[0];
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
        private Result<object> HandleAdd(string[] tokens)
        {
            EnsureTokenLength(tokens, 7); // if this throws catch it in the Route
            var type = tokens[1];
            if(type.Equals("ebook"))
            {
                var title = tokens[2];
                var author = tokens[3];
                int.TryParse(tokens[4], out int year);
                var fileFormat = tokens[5];
                int.TryParse(tokens[6], out int fileSizeMb);
                AddEBookCommand addEBookCommand = new(title, author, year, fileFormat, fileSizeMb);
                AddEBookCommand handler = new AddEBookHandler();
            } else if (type.Equals("physical"))
            {
                
            } else throw new ArgumentException(ErrorMessages.InscorrectParameters);
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

        private static void EnsureTokenLength(string[] tokens, int expected)
        {
            if (tokens.Length < expected)
                throw new Exception(ErrorMessages.InsufficientParameters);
        }

    }
}
