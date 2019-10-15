using Liyanjie.FakeMQ;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Liyanjie.FakeMQ.Test.Infrastructure.EntityConfigurations
{
    public class ProcessConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.HasKey(_ => _.Subscription);
        }
    }
}
