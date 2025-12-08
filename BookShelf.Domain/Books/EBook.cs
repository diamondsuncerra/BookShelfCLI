using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelf.Domain.Books
{
    public class EBook : Book
    {
        EBookFormat Format { get; }
        public decimal FileSizeMb { get; }

        public EBook(string title, string author, int year, string format, decimal size)
            : base(title, author, year)
        {
            Format = ParseFormat(format);
            FileSizeMb = ValidateSize(size);
        }

        private decimal ValidateSize(decimal size)
        {
            if (size < 0) throw new ArgumentException("Pages cannot be negative.");
            return size;
        }

        private static EBookFormat ParseFormat(string input)
        {
            if (Enum.TryParse<EBookFormat>(input, true, out var result))
                return result;

            throw new ArgumentException("Format must be pdf, epub, or mobi.");
        }
    }
}