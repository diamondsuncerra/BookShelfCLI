using BookShelf.Application.Commands.Abstract;
using BookShelf.Application.Commands.Models;

namespace BookShelf.Application.Commands.Handlers
{
    public class UndoCommandHandler(IBookService bookService) : ICommandHandler<UndoCommand, Result<bool>>
    {
        private readonly IBookService _bookService = bookService;
        public Result<bool> Handle(UndoCommand command)
        {
            throw new NotImplementedException();
        }
    }
}