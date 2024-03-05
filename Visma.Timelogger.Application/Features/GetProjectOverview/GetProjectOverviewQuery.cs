using Visma.Timelogger.Application.VieModels;
namespace Visma.Timelogger.Application.Features.GetProjectOverview
{
    public class GetProjectOverviewQuery : Message<ProjectOverviewViewModel>
    {
        public Guid ProjectId { get; set; }
        public GetProjectOverviewQuery(Guid projectId, Guid userId)
        {
            UserId = userId;
            ProjectId = projectId;
        }
    }
}
