namespace Visma.Timelogger.Application.Events
{
    public class TimeRecordCreatedEvent : Event
    {
        public Guid FreelancerId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
