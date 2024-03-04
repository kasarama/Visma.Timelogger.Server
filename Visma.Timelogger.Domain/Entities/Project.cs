namespace Visma.Timelogger.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid FreelancerId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public List<TimeRecord> TimeRecords { get; set; } = new List<TimeRecord>();
        public bool IsActive { get; set; }
    }
}
