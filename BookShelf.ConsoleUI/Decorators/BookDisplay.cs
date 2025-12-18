using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI.Decorators
{
    public class BookDisplay(Book book) : IBookDisplay
    {
        private readonly Book _book = book;
        public string Display() =>  $"\"{_book.Title}\" by {_book.Author} ({_book.Year})";
    }
}