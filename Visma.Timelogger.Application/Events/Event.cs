namespace Visma.Timelogger.Application.Events
{
    public abstract class Event
    {
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
    }
}
