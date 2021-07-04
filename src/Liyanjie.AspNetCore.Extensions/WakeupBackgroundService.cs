using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public class WakeupBackgroundService : BackgroundService
    {
        readonly ILogger<WakeupBackgroundService> logger;
        readonly IHttpClientFactory httpClientFactory;
        readonly string wakeupUrl;
        public WakeupBackgroundService(
            ILogger<WakeupBackgroundService> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.wakeupUrl = configuration.GetValue<string>("WakeupUrl", null) ?? throw new ApplicationException("找不到 WakeupUrl 配置项");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrEmpty(wakeupUrl))
                return;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                logger.LogTrace("Wake up!");
                try
                {
                    using var http = httpClientFactory.CreateClient();
                    var response = await http.GetStringAsync(wakeupUrl, stoppingToken);

                    logger.LogTrace(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
