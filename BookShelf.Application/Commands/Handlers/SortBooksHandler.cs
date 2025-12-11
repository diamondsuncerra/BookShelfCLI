namespace BookShelf.Application.Commands.Handlers
{
    public class SortBooksHandler(IBookService bookService)
    {
        private readonly IBookService _bookService = bookService;   
    }
}