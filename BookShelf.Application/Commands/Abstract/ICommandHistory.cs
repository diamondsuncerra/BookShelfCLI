namespace BookShelf.Application.Commands.Abstract
{
    public interface ICommandHistory
    {
        void Push(IUndoableCommandHandler<object, object> handler); // or non-generic base
        IUndoableCommandHandler<object, object>? Pop();
    }
}