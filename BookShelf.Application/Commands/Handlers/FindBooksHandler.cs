namespace BookShelf.Application.Commands.Handlers
{
    public class FindBooksHandler(IBookService bookService)
    {
        private readonly IBookService _bookService = bookService;
    }
}