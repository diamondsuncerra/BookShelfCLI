namespace BookShelf.Application.Commands.Models
{
    public sealed record AddEBookCommand
    (string Title,
     string Author,
     int Year,
     string FileFormat,
     decimal FileSizeMb);
}