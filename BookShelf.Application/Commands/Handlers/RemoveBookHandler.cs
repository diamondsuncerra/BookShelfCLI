using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;
using BookShelf.Domain.Books;

namespace BookShelf.Application.Commands.Handlers
{
    public class RemoveBookHandler(IBookService bookService) : IUndoableCommandHandler<RemoveBookCommand, Result<bool>>
    {
        private readonly IBookService _bookService = bookService;
        private Book? _removedBook;

        public Result<bool> Handle(RemoveBookCommand command)
        {
            _removedBook = _bookService.Get(command.Id);
            var result = _bookService.Remove(command.Id);
            if (!result) _removedBook = null;
            return Result<bool>.Ok(result, "Book removed successfully.");
        }

        public void Undo()
        {
            if (_removedBook is null) return;
            _bookService.Restore(_removedBook);
            _removedBook = null;
        }
    }
}