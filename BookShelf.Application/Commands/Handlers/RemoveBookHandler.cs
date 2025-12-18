using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class RemoveBookHandler(IBookService bookService) :  IUndoableCommandHandler<RemoveBookCommand, Result<bool>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<bool> Handle(RemoveBookCommand command)
        {
            bool result = _bookService.Remove(command.Id);
            return Result<bool>.Ok(result, "Book removed successfully.");
            // add error handling later!
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}