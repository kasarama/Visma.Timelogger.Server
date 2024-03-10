using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdForFreelancerAsync(Guid projectId, Guid freelancerId);
        Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId);
        Task<Guid> AddTimeRecordAsync(Project entity);
        Task<List<Project>> GetListForFreelancerAsync(Guid freelancerId);
    }
}
