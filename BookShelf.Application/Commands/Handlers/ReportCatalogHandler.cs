namespace BookShelf.Application.Commands.Handlers
{
    public class ReportCatalogHandler(IBookService bookService)
    {
        
        private readonly IBookService _bookService = bookService;
    }
}