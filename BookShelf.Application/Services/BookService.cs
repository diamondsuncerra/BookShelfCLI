using System.Text;
using BookShelf.Application.Commands.Enums;
using BookShelf.Domain.Books;
using BookShelf.Domain.Factories;
using BookShelf.Domain.Reports;
using BookShelf.Domain.Repositories;
using BookShelf.Domain.Strategies;
using BookShelf.Domain.Strategies.Match;
using BookShelf.Domain.Strategies.Sort;

namespace BookShelf.Application.Services
{
    public class BookService(IBookFactory factory, IBookRepository repository) : IBookService
    {
        private readonly IBookFactory _bookFactory = factory;
        private readonly IBookRepository _bookRepository = repository;
        private readonly ReportTemplate _catalogReport = new CatalogReport();
        private readonly ReportTemplate _summaryReport = new SummaryReport();
        private ISortStrategy _sortStrategy;
        private IMatchStrategy _matchStrategy;
        public Guid AddEBook(string title, string author, int year, string fileFormat, decimal fileSizeMb)
        {
            Book newBook = _bookFactory.CreateEBook(title, author, year, fileFormat, fileSizeMb);
            _bookRepository.Add(newBook);
            return newBook.Id;
            // handler catches the exceptions TODO
        }

        public Guid AddPhysical(string title, string author, int year, string isbn13, int pages)
        {
            Book newBook = _bookFactory.CreatePhysical(title, author, year, isbn13, pages);
            // TODO create maybe an object for all of these params? DTOS?? 
            _bookRepository.Add(newBook);
            return newBook.Id;
        }

        public string BuildCatalogReport()
        {
            var books = _bookRepository.List();
            return _catalogReport.Build(books);
        }

        public string BuildSummaryReport()
        {
            var books = _bookRepository.List();
            return _summaryReport.Build(books);
        }

        public IReadOnlyList<Book> Find(FindField field, string term)
        {
            var books = _bookRepository.List();
            if (!Enum.IsDefined(typeof(FindField), field))
            {
                throw new ArgumentException("Unsupported field type for search.");
            }

            _matchStrategy = field.Equals(FindField.Author) ? new AuthorContainsStrategy() : new TitleContainsStrategy();


            IReadOnlyList<Book> result = [];
            foreach (Book book in books)
            {
                _matchStrategy.IsMatch(book, term);
                _ = result.Append(book);
            }
            return result;
        }

        public IReadOnlyList<Book> List()
        {
            return _bookRepository.List();
        }

        public bool Remove(Guid id)
        {
            return _bookRepository.Remove(id);
        }

        public IReadOnlyList<Book> Sort(SortField strategy)
        {
            var books = _bookRepository.List();
            _sortStrategy = strategy switch
            {
                SortField.Author => new SortByAuthorStrategy(),
                SortField.Title => new SortByTitleStrategy(),
                SortField.Year => new SortByYearStrategy(),
                _ => throw new ArgumentException("Unsupported sort strategy.")
            };

            IReadOnlyList<Book> result = _sortStrategy.Sort(books);
            return result;
        }
    }
}