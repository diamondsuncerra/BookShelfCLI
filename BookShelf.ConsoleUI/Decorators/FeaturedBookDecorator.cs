using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI.Decorators
{
    public class FeaturedBookDecorator(IBookDisplay bookDisplay) : IBookDisplay
    {
        private readonly IBookDisplay _bookDisplay = bookDisplay;
        public string Display() => $"<FEATURED>" + _bookDisplay.Display();
    }
}