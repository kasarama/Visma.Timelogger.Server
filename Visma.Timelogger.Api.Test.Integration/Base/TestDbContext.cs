using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Domain.Entities;
using Visma.Timelogger.Persistence;
using Visma.Timelogger.Persistence.EntityConfigurations;

namespace Visma.Timelogger.Api.Test.Integration.Base
{
    public class TestDbContext : ProjectDbContext
    {
        public TestDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TimeRecordEntityConfiguration());
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasData(TestData.ActiveProject);
            modelBuilder.Entity<Project>().HasData(TestData.InactiveProject);
        }
    }
}
