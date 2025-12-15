using BookShelf.Application;
using BookShelf.Application.Commands;
using BookShelf.Application.Commands.Handlers;
using BookShelf.Application.Commands.Models;
using BookShelf.Application.Services;
using BookShelf.ConsoleUI.UIMessages;
using BookShelf.Domain.Factories;
using BookShelf.Domain.Repositories;
using BookShelf.Infrastructure.Factory;
using BookShelf.Infrastructure.Repository;

namespace BookShelf.ConsoleUI
{
    public class CommandRouter
    {
        private readonly IBookFactory _bookFactory;
        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;

        public CommandRouter()
        {
            _bookFactory = new BookFactory();
            _bookRepository = new InMemoryBookRepository();
            _bookService = new BookService(_bookFactory, _bookRepository);
        }
        public Result Route(string input)
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
        private Result HandleAdd(string[] tokens)
        {
            EnsureTokenLength(tokens, 7); // if this throws catch it in the Route
            var type = tokens[1];
            if (type.Equals("ebook"))
            {
                var title = tokens[2];
                var author = tokens[3];
                var fileFormat = tokens[5];
                if (!int.TryParse(tokens[4], out int year))
                    return Result<object>.Fail(ErrorMessages.InvalidYear);
                if (!int.TryParse(tokens[6], out int fileSizeMb))
                    return Result<object>.Fail(ErrorMessages.InvalidFileSize);

                AddEBookCommand addEBookCommand = new(title, author, year, fileFormat, fileSizeMb);
                AddEBookHandler handler = new(_bookService);
                return handler.Handle(addEBookCommand);
            }
            else if (type.Equals("physical"))
            {
                var title = tokens[2];
                var author = tokens[3];
                var isbn13 = tokens[5];
                if (!int.TryParse(tokens[4], out int year))
                    return Result<object>.Fail(ErrorMessages.InvalidYear);

                if (!int.TryParse(tokens[6], out int pages))
                    return Result<object>.Fail(ErrorMessages.InvalidPageNumber);
                    
                AddPhysicalBookCommand addPhysicalBookCommand = new(title, author, year, isbn13, pages);
                AddPhysicalBookHandler handler = new(_bookService);
                return handler.Handle(addPhysicalBookCommand);
            }
            else throw new ArgumentException(ErrorMessages.IncorrectParameters);
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
