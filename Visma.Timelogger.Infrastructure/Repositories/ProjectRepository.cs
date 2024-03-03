using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        protected readonly ProjectDbContext _dbContext;

        public ProjectRepository(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project?> GetActiveByProjectIdForFreelancerAsync(Guid projectId, Guid freelancerId)
        {
            var result = await _dbContext.Projects
                            .Where(p => p.IsActive && p.Id == projectId && p.FreelancerId == freelancerId)
                            .Include(p => p.TimeRecords)
                            .FirstOrDefaultAsync();
            return result;
        }

        public async Task UpdateAsync(Project entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }

}
