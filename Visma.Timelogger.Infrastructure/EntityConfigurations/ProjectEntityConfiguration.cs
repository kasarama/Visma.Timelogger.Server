using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.EntityConfigurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Deadline);
            builder.Property(p => p.FreelancerId).IsRequired();
            builder.Property(p => p.CustomerId).IsRequired();
            builder.Property(p => p.Deadline).IsRequired();
            builder.Property(p => p.StartTime).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.HasMany(p => p.TimeRecords)
            .WithOne(tr => tr.Project)
            .HasForeignKey(tr => tr.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
