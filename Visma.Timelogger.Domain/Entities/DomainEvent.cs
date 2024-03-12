using System.Text.Json;

namespace Visma.Timelogger.Domain.Entities
{
    public class DomainEvent
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public Guid AggregateId { get; set; }
        public bool IsPublished { get; set; }
        public string Data { get; set; } = "{}";
    }
}
