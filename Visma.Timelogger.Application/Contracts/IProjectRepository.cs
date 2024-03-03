using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IProjectRepository : IAsyncRepository<Project>
    {
        Task<IReadOnlyList<Project>> ListAllWithTimeRecordsAsync();

        Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId);
    }
}
