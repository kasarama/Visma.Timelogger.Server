using Visma.Timelogger.Application.Contracts;

namespace Visma.Timelogger.Persistence.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        public readonly ProjectDbContext _dbContext;

        public BaseRepository(ProjectDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
