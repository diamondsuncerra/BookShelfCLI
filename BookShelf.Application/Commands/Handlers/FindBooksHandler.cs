using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class FindBooksHandler(IBookService bookService) : ICommandHandler<FindBooksCommand, Result<IReadOnlyList<Book>>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<IReadOnlyList<Book>> Handle(FindBooksCommand command)
        {
            return Result<IReadOnlyList<Book>>.Ok(_bookService.Find(command.Field, command.Term), "Found books.");
        }

    }
}