using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class AddEBookHandler(IBookService bookService) : IUndoableCommandHandler<AddEBookCommand, Result<Guid>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<Guid> Handle(AddEBookCommand command)
        {
            Guid id = _bookService.AddEBook(
                        command.Title,
                        command.Author,
                        command.Year,
                        command.FileFormat,
                        command.FileSizeMb);
            return Result<Guid>.Ok(id, "Ebook added.");
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}