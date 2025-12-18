using BookShelf.Application.Events;

namespace BookShelf.ConsoleUI.Observer
{
    public class ConsoleLoggerObserver : IBookEventObserver
    {
        public void Update(IBookEvent bookEvent)
        {
            Console.WriteLine(bookEvent);
        }
    }
}