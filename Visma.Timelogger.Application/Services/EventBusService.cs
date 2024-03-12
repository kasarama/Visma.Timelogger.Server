using AutoMapper;
using Microsoft.Extensions.Logging;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Events;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Services
{
    public class EventBusService : IEventBusService
    {
        private readonly IEventBus _eventBus;
        private readonly IAsyncRepository<DomainEvent> _eventRepository;
        private readonly ILogger<EventBusService> _logger;
        private readonly IMapper _mapper;

        public EventBusService(IEventBus eventBus,
                               IAsyncRepository<DomainEvent> eventRepository,
                               ILogger<EventBusService> logger,
                               IMapper mapper)
        {
            _eventBus = eventBus;
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            bool isPublished = false;
            try
            {
                await _eventBus.PublishAsync(@event);
                isPublished = true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("EventBus unavailable: Event {id} not published.\n {ex}", @event.EventId, ex);
            }

            DomainEvent domainEvent = _mapper.Map<DomainEvent>(@event);
            domainEvent.IsPublished = isPublished;
            await _eventRepository.AddAsync(domainEvent);
        }
    }
}
