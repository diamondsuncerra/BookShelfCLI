using BookShelf.Domain.Books;

namespace BookShelf.Domain.Reports
{
    public abstract class ReportTemplate
    {
        public string Build(IEnumerable<Book> books)
        {
            ArgumentNullException.ThrowIfNull(books);
            
            var header = TrimOrThrow(BuildHeader(books), "Header");
            var body = TrimOrThrow(BuildBody(books), "Body");
            var footer = TrimOrThrow(BuildFooter(books), "Footer");

            return header + Environment.NewLine + body + Environment.NewLine + footer;
        }
        private static string TrimOrThrow(string text, string name)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException($"{name} cannot be null or whitespace.", nameof(text));

            else return text.TrimEnd();
        }
        protected abstract string BuildHeader(IEnumerable<Book> books);
        protected abstract string BuildBody(IEnumerable<Book> books);
        protected abstract string BuildFooter(IEnumerable<Book> books);
    }
}