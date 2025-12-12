using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class ReportCatalogHandler(IBookService bookService) : ICommandHandler<ReportCatalogCommand, Result<string>>
    {

        private readonly IBookService _bookService = bookService;

        public Result<string> Handle(ReportCatalogCommand command)
        {
            string result = _bookService.BuildCatalogReport();
            return Result<string>.Ok(result);
        }
    }
}