using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Visma.Timelogger.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjectDbContext>(options =>
                options.UseInMemoryDatabase("projects"));
            return services;
        }
    }
}
