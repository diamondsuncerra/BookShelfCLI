namespace BookShelf.Application.Events
{
    public sealed record BookRemovedEvent (Guid Id, DateTime Timestamp) : IBookEvent;
}