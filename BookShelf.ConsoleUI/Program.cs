using System;
using BookShelf.ConsoleUI;
using BookShelf.ConsoleUI.UIMessages;
using BookShelf.Application.Commands;
using BookShelf.Domain.Factories;
using BookShelf.Domain.Repositories;
using BookShelf.Application;
using BookShelf.Infrastructure.Factory;
using BookShelf.Infrastructure.Repository;
using BookShelf.Application.Services;

namespace BookShelf.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IBookFactory bookFactory = new BookFactory();
            IBookRepository bookRepository = new InMemoryBookRepository();
            IBookService bookService = new BookService(bookFactory, bookRepository);

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

                Result result = router.Route(input);

                if (!result.IsSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    if (//string.Equals(result.Message, CommandsOrFields.Exit, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(result.Message, "exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("GoodBye!");
                        break;
                    }
                    Console.WriteLine(result.Message);
                }

            }
        }
    }
}
