using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class FindBooksHandler(IBookService bookService) : ICommandHandler<FindBooksCommand, CommandResult<IReadOnlyList<Book>>>
    {
        private readonly IBookService _bookService = bookService;

        public CommandResult<IReadOnlyList<Book>> Handle(FindBooksCommand command)
        {

            return CommandResult<IReadOnlyList<Book>>.Ok(_bookService.Find(command.Field, command.Term), "Found books.");
        }

    }
}