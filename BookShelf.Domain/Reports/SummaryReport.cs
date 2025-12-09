using System.Text;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Reports
{
    public sealed class SummaryReport : ReportTemplate
    {
        protected override string BuildHeader(IEnumerable<Book> books)
        {
            return "=== Summary Report ===";
        }

        protected override string BuildBody(IEnumerable<Book> books)
        {
            ArgumentNullException.ThrowIfNull(books);

            var list = books.ToList();
            if (list.Count == 0)
            {
                return "No books found.";
            }

            var total = list.Count;
            var physicalCount = list.OfType<PhysicalBook>().Count();
            var ebookCount = list.OfType<EBook>().Count();

            var oldestYear = list.Min(b => b.Year);
            var newestYear = list.Max(b => b.Year);

            var sb = new StringBuilder();
            sb.AppendLine($"Total books: {total}");
            sb.AppendLine($"Physical books: {physicalCount}");
            sb.AppendLine($"EBooks: {ebookCount}");
            sb.AppendLine($"Oldest year: {oldestYear}");
            sb.AppendLine($"Newest year: {newestYear}");

            return sb.ToString();
        }

        protected override string BuildFooter(IEnumerable<Book> books)
        {
            return $"Generated at: {DateTimeOffset.UtcNow:u}";
        }
    }
}
