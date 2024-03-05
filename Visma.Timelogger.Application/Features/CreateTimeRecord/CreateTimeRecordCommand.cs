using Visma.Timelogger.Application.RequestModels;

namespace Visma.Timelogger.Application.Features.CreateTimeRecord
{
    public class CreateTimeRecordCommand : Message<bool>
    {
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }

        public CreateTimeRecordCommand(CreateTimeRecordRequestModel request, Guid userId)
        {
            UserId = userId;
            ProjectId = request.ProjectId;
            StartTime = request.StartTime;
            DurationMinutes = request.DurationMinutes;
        }
    }
}
