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

        public async Task<IReadOnlyList<Project>> ListAllWithTimeRecordsAsync()
        {
            return await _dbContext.Set<Project>().Include(p => p.TimeRecords).ToListAsync();
        }

    }

}
