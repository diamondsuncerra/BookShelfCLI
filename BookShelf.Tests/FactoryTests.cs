using BookShelf.Domain.Books;
using BookShelf.Infrastructure.Factory;

namespace BookShelf.Tests;

public class FactoryTests
{
    [Fact]
    public void CreatePhysical_NormalizesFields()
    {
        var factory = new BookFactory();

        var book = (PhysicalBook)factory.CreatePhysical(
            "  The Title  ",
            "  The Author ",
            2001,
            "978-1 234567890",
            123);

        Assert.Equal("The Title", book.Title);
        Assert.Equal("The Author", book.Author);
        Assert.Equal("9781234567890", book.Isbn13);
    }

    [Fact]
    public void CreateEBook_NormalizesFields()
    {
        var factory = new BookFactory();

        var book = (EBook)factory.CreateEBook(
            "  Clean Code ",
            "  Robert C. Martin ",
            2008,
            " PDF ",
            2.5m);

        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Robert C. Martin", book.Author);
        Assert.Equal(EBookFormat.Pdf, book.Format);
    }

    [Fact]
    public void CreatePhysical_ThrowsOnBadIsbn13()
    {
        var factory = new BookFactory();

        Assert.Throws<ArgumentException>(() =>
            factory.CreatePhysical("Title", "Author", 2000, "123", 10));
    }

    [Fact]
    public void CreateEBook_ThrowsOnNegativeFileSize()
    {
        var factory = new BookFactory();

        Assert.Throws<ArgumentException>(() =>
            factory.CreateEBook("Title", "Author", 2000, "pdf", -1m));
    }

    [Fact]
    public void CreateBook_ThrowsOnInvalidYear()
    {
        var factory = new BookFactory();

        Assert.Throws<ArgumentException>(() =>
            factory.CreatePhysical("Title", "Author", 1400, "9781234567890", 10));
    }

    [Fact]
    public void CreateBook_ThrowsOnEmptyTitleOrAuthor()
    {
        var factory = new BookFactory();

        Assert.ThrowsAny<Exception>(() =>
            factory.CreatePhysical(" ", "Author", 2000, "9781234567890", 10));

        Assert.ThrowsAny<Exception>(() =>
            factory.CreatePhysical("Title", " ", 2000, "9781234567890", 10));
    }
}
