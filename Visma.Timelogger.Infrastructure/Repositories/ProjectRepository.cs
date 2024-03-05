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

        public async Task AddTimeRecordAsync(Project entity)
        {
            _dbContext.Entry(entity.TimeRecords.Last()).State = EntityState.Added;
            _dbContext.Projects.Include(p => p.TimeRecords);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Project?> GetByIdForFreelancerAsync(Guid projectId, Guid freelancerId)
        {
            var result = _dbContext.Projects
                            .Where(p => (p.Id == projectId && p.FreelancerId == freelancerId))
                            .Include(p => p.TimeRecords);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetListForFreelancerAsync(Guid freelancerId)
        {
            var result = await _dbContext.Projects
                        .Include(p => p.TimeRecords)
                        .Where(p => p.FreelancerId == freelancerId)
                        .OrderBy(p => p.Deadline)
                        .ToListAsync();
            return result;
        }
    }

}
