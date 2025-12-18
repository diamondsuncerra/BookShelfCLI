using BookShelf.Domain.Books;

namespace BookShelf.ConsoleUI.Decorators
{
    public class RecommendBookDecorator(IBookDisplay bookDisplay) : IBookDisplay
    {
        private readonly IBookDisplay _bookDisplay = bookDisplay;
        public string Display() => $"<Recommended> " + _bookDisplay.Display();
    }

}