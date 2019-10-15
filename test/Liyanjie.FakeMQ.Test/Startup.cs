using Liyanjie.FakeMQ;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Liyanjie.FakeMQ.Test.Domains;
using Liyanjie.FakeMQ.Test.Infrastructure;
using Liyanjie.FakeMQ.Test.Infrastructure.EventHandlers;

namespace Liyanjie.FakeMQ.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SqliteContext>(builder =>
            {
                builder.UseSqlite(@"Data Source=.\Database.sqlite");
            });

            services.AddTransient<MessageEventHandler>();

            services.AddFakeMQ<EventStore, ProcessStore>();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var eventBus = app.ApplicationServices.GetRequiredService<EventBus>();
            eventBus.Subscribe<MessageEvent, MessageEventHandler>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
