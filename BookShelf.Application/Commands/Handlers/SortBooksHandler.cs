using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class SortBooksHandler(IBookService bookService) : ICommandHandler<SortBooksCommand, Result<IReadOnlyList<Book>>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<IReadOnlyList<Book>> Handle(SortBooksCommand command)
        {
            IReadOnlyList<Book> result = _bookService.Sort(command.Strategy);
            return Result<IReadOnlyList<Book>>.Ok(result, "Books sorted");
        }
    }
}