using System.Linq;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Strategies.Sort
{
    public class SortByAuthorStrategy : ISortStrategy
    {
        public IReadOnlyList<Book> Sort(IReadOnlyList<Book> books)
        {
            return books.OrderBy(b => b.Author,StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
        }
    }
}