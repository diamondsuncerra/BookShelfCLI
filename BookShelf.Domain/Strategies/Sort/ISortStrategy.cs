using BookShelf.Domain.Books;

namespace BookShelf.Domain.Strategies
{
    public interface ISortStrategy
    {
        IReadOnlyList<Book> Sort(IReadOnlyList<Book> books);
    }
}