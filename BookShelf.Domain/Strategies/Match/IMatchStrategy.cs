using BookShelf.Domain.Books;

namespace BookShelf.Domain.Strategies.Match
{
    public interface IMatchStrategy
    {
        bool IsMatch(Book book, string term);
    }
}