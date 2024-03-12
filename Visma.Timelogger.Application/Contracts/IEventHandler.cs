namespace Visma.Timelogger.Application.Contracts
{
    public interface IEventHandler<TEvent> 
    {
        Task Handle(TEvent @event);
    }
}
