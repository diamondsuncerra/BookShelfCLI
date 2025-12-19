namespace BookShelf.Application.Commands.Abstract
{
    public interface ICommandHandler<TCommand, TResult>
    {
        TResult Handle(TCommand command);
    }
}