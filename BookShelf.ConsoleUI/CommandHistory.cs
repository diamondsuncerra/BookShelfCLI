using BookShelf.Application.Commands.Abstract;

namespace BookShelf.ConsoleUI
{
    public class CommandHistory : ICommandHistory
    {
        private readonly Stack<IUndoable> _history = new();

        public void Push(IUndoable command)
        {
            if (command is not null) _history.Push(command);
        }

        public IUndoable? Pop()
        {
            return _history.Count == 0 ? null : _history.Pop();
        }
    }
}