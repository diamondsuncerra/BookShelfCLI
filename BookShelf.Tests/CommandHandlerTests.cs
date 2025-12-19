using BookShelf.Application.Commands.Enums;
using BookShelf.Application.Commands.Handlers;
using BookShelf.Application.Commands.Models;
using BookShelf.Application.Services;
using BookShelf.Infrastructure.Events;
using BookShelf.Infrastructure.Factory;
using BookShelf.Infrastructure.Repository;

namespace BookShelf.Tests;

public class CommandHandlerTests
{
    private static (BookService Service, InMemoryBookRepository Repo) CreateService()
    {
        var repo = new InMemoryBookRepository();
        var factory = new BookFactory();
        var publisher = new Publisher();
        var service = new BookService(factory, repo, publisher);
        return (service, repo);
    }

    [Fact]
    public void AddPhysicalHandler_AddsAndReturnsCreatedId()
    {
        var (service, repo) = CreateService();
        var handler = new AddPhysicalBookHandler(service);
        var command = new AddPhysicalBookCommand("Title", "Author", 2000, "9781234567890", 10);

        var result = handler.Handle(command);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        Assert.NotNull(repo.Get(result.Value));
    }

    [Fact]
    public void FindHandler_TitleContains_ReturnsMatchingBooks()
    {
        var (service, _) = CreateService();
        service.AddPhysical("Clean Code", "Author A", 2008, "9781234567890", 10);
        service.AddPhysical("The Pragmatic Programmer", "Author B", 1999, "9781234567891", 10);
        service.AddPhysical("Code Complete", "Author C", 2004, "9781234567892", 10);

        var handler = new FindBooksHandler(service);
        var command = new FindBooksCommand(FindField.Title, "code");
        var result = handler.Handle(command);

        Assert.True(result.IsSuccess);
        var titles = result.Value!.Select(b => b.Title).ToList();
        Assert.Contains("Clean Code", titles);
        Assert.Contains("Code Complete", titles);
        Assert.DoesNotContain("The Pragmatic Programmer", titles);
    }

    [Fact]
    public void SortHandler_DoesNotMutateRepositoryOrder()
    {
        var (service, _) = CreateService();
        service.AddPhysical("Zulu", "Author A", 2005, "9781234567890", 10);
        service.AddPhysical("Alpha", "Author B", 1999, "9781234567891", 10);
        service.AddPhysical("Bravo", "Author C", 2010, "9781234567892", 10);

        var initialOrder = service.List().Select(b => b.Id).ToArray();

        var handler = new SortBooksHandler(service);
        var byTitle = handler.Handle(new SortBooksCommand(SortField.Title));
        var byYear = handler.Handle(new SortBooksCommand(SortField.Year));

        Assert.True(byTitle.IsSuccess);
        Assert.True(byYear.IsSuccess);
        Assert.Equal(["Alpha", "Bravo", "Zulu"], byTitle.Value!.Select(b => b.Title).ToArray());
        Assert.Equal([1999, 2005, 2010], byYear.Value!.Select(b => b.Year).ToArray());
        Assert.Equal(initialOrder, service.List().Select(b => b.Id).ToArray());
    }

    [Fact]
    public void RemoveHandler_ReturnsFalseForUnknownId_TrueForExisting()
    {
        var (service, _) = CreateService();
        var handler = new RemoveBookHandler(service);

        var missing = handler.Handle(new RemoveBookCommand(Guid.NewGuid()));
        Assert.True(missing.IsSuccess);
        Assert.False(missing.Value);

        var id = service.AddPhysical("Title", "Author", 2000, "9781234567890", 10);
        var removed = handler.Handle(new RemoveBookCommand(id));
        Assert.True(removed.IsSuccess);
        Assert.True(removed.Value);
    }
}
