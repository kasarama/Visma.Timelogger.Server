namespace Visma.Timelogger.Application.ViewModels
{
    public class TimeRecordViewModel
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
