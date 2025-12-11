namespace BookShelf.Application.Commands.Models
{
    public sealed record AddPhysicalBookCommand
    (string Title,
     string Author,
     int Year,
     string Isbn13,
     int Pages);
}