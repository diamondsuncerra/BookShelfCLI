using BookShelf.Domain.Books;
namespace BookShelf.Application
{
    public interface IBookService
    {
        Guid AddPhysical(string title, string author, int year, string isbn13, int pages);
        IReadOnlyList<Book> List();
        IReadOnlyList<Book> Find();
        IReadOnlyList<Book> Sort();
        bool Remove(Guid id);
        string BuildSummaryReport();
        string BuildCatalogReport();
        Guid AddEBook(string title, string author, int year, string fileFormat, decimal fileSizeMb);
    }
}