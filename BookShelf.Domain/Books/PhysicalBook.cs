namespace BookShelf.Domain.Books
{
    public class PhysicalBook : Book
    {
        public string Isbn13 { get; }
        public int Pages { get; }
        public PhysicalBook(string title, string author, int year, string isbn13, int pages) : base(title, author, year)
        {
            Isbn13 = ValidateIsbn13(isbn13);
            Pages = ValidatePages(pages);
        }

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