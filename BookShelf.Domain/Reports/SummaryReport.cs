using BookShelf.Domain.Books;

namespace BookShelf.Domain.Reports
{
    public class SummaryReport : ReportTemplate
    {
        protected override string BuildBody(IEnumerable<Book> books)
        {
            throw new NotImplementedException();
        }

        protected override string BuildFooter(IEnumerable<Book> books)
        {
            throw new NotImplementedException();
        }

        protected override string BuildHeader(IEnumerable<Book> books)
        {
            throw new NotImplementedException();
        }
    }
}