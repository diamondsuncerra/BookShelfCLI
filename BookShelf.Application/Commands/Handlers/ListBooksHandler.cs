using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class ListBooksHandler(IBookService bookService) : ICommandHandler<ListBooksCommand, Result<IReadOnlyList<Book>>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<IReadOnlyList<Book>> Handle(ListBooksCommand command)
        {
            return Result<IReadOnlyList<Book>>.Ok(_bookService.List(), "Listed books.");
        }
    }
}