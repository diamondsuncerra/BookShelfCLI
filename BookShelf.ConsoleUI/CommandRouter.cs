using System.Globalization;
using System.Text;
using BookShelf.Application;
using BookShelf.Application.Commands;
using BookShelf.Application.Commands.Enums;
using BookShelf.Application.Commands.Handlers;
using BookShelf.Application.Commands.Models;
using BookShelf.Application.Services;
using BookShelf.ConsoleUI.UIMessages;
using BookShelf.Domain.Factories;
using BookShelf.Domain.Reports;
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
                    return Result.Fail(ErrorMessages.NoCommandGiven);

                string[] tokens = Tokenize(input);

                if (tokens.Length == 0)
                    return Result.Fail(ErrorMessages.NoCommandGiven);

                var commandType = tokens[0].ToLowerInvariant();

                return commandType switch
                {
                    "add"    => HandleAdd(tokens),
                    "list"   => HandleList(),
                    "find"   => HandleFind(tokens),
                    "sort"   => HandleSort(tokens),
                    "remove" => HandleRemove(tokens),
                    "report" => HandleReport(tokens),
                    "help"   => Result.Ok(UIHelperMessages.AvailableCommands),
                    "exit"   => Result.Ok(CommandsOrFields.Exit),
                    _        => Result.Fail($"Unknown command '{commandType}'. Type 'help' for usage.")
                };
            }
            catch
            {
                return Result.Fail(ErrorMessages.UnexpectedError);
            }
        }


        private Result HandleAdd(string[] tokens)
        {
            if (tokens.Length < 7)
                return Result.Fail(ErrorMessages.InsufficientParameters);

            var type = tokens[1];

            if (type.Equals(CommandsOrFields.Ebook, StringComparison.OrdinalIgnoreCase))
            {
                var title      = tokens[2];
                var author     = tokens[3];
                var fileFormat = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return Result.Fail(ErrorMessages.InvalidYear);

                if (!decimal.TryParse(tokens[6], NumberStyles.Number, CultureInfo.InvariantCulture, out var fileSizeMb) ||
                    fileSizeMb < 0)
                    return Result.Fail(ErrorMessages.InvalidFileSize);

                var command = new AddEBookCommand(title, author, year, fileFormat, fileSizeMb);
                var handler = new AddEBookHandler(_bookService);

                return handler.Handle(command);
            }

            if (type.Equals(CommandsOrFields.Physical, StringComparison.OrdinalIgnoreCase))
            {
                var title  = tokens[2];
                var author = tokens[3];
                var isbn13 = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return Result.Fail(ErrorMessages.InvalidYear);

                if (!int.TryParse(tokens[6], out int pages) || pages <= 0)
                    return Result.Fail(ErrorMessages.InvalidPageNumber);

                var command = new AddPhysicalBookCommand(title, author, year, isbn13, pages);
                var handler = new AddPhysicalBookHandler(_bookService);

                return handler.Handle(command);
            }
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }

        private Result HandleList()
        {
            var handler = new ListBooksHandler(_bookService);
            return handler.Handle(new ListBooksCommand());
        }
        private Result HandleFind(string[] tokens)
        {
            if (tokens.Length < 3)
                return Result.Fail(ErrorMessages.InsufficientParameters);

            if (!Enum.TryParse<FindField>(tokens[1], true, out var field))
                return Result.Fail(ErrorMessages.IncorrectParameters);

            var searchTerm = tokens[2];

            if (string.IsNullOrWhiteSpace(searchTerm))
                return Result.Fail(ErrorMessages.IncorrectParameters);

            var handler = new FindBooksHandler(_bookService);
            var command = new FindBooksCommand(field, searchTerm);

            return handler.Handle(command);
        }
        private Result HandleSort(string[] tokens)
        {
            if (tokens.Length < 2)
                return Result.Fail(ErrorMessages.InsufficientParameters);

            if (Enum.TryParse<SortField>(tokens[1], true, out var strategy))
            {
                var handler = new SortBooksHandler(_bookService);
                var command = new SortBooksCommand(strategy);
                return handler.Handle(command);
            }
            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private Result HandleRemove(string[] tokens)
        {
            if (tokens.Length < 2)
                return Result.Fail(ErrorMessages.InsufficientParameters);

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

            if (tokens.Length < 2)
                return Result.Fail(ErrorMessages.InsufficientParameters);

            if (!Enum.TryParse<ReportType>(tokens[1], true, out var reportType))
                return Result.Fail(ErrorMessages.IncorrectParameters);

            if (reportType == ReportType.Catalog)
            {
                var handler = new ReportCatalogHandler(_bookService);
                return handler.Handle(new ReportCatalogCommand());
            }

            if (reportType == ReportType.Summary)
            {
                var handler = new ReportSummaryHandler(_bookService);
                return handler.Handle(new ReportSummaryCommand());
            }

            return Result.Fail(ErrorMessages.IncorrectParameters);
        }
        private static string[] Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in input)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue; 
                }

                if (char.IsWhiteSpace(c) && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
                tokens.Add(current.ToString());

            return [.. tokens];
        }
    }
}
