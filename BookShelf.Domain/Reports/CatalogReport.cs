using System.Text;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Reports
{
    public class CatalogReport : ReportTemplate
    {
        protected override string BuildHeader(IEnumerable<Book> books)
        {
            return "=== Catalog Report ===";
        }

        protected override string BuildBody(IEnumerable<Book> books)
        {
            // books should come ordered i believe
            ArgumentNullException.ThrowIfNull(books);

            if (!books.Any())
                return "No books found.";

            var orderedBooks = books.OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase);
            StringBuilder result = new();
            result.AppendLine($"Total number of books: {books.Count()}");

            foreach (var book in orderedBooks)
            {
                var type = book switch
                {
                    PhysicalBook => "Physical",
                    EBook => "EBook",
                    _ => "Unknown"
                };

                result.AppendLine($"{book.Title} | {book.Author} | {book.Year} | {type}");
            }
            return result.ToString();
        }
        protected override string BuildFooter(IEnumerable<Book> books)
        {
            return $"Generated at: {DateTimeOffset.UtcNow:u}";
        }

    }
}