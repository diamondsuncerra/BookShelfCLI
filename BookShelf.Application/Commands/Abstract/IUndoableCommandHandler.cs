namespace BookShelf.Application.Commands.Abstract
{
    public interface IUndoableCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    {
        void Undo();
    }
}