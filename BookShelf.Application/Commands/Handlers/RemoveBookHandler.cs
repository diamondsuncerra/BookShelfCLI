namespace BookShelf.Application.Commands.Handlers
{
    public class RemoveBookHandler(IBookService bookService)
    {
        
        private readonly IBookService _bookService = bookService;
    }
}