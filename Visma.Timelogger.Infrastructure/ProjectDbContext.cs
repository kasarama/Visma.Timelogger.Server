using Microsoft.EntityFrameworkCore;
using Persisence.Entities;
using Persistence.Entities;
using Visma.Timelogger.Persistence.EntityConfigurations;

namespace Visma.Timelogger.Persistence
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }

        public static ModelBuilder BaseOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(new ProjectEntityConfiguration()
                                                                 .GetType().Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(new TimeRecordEntityConfiguration()
                                                                 .GetType().Assembly);
            return modelBuilder;
        }
    }
}