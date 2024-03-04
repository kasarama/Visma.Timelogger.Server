using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Contracts
{
    public interface IProjectRepository
    {
        Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId);
        Task AddTimeRecordAsync(Project entity);
    }
}
