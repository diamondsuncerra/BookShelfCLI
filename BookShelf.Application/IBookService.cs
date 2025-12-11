using BookShelf.Domain.Books;
namespace BookShelf.Application
{
    public interface IBookService
    {
        Guid AddPhysical(string title, string author, int year, string isbn13, int pages);
        Guid AddEBook();
        IReadOnlyList<Book> List();
        IReadOnlyList<Book> Find();
        IReadOnlyList<Book> Sort();
        bool Remove(Guid id);
        string BuildSummaryReport();
        string BuildCatalogReport();
    }
}