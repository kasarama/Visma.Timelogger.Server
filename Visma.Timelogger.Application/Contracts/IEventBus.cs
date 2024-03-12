using System;
using Visma.Timelogger.Application.Events;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : Event;

        Task SubscribeAsync<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
