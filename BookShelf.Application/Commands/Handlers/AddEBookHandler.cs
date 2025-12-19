using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddEBookHandler(IBookService bookService) : IUndoableCommandHandler<AddEBookCommand, Result<Guid>>
    {
        private readonly IBookService _bookService = bookService;
        private Guid? _addedId;
        public Result<Guid> Handle(AddEBookCommand command)
        {
            Guid id = _bookService.AddEBook(
                        command.Title,
                        command.Author,
                        command.Year,
                        command.FileFormat,
                        command.FileSizeMb);
            _addedId = id;
            return Result<Guid>.Ok(id, "Ebook added.");
        }


        public void Undo()
        {
            if (_addedId is null) return;
            _bookService.Remove(_addedId.Value);
            _addedId = null;
        }

    }
}