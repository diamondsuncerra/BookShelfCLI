using BookShelf.Domain.Factories;
using BookShelf.Domain.Repositories;
using BookShelf.Application;
using BookShelf.Infrastructure.Factory;
using BookShelf.Infrastructure.Repository;
using BookShelf.Application.Services;
using BookShelf.Application.Events;
using BookShelf.Infrastructure.Events;
using BookShelf.ConsoleUI.Observer;

namespace BookShelf.ConsoleUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            IBookFactory bookFactory = new BookFactory();
            IBookRepository bookRepository = new InMemoryBookRepository();
            IBookEventPublisher bookEventPublisher = new Publisher();
            IBookEventObserver consoleLoggerObserver = new ConsoleLoggerObserver();
            bookEventPublisher.Attach(consoleLoggerObserver);
            IBookService bookService = new BookService(bookFactory, bookRepository, bookEventPublisher);

            var router = new CommandRouter(bookService);
            Console.WriteLine("BookShelf CLI");
            Console.WriteLine("Type 'help' for usage. Type 'exit' to quit.");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (input is null)
                    break;

                var result = router.Route(input);

                if (!result.IsSuccess)
                    Console.WriteLine(result.Error);
                else
                    Console.WriteLine(result.Output);

                if (result.ShouldExit)
                    break;
            }

        }
    }
}

