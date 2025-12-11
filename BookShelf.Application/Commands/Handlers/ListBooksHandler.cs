using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class ListBooksHandler(IBookService bookService) : ICommandHandler<ListBooksCommand, CommandResult<IReadOnlyList<Book>>>
    {
        private readonly IBookService _bookService = bookService;

        public CommandResult<IReadOnlyList<Book>> Handle(ListBooksCommand command)
        {
            return CommandResult<IReadOnlyList<Book>>.Ok(_bookService.List(), "Listed books.");
        }
    }
}