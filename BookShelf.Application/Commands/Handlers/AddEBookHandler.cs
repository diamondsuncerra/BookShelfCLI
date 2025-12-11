using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddEBookHandler(IBookService bookService) : ICommandHandler<AddEBookCommand, CommandResult<Guid>>
    {
        private readonly IBookService _bookService = bookService;

        public CommandResult<Guid> Handle(AddEBookCommand command)
        {
            Guid id = _bookService.AddEBook(
                        command.Title,
                        command.Author,
                        command.Year,
                        command.FileFormat,
                        command.FileSizeMb);
            return CommandResult<Guid>.Ok(id, "Ebook added.");
        }

    }
}