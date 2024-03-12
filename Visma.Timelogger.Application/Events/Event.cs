namespace Visma.Timelogger.Application.Events
{
    public abstract class Event
    {
        public Guid EventId { get; set; } = new Guid();
        public Guid AggregateId { get; set; }
    }
}
