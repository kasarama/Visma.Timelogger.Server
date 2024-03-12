using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Events.Sub
{
    public class ProjectCreatedEvent : Event
    {
        public Guid FreelancerId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
