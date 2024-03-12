using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IProjectRepository : IAsyncRepository<Project>
    {
        Task<Project?> GetByIdForFreelancerAsync(Guid projectId, Guid freelancerId);
        Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId);
        Task AddTimeRecordAsync(Project entity);
        Task<List<Project>> GetListForFreelancerAsync(Guid freelancerId);
    }
}
