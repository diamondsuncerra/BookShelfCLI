using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class RemoveBookHandler(IBookService bookService) :  ICommandHandler<RemoveBookCommand, Result<bool>>
    {
        private readonly IBookService _bookService = bookService;

        public Result<bool> Handle(RemoveBookCommand command)
        {
            bool result = _bookService.Remove(command.Id);
            return Result<bool>.Ok(result, "Book removed succesfully.");
            // add error handling later!
        }
    }
}