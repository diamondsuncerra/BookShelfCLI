using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI.Decorators
{
    public class RecommendedBookDecorator(Book book) : IBookDisplay
    {
        private readonly Book _book = book;
        public string Display() =>  $"<RECOMMENDED> \"{_book.Title}\" by {_book.Author} ({_book.Year})";
    }
}