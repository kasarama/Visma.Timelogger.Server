namespace Visma.Timelogger.Application.Contracts
{
    public interface IAsyncRepository<T> where T : class
    {
        Task AddAsync(T entity);
    }
}
