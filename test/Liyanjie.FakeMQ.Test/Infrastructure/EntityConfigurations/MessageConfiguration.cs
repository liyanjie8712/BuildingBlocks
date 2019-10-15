using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Liyanjie.FakeMQ.Test.Domains;

namespace Liyanjie.FakeMQ.Test.Infrastructure.EntityConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(_ => _.Content).HasMaxLength(500);

            builder.HasKey(_ => _.Id);
        }
    }
}
