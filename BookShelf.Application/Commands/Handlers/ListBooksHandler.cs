namespace BookShelf.Application.Commands.Handlers
{
    public class ListBooksHandler(IBookService bookService)
    {
        private readonly IBookService _bookService = bookService;
    }
}