namespace BookShelf.Domain.Books
{
    public abstract class Book
    {
        public Guid Id { get; protected set; }
        public string Title { get; protected set; }
        public string Author { get; protected set; }
        public int Year { get; protected set; }
        public DateTimeOffset AddedAt { get; protected set; }

        public Book(string title, string author, int year)
        {
            Title = ValidateText(title);
            Author = ValidateText(author);
            Year = ValidateYear(year);
            Id = Guid.NewGuid();
            AddedAt = DateTimeOffset.UtcNow;
        }

        // E ok validarea aici?
        private static int ValidateYear(int year)
        {
            int currentLimit = DateTimeOffset.UtcNow.Year + 1;
            if (year < 1450 || year > currentLimit)
                throw new ArgumentException($"Year must be in range 1450..{currentLimit}.");
            return year;
        }

        private static string ValidateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("Field cannot be empty.");
            }
            else return text.Trim();
        }
        public string ToString()
        {
            return Title + " " + Author + Environment.NewLine;
        }
    }
}