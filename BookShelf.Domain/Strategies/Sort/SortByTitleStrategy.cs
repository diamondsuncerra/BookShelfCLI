using System.Linq;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Strategies.Sort
{
    public class SortByTitleStrategy : ISortStrategy
    {
        public IReadOnlyList<Book> Sort(IReadOnlyList<Book> books)
        {
            return books.OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
        }
    }
}