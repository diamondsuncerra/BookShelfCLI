using BookShelf.Domain.Books;

namespace BookShelf.Domain.Factories
{
    public interface IBookFactory
    {
        Book CreatePhysical(string title, string author, int year, string isbn13, int pages);
        Book CreateEBook(string title, string author, int year, string fileFormat, decimal fileSizeMb);
    }
}