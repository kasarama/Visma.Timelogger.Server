using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Persistence.Repositories;

namespace Visma.Timelogger.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjectDbContext>(options =>
                       //     options.UseSqlServer(configuration.GetConnectionString("MMSQLConnectionString")));

            options.EnableSensitiveDataLogging().UseInMemoryDatabase("projects"));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));

            return services;
        }
    }
}
