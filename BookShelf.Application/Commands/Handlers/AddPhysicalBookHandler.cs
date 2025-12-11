using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddPhysicalBookHandler : ICommandHandler<AddPhysicalBookCommand, CommandResult<Guid>>
    {
        private readonly IBookService _bookService;
        public AddPhysicalBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }
        public CommandResult<Guid> Handle(AddPhysicalBookCommand command)
        {
            Guid id = _bookService.AddPhysical(
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