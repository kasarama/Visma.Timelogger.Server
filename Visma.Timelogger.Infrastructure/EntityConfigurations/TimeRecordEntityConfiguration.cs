using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.EntityConfigurations
{
    public class TimeRecordEntityConfiguration : IEntityTypeConfiguration<TimeRecord>
    {
        public void Configure(EntityTypeBuilder<TimeRecord> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FreelancerId).IsRequired();
            builder.Property(p => p.ProjectId).IsRequired();
            builder.Property(p => p.StartTime).IsRequired();
            builder.Property(p => p.DurationMinutes).IsRequired();
        }
    }
}
