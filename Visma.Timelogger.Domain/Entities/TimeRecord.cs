namespace Persistence.Entities
{
    public class TimeRecord
    {
        public Guid Id { get; set; }
        public Guid FreelancerId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
