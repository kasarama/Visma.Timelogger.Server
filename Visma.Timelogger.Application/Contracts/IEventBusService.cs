using Visma.Timelogger.Application.Events;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IEventBusService
    {
        Task PublishEvent<T>(T @event) where T : Event;
    }
}
