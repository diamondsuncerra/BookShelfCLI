namespace BookShelf.Application.Commands.Handlers
{
    public class ReportSummaryHandler(IBookService bookService)
    {
        
        private readonly IBookService _bookService = bookService;
    }
}