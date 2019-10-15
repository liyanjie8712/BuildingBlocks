using Liyanjie.FakeMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFakeMQ<TEventStore, TProcessStore>(this IServiceCollection services)
            where TEventStore : class, IEventStore
            where TProcessStore : class, IProcessStore
        {
            services.AddTransient<IEventStore, TEventStore>();
            services.AddTransient<IProcessStore, TProcessStore>();
            services.AddSingleton<EventBus>();
            services.AddHostedService<BackgroundService>();

            return services;
        }
    }
}
