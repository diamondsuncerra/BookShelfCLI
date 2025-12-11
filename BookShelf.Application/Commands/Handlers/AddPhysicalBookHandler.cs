using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddPhysicalBookHandler(IBookService bookService) : ICommandHandler<AddPhysicalBookCommand, CommandResult<Guid>>
    {
        private readonly IBookService _bookService = bookService;

        public CommandResult<Guid> Handle(AddPhysicalBookCommand command)
        {
            Guid id = _bookService.AddPhysical( 
                // to avoid coupling bookservice to the commands
                command.Title,
                command.Author,
                command.Year,
                command.Isbn13,
                command.Pages
            );
            return CommandResult<Guid>.Ok(id, "Physical book added.");
        }
    }
}