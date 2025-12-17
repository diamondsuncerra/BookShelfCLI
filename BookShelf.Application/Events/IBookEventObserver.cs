namespace BookShelf.Application.Events
{
    public interface IBookEventObserver
    {
        void Update(IBookEvent bookEvent);
    }
}