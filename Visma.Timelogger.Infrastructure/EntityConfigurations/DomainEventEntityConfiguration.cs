using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Persistence.EntityConfigurations
{
    public class DomainEventEntityConfiguration : IEntityTypeConfiguration<DomainEvent>
    {
        public void Configure(EntityTypeBuilder<DomainEvent> builder)
        {
            builder.HasKey(p => p.EventId);
            builder.Property(p => p.AggregateId).IsRequired();
            builder.Property(p => p.EventName).IsRequired();
            builder.Property(p => p.Data).IsRequired();
        }
    }
}
