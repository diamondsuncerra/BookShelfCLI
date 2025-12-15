using BookShelf.Application;
using BookShelf.Application.Commands;
using BookShelf.Application.Commands.Enums;
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
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return Result.Fail("No command. Type 'help' for usage.");

                char[] delimiters = { ',', ';', '|', ' ' }; // o sa avem probleme la "clean code"
                string[] tokens = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length == 0)
                    return Result.Fail("No command. Type 'help' for usage.");

                var commandType = tokens[0].ToLowerInvariant();

                return commandType switch
                {
                    "add" => HandleAdd(tokens),
                    "list" => HandleList(tokens),
                    "find" => HandleFind(tokens),
                    "sort" => HandleSort(tokens),
                    "remove" => HandleRemove(tokens),
                    "report" => HandleReport(tokens),
                    "help" => Result.Ok("Available commands: add, list, find, sort, remove, report, help, exit"),
                    "exit" => Result.Ok("exit"),
                    _ => Result.Fail($"Unknown command '{commandType}'. Type 'help' for usage.")
                };
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        private Result HandleAdd(string[] tokens)
        {
            EnsureTokenLength(tokens, 7); // throws => caught in Route as unexpected error

            var type = tokens[1];

            if (type.Equals("ebook", StringComparison.OrdinalIgnoreCase))
            {
                var title = tokens[2];
                var author = tokens[3];
                var fileFormat = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return Result.Fail(ErrorMessages.InvalidYear);

                if (!int.TryParse(tokens[6], out int fileSizeMb))
                    return Result.Fail(ErrorMessages.InvalidFileSize);

                var command = new AddEBookCommand(title, author, year, fileFormat, fileSizeMb);
                var handler = new AddEBookHandler(_bookService);

                return handler.Handle(command);
            }

            if (type.Equals("physical", StringComparison.OrdinalIgnoreCase))
            {
                var title = tokens[2];
                var author = tokens[3];
                var isbn13 = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return Result.Fail(ErrorMessages.InvalidYear);

                if (!int.TryParse(tokens[6], out int pages))
                    return Result.Fail(ErrorMessages.InvalidPageNumber);

                var command = new AddPhysicalBookCommand(title, author, year, isbn13, pages);
                var handler = new AddPhysicalBookHandler(_bookService);

                return handler.Handle(command);
            }

            // Type validation is still an expected failure.
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private Result HandleList(string[] tokens)
        {
            var handler = new ListBooksHandler(_bookService);
            return handler.Handle(new ListBooksCommand());
        }

        private Result HandleFind(string[] tokens)
        {
            EnsureTokenLength(tokens, 3);
            if (Enum.TryParse<FindField>(tokens[1], true, out var strategy))
            {
                var handler = new FindBooksHandler(_bookService);
                return handler.Handle(new FindBooksCommand(strategy, tokens[2]));
            }
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private Result HandleSort(string[] tokens)
        {
            EnsureTokenLength(tokens, 2);
            if (Enum.TryParse<SortField>(tokens[1], true, out var strategy))
            {
                var handler = new SortBooksHandler(_bookService);
                return handler.Handle(new SortBooksCommand(strategy));
            }
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private Result HandleRemove(string[] tokens)
        {
            EnsureTokenLength(tokens, 2);

            if (Guid.TryParse(tokens[1], out Guid guid))
            {
                var handler = new RemoveBookHandler(_bookService);
                var command = new RemoveBookCommand(guid);
                return handler.Handle(command);
            }

            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private Result HandleReport(string[] tokens)
        {
            EnsureTokenLength(tokens, 2);

            if (tokens[1].Equals("catalog", StringComparison.OrdinalIgnoreCase))
            {
                var handler = new ReportCatalogHandler(_bookService);
                return handler.Handle(new ReportCatalogCommand());
            }
            else if (tokens[1].Equals("summary", StringComparison.OrdinalIgnoreCase))
            {
                var handler = new ReportSummaryHandler(_bookService);
                return handler.Handle(new ReportSummaryCommand());
            }
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private static void EnsureTokenLength(string[] tokens, int expected)
        {
            if (tokens.Length < expected)
                throw new Exception(ErrorMessages.InsufficientParameters);
        }
    }
}
