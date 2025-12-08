using System;
using BookShelf.Domain.Books;
using BookShelf.Domain.Factories;

namespace BookShelf.Infrastructure.Factory
{
    public class BookFactory : IBookFactory
    {
        public Book CreateEBook(string title, string author, int year, string fileFormat, decimal fileSizeMb)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(author);
            ArgumentNullException.ThrowIfNull(fileFormat);

            var normalizedTitle = title.Trim();
            var normalizedAuthor = author.Trim();
            var normalizedFormat = fileFormat.Trim().ToLowerInvariant();

            return new EBook(normalizedTitle, normalizedAuthor, year, normalizedFormat, fileSizeMb);
        }

        public Book CreatePhysical(string title, string author, int year, string isbn13, int pages)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(author);
            ArgumentNullException.ThrowIfNull(isbn13);

            var normalizedTitle = title.Trim();
            var normalizedAuthor = author.Trim();
            var normalizedIsbn = isbn13.Trim()
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty);

            return new PhysicalBook(normalizedTitle, normalizedAuthor, year, normalizedIsbn, pages);
        }
    }
}
