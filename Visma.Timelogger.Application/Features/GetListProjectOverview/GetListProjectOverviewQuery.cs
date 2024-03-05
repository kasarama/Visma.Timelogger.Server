using Visma.Timelogger.Application.VieModels;

namespace Visma.Timelogger.Application.Features.GetListProjectOverview
{
    public class GetListProjectOverviewQuery : Message<List<ProjectOverviewViewModel>>
    {
        public GetListProjectOverviewQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
