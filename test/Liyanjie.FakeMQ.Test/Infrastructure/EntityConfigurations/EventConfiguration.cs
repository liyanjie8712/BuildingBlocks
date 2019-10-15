using Liyanjie.FakeMQ;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Liyanjie.FakeMQ.Test.Infrastructure.EntityConfigurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(_ => _.Type).HasMaxLength(50);

            builder.HasKey(_ => _.Id);

            builder.HasIndex(_ => _.Type);
            builder.HasIndex(_ => _.Timestamp);
        }
    }
}
