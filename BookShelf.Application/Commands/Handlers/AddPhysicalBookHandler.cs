using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddPhysicalBookHandler(IBookService bookService) : ICommandHandler<AddPhysicalBookCommand, Result<Guid>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<Guid> Handle(AddPhysicalBookCommand command)
        {
            Guid id = _bookService.AddPhysical( 
                // to avoid coupling bookservice to the commands
                command.Title,
                command.Author,
                command.Year,
                command.Isbn13,
                command.Pages
            );
            return Result<Guid>.Ok(id, "Physical book added.");
        }
    }
}