using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Repositories
{
    public interface IBookRepository
    {
        void Add(Book book);
        bool Remove(Guid id);
        Book? Get(Guid id);
        IReadOnlyList<Book> List(); //immutable
    }
}