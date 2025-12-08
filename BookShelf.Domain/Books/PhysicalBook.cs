using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelf.Domain.Books
{
    public class PhysicalBook : Book
    {
        int Pages { get; set; }
        string Isbn13 { get; set; }
        public PhysicalBook(int pages, string isbn13, string title, string author, int year) : base(title, author, year)
        {
            Pages = ValidatePages(pages);
            Isbn13 = ValidateIsbn13(isbn13);
        }

        // constructor validation required not fluid.
        private static string ValidateIsbn13(string isbn13)
        {
            if (string.IsNullOrWhiteSpace(isbn13)) throw new ArgumentException("ISBN13 cannot be empty.");
            var trimmedIsbn = isbn13.Trim();

            if (trimmedIsbn.Length != 13 || !trimmedIsbn.All(char.IsDigit))
                throw new ArgumentException("ISBN13 must be exactly 13 digits.");

            return trimmedIsbn;
        }

        private static int ValidatePages(int pages)
        {
            if (pages < 0) throw new ArgumentException("Pages cannot be negative.");
            return pages;
        }
    }
}