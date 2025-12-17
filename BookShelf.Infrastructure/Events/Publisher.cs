using BookShelf.Application.Events;

namespace BookShelf.Infrastructure.Events
{
    public class Publisher : IBookEventPublisher
    {
        private readonly IList<IBookEventObserver> observers = []; 
        public void Attach(IBookEventObserver observer)
        {
           observers.Add(observer);
        }

        public void Dettach(IBookEventObserver observer)
        {
            observers.Remove(observer);
        }

        public void Publish(IBookEvent bookEvent)
        {
            foreach(IBookEventObserver observer in observers) {
                observer.Update(bookEvent);
            }
        }
    }
}