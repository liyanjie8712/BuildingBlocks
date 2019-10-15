using Liyanjie.FakeMQ;

using Microsoft.EntityFrameworkCore;

using Liyanjie.FakeMQ.Test.Domains;
using Liyanjie.FakeMQ.Test.Infrastructure.EntityConfigurations;

namespace Liyanjie.FakeMQ.Test.Infrastructure
{
    public class SqliteContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Process> Processes { get; set; }

        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new ProcessConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
        }
    }
}
