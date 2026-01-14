using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddPhysicalBookHandler(IBookService bookService) : IUndoableCommandHandler<AddPhysicalBookCommand, Result<Guid>>
    {
        private readonly IBookService _bookService = bookService;
        private Guid? _addedId;
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
            _addedId = id;
            return Result<Guid>.Ok(id, "Physical book added.");
        }
        public void Undo()
        {
            if (_addedId is null) return;
            _bookService.Remove(_addedId.Value);
            _addedId = null;
        }
    }
}