namespace Visma.Timelogger.Application.RequestModels
{
    public class CreateTimeRecordRequestModel
    {
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
    }
}
