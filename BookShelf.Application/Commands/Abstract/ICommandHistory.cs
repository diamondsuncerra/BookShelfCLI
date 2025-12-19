using BookShelf.Application.Commands.Abstract;
public interface ICommandHistory
{
    void Push(IUndoable command);
     IUndoable? Pop();
}
