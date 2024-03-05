using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Domain.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TimeRecordEntityConfiguration());
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                List<TimeRecord> timeRecords = DevTestData.TestData.Item2;
                Project[] projects = DevTestData.TestData.Item1;

                modelBuilder.Entity<Project>().HasData(projects);
                modelBuilder.Entity<TimeRecord>().HasData(timeRecords);
            };
        }
    }
}