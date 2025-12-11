namespace BookShelf.Application.Commands
{
    public interface ICommandHandler<TCommand, TResult>
    {
        TResult Handle(TCommand command);
    }
}