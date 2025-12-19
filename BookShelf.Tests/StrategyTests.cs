using BookShelf.Domain.Books;
using BookShelf.Domain.Strategies.Match;
using BookShelf.Domain.Strategies.Sort;
using BookShelf.Infrastructure.Factory;

namespace BookShelf.Tests;

public class StrategyTests
{
    [Fact]
    public void SortStrategies_OrderAsExpected_AndAreStable()
    {
        var factory = new BookFactory();
        var books = new List<Book>
        {
            factory.CreatePhysical("Zulu", "Alpha", 2001, "9781234567890", 10),
            factory.CreatePhysical("Alpha", "Alpha", 1999, "9781234567891", 10),
            factory.CreatePhysical("Bravo", "Beta", 2010, "9781234567892", 10)
        };

        var byTitle = new SortByTitleStrategy().Sort(books);
        Assert.Equal(["Alpha", "Bravo", "Zulu"], byTitle.Select(b => b.Title).ToArray());

        var byYear = new SortByYearStrategy().Sort(books);
        Assert.Equal([1999, 2001, 2010], byYear.Select(b => b.Year).ToArray());

        var byAuthor = new SortByAuthorStrategy().Sort(books);
        var alphaBooks = byAuthor.Where(b => b.Author == "Alpha").ToList();
        Assert.Equal(2, alphaBooks.Count);
        Assert.Equal("Zulu", alphaBooks[0].Title);
        Assert.Equal("Alpha", alphaBooks[1].Title);
    }

    [Fact]
    public void MatchStrategies_AreCaseInsensitive_AndAllowPartialMatches()
    {
        var factory = new BookFactory();
        var book = factory.CreateEBook("Clean Code", "Robert C. Martin", 2008, "pdf", 1.2m);

        var titleStrategy = new TitleContainsStrategy();
        Assert.True(titleStrategy.IsMatch(book, "code"));
        Assert.True(titleStrategy.IsMatch(book, "CLE"));

        var authorStrategy = new AuthorContainsStrategy();
        Assert.True(authorStrategy.IsMatch(book, "martin"));
        Assert.True(authorStrategy.IsMatch(book, "ROB"));
    }
}
