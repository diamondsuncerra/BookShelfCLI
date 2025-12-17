namespace BookShelf.Application.Events
{
    public interface IBookEventPublisher
    {
        void Attach(IBookEventObserver observer);
        void Dettach(IBookEventObserver observer);
        void Publish(IBookEvent bookEvent);
    }
}