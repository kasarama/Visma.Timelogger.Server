using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ProjectDbContext projectDbContext) : base(projectDbContext)
        {
        }

        public async Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId)
        {
            var result = await _dbContext.Projects
                            .Where(p => p.IsActive && p.Id == projectId && p.FreelancerId == freelancerId)
                            .Include(p => p.TimeRecords)
                            .FirstOrDefaultAsync();
            return result;
        }

        public async Task<IReadOnlyList<Project>> ListAllWithTimeRecordsAsync()
        {
            var result = await _dbContext.Set<Project>().Include(p => p.TimeRecords).ToListAsync();
            return result;
        }

    }

}
