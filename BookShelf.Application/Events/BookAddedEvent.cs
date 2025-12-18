namespace BookShelf.Application.Events
{
    public sealed record BookAddedEvent(Guid Id, DateTime Timestamp) : IBookEvent;
}