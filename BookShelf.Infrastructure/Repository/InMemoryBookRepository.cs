using BookShelf.Domain.Books;
using BookShelf.Domain.Repositories;

namespace BookShelf.Infrastructure.Repository
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly Dictionary<Guid,Book> _data;
        public InMemoryBookRepository()
        {
            _data = new();
        }
        public void Add(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);
            _data.TryAdd(book.Id, book);
        }

        public Book? Get(Guid id)
        {
            _data.TryGetValue(id, out Book? book);
            return book;
        }

        public IReadOnlyList<Book> List()
        {
            return [.. _data.Values];
        }

        public bool Remove(Guid id)
        {
            return _data.Remove(id);
        }
    }
}