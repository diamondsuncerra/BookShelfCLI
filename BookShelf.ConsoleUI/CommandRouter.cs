using System;
using System.Globalization;
using System.Text;
using BookShelf.Application;
using BookShelf.Application.Commands;
using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Enums;
using BookShelf.Application.Commands.Handlers;
using BookShelf.Application.Commands.Models;
using BookShelf.ConsoleUI.Decorators;
using BookShelf.ConsoleUI.UIMessages;
using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI
{
    public class CommandRouter(IBookService bookService, ICommandHistory commandHistory)
    {
        private readonly IBookService _bookService = bookService;
        private readonly ICommandHistory _commandHistory = commandHistory;

        public UiResult Route(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    return UiResult.Fail(ErrorMessages.NoCommandGiven);

                string[] tokens = Tokenize(input);

                if (tokens.Length == 0)
                    return UiResult.Fail(ErrorMessages.NoCommandGiven);

                var commandName = tokens[0].ToLowerInvariant();

                return commandName switch
                {
                    "add" => HandleAdd(tokens),
                    "list" => HandleList(),
                    "find" => HandleFind(tokens),
                    "sort" => HandleSort(tokens),
                    "remove" => HandleRemove(tokens),
                    "report" => HandleReport(tokens),
                    "undo" => HandleUndo(),
                    "help" => UiResult.Ok(UIHelperMessages.AvailableCommands),
                    "exit" => UiResult.Exit(),
                    _ => UiResult.Fail($"Unknown command '{commandName}'. Type 'help' for usage.")
                };
            }
            catch
            {
                return UiResult.Fail(ErrorMessages.UnexpectedError);
            }
        }

        private UiResult HandleUndo()
        {
            var undoable = _commandHistory.Pop();
            if (undoable is null)
                return UiResult.Fail("Nothing to undo.");
            undoable.Undo();
            return UiResult.Ok("Previous undoable operation was retracted.");
        }

        private UiResult HandleAdd(string[] tokens)
        {
            if (tokens.Length < 7)
                return UiResult.Fail(ErrorMessages.InsufficientParameters);

            var bookType = tokens[1];

            if (bookType.Equals(CommandsOrFields.Ebook, StringComparison.OrdinalIgnoreCase))
            {
                var title = tokens[2];
                var author = tokens[3];
                var fileFormat = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return UiResult.Fail(ErrorMessages.InvalidYear);

                if (!decimal.TryParse(tokens[6], NumberStyles.Number, CultureInfo.InvariantCulture, out var fileSizeMb) ||
                    fileSizeMb < 0)
                    return UiResult.Fail(ErrorMessages.InvalidFileSize);

                var command = new AddEBookCommand(title, author, year, fileFormat, fileSizeMb);
                // intrebare -> e mai ok aici sau ca fields. GPT a zis ca e mai ok aici pt ca 
                // sunt mici si nu e mare pb de performanta?
                var handler = new AddEBookHandler(_bookService);
                var result = handler.Handle(command); // Result<Guid>

                if (!result.IsSuccess)
                    return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

                _commandHistory.Push(handler);
                return UiResult.Ok($"Ebook added with Id: {result.Value}");

            }

            if (bookType.Equals(CommandsOrFields.Physical, StringComparison.OrdinalIgnoreCase))
            {
                var title = tokens[2];
                var author = tokens[3];
                var isbn13 = tokens[5];

                if (!int.TryParse(tokens[4], out int year))
                    return UiResult.Fail(ErrorMessages.InvalidYear);

                if (!int.TryParse(tokens[6], out int pages) || pages <= 0)
                    return UiResult.Fail(ErrorMessages.InvalidPageNumber);

                var command = new AddPhysicalBookCommand(title, author, year, isbn13, pages);
                var handler = new AddPhysicalBookHandler(_bookService);
                var result = handler.Handle(command); // Result<Guid>

                if (!result.IsSuccess)
                    return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);
                _commandHistory.Push(handler);
                return UiResult.Ok($"Physical book added with Id: {result.Value}");
            }

            return UiResult.Fail(ErrorMessages.IncorrectParameters);
        }

        private UiResult HandleList()
        {
            var handler = new ListBooksHandler(_bookService);
            var result = handler.Handle(new ListBooksCommand()); // Result<IReadOnlyList<Book>>

            if (!result.IsSuccess)
                return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

            var output = FormatBookList(result.Value);
            return UiResult.Ok(output);
        }

        private UiResult HandleFind(string[] tokens)
        {
            if (tokens.Length < 3)
                return UiResult.Fail(ErrorMessages.InsufficientParameters);

            if (!Enum.TryParse<FindField>(tokens[1], true, out var field))
                return UiResult.Fail(ErrorMessages.IncorrectParameters);

            var searchTerm = tokens[2];

            if (string.IsNullOrWhiteSpace(searchTerm))
                return UiResult.Fail(ErrorMessages.IncorrectParameters);

            var handler = new FindBooksHandler(_bookService);
            var command = new FindBooksCommand(field, searchTerm);
            var result = handler.Handle(command); // Result<IReadOnlyList<Book>>

            if (!result.IsSuccess)
                return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

            var output = FormatBookList(result.Value);
            return UiResult.Ok(output);
        }

        private UiResult HandleSort(string[] tokens)
        {
            if (tokens.Length < 2)
                return UiResult.Fail(ErrorMessages.InsufficientParameters);

            if (!Enum.TryParse<SortField>(tokens[1], true, out var sortField))
                return UiResult.Fail(ErrorMessages.IncorrectParameters);

            var handler = new SortBooksHandler(_bookService);
            var command = new SortBooksCommand(sortField);
            var result = handler.Handle(command); // Result<IReadOnlyList<Book>>

            if (!result.IsSuccess)
                return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

            var output = FormatBookList(result.Value);
            return UiResult.Ok(output);
        }

        private UiResult HandleRemove(string[] tokens)
        {
            if (tokens.Length < 2)
                return UiResult.Fail(ErrorMessages.InsufficientParameters);

            if (!Guid.TryParse(tokens[1], out Guid bookId))
                return UiResult.Fail(ErrorMessages.IncorrectParameters);

            var handler = new RemoveBookHandler(_bookService);
            var command = new RemoveBookCommand(bookId);
            var result = handler.Handle(command); // Result<bool>

            if (!result.IsSuccess)
                return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

            if (!result.Value)
                return UiResult.Fail("No book found with the specified Id.");

            var message = string.IsNullOrWhiteSpace(result.Message)
                ? "Book removed successfully."
                : result.Message;
                
            _commandHistory.Push(handler);
            return UiResult.Ok(message);
        }

        private UiResult HandleReport(string[] tokens)
        {
            if (tokens.Length < 2)
                return UiResult.Fail(ErrorMessages.InsufficientParameters);

            if (!Enum.TryParse<ReportType>(tokens[1], true, out var reportType))
                return UiResult.Fail(ErrorMessages.IncorrectParameters);

            if (reportType == ReportType.Catalog)
            {
                var handler = new ReportCatalogHandler(_bookService);
                var result = handler.Handle(new ReportCatalogCommand()); // Result<string>

                if (!result.IsSuccess)
                    return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

                return UiResult.Ok(result.Value);
            }

            if (reportType == ReportType.Summary)
            {
                var handler = new ReportSummaryHandler(_bookService);
                var result = handler.Handle(new ReportSummaryCommand()); // Result<string>

                if (!result.IsSuccess)
                    return UiResult.Fail(result.Error ?? ErrorMessages.UnexpectedError);

                return UiResult.Ok(result.Value);
            }

            return UiResult.Fail(ErrorMessages.IncorrectParameters);
        }

        private static string[] Tokenize(string input)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

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

        private static string FormatBookList(IReadOnlyList<Book> books)
        {
            if (books == null || books.Count == 0)
                return "No books found.";

            var builder = new StringBuilder();

            foreach (var book in books)
            {
                builder.AppendLine(new FeaturedBookDecorator(new BookDisplay(book)).Display());
            }

            return builder.ToString();
        }
    }
}
