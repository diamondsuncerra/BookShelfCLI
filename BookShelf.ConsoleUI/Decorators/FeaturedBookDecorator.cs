using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI.Decorators
{
    public class FeaturedBookDecorator(Book book) : IBookDisplay
    {
        private readonly Book _book = book;
        public string Display() => $"<FEATURED> \"{_book.Title}\" by {_book.Author} ({_book.Year})";
    }
}