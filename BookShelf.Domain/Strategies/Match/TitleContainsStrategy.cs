using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelf.Domain.Books;

namespace BookShelf.Domain.Strategies.Match
{
    public class TitleContainsStrategy : IMatchStrategy
    {
        public bool IsMatch(Book book, string term)
        {
            ArgumentException.ThrowIfNullOrEmpty(term);
            ArgumentNullException.ThrowIfNull(book);
            ArgumentException.ThrowIfNullOrEmpty(book.Title);
            return book.Title.Contains(term);
        }
    }
}