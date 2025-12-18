using BookShelf.Application.Commands.Abstract;

namespace BookShelf.ConsoleUI
{
    public class CommandHistory : ICommandHistory
    {
        private readonly Stack<IUndoableCommandHandler<object, object>> _stack = new();

        public void Push(IUndoableCommandHandler<object, object> handler)
            => _stack.Push(handler);

        public IUndoableCommandHandler<object, object>? Pop()
            => _stack.Count > 0 ? _stack.Pop() : null;

    }
}