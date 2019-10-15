using System.Threading;
using System.Threading.Tasks;

namespace Liyanjie.FakeMQ
{
    public class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        readonly EventBus eventBus;
        public BackgroundService(EventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventBus.ProcessAsync(stoppingToken);
        }
    }
}
