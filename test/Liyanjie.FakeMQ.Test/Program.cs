using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Polly;

using Liyanjie.FakeMQ.Test.Infrastructure;

namespace Liyanjie.FakeMQ.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            MigrateDbContext(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void MigrateDbContext(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<SqliteContext>>();
            var context = services.GetRequiredService<SqliteContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {nameof(SqliteContext)}");

                Policy.Handle<Exception>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(6),
                            TimeSpan.FromSeconds(9),
                    })
                    .Execute(() =>
                    {
                        context.Database.Migrate();
                    });

                logger.LogInformation($"Migrated database associated with context {nameof(SqliteContext)}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database used on context {nameof(SqliteContext)}");
            }
        }
    }
}
