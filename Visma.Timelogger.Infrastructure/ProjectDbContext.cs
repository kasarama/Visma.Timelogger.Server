using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
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
                var pro = DevTestData.TestData.Item1;
                var tim = DevTestData.TestData.Item2;

                modelBuilder.Entity<Project>().HasData(pro);                modelBuilder.Entity<TimeRecord>().HasData(tim);

            };
        }
    }
}