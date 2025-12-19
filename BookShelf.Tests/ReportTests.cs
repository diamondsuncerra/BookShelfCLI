using BookShelf.Domain.Reports;
using BookShelf.Infrastructure.Factory;

namespace BookShelf.Tests;

public class ReportTests
{
    [Fact]
    public void SummaryReport_Build_IncludesTotalsAndMinMaxYears()
    {
        var factory = new BookFactory();
        var books = new[]
        {
            factory.CreatePhysical("Alpha", "Author A", 1990, "9781234567890", 10),
            factory.CreateEBook("Bravo", "Author B", 2010, "pdf", 1.0m),
            factory.CreatePhysical("Charlie", "Author C", 2000, "9781234567891", 10)
        };

        var report = new SummaryReport();
        var output = report.Build(books);

        Assert.Contains("Total books: 3", output);
        Assert.Contains("Physical books: 2", output);
        Assert.Contains("EBooks: 1", output);
        Assert.Contains("Oldest year: 1990", output);
        Assert.Contains("Newest year: 2010", output);
    }

    [Fact]
    public void CatalogReport_Build_ListsEntriesSortedByTitle()
    {
        var factory = new BookFactory();
        var books = new[]
        {
            factory.CreatePhysical("Zulu", "Author A", 2000, "9781234567890", 10),
            factory.CreateEBook("Alpha", "Author B", 2010, "pdf", 1.0m),
            factory.CreatePhysical("Bravo", "Author C", 2005, "9781234567891", 10)
        };

        var report = new CatalogReport();
        var output = report.Build(books);
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var alphaIndex = Array.FindIndex(lines, line => line.StartsWith("Alpha |", StringComparison.Ordinal));
        var bravoIndex = Array.FindIndex(lines, line => line.StartsWith("Bravo |", StringComparison.Ordinal));
        var zuluIndex = Array.FindIndex(lines, line => line.StartsWith("Zulu |", StringComparison.Ordinal));

        Assert.True(alphaIndex >= 0);
        Assert.True(bravoIndex > alphaIndex);
        Assert.True(zuluIndex > bravoIndex);
    }
}
