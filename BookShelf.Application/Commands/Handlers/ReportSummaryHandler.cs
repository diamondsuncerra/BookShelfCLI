using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class ReportSummaryHandler(IBookService bookService) : ICommandHandler<ReportSummaryCommand, Result<string>>
    {

        private readonly IBookService _bookService = bookService;

        public Result<string> Handle(ReportSummaryCommand command)
        {
            string result = _bookService.BuildSummaryReport();
            return Result<string>.Ok(result);
        }
    }
}