using BookShelf.Application.Commands.Enums;

namespace BookShelf.Application.Commands.Models
{
    public sealed record FindBooksCommand
    (FindField Field,
     string Term);    
}