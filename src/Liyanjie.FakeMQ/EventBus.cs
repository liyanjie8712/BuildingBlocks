using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Liyanjie.FakeMQ
{
    public class EventBus : IDisposable
    {
        readonly ILogger<EventBus> logger;
        readonly IServiceScope scope;
        readonly IEventStore eventStore;
        readonly IProcessStore processStore;
        readonly IDictionary<Type, Type> subscriptions = new Dictionary<Type, Type>();
        public EventBus(IServiceProvider serviceProvider)
        {
            this.logger = serviceProvider.GetService<ILogger<EventBus>>();
            this.scope = serviceProvider.CreateScope();
            this.eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            this.processStore = scope.ServiceProvider.GetRequiredService<IProcessStore>();
        }

        public void Dispose()
        {
            subscriptions.Clear();
            scope.Dispose();
        }

        public void Publish<TEventMessage>(TEventMessage message)
            where TEventMessage : IEventMessage
        {
            var @event = new Event
            {
                Type = typeof(TEventMessage).FullName,
                Message = Event.GetMsgString(message),
            };
            if (TryExecute(() => eventStore.Add(@event)))
                logger.LogInformation($"Publish an event:Id={@event.Id},Type={@event.Type},Message={@event.Message}");
        }
        public void Subscribe<TEventMessage, TEventHandler>()
            where TEventMessage : IEventMessage
            where TEventHandler : IEventHandler<TEventMessage>
        {
            var messageType = typeof(TEventMessage);
            var handlerType = typeof(TEventHandler);
            var subscriptionId = GetSubscriptionId(messageType, handlerType);

            if (processStore.Add(new Process
            {
                Subscription = subscriptionId,
            }))
            {
                if (!subscriptions.ContainsKey(handlerType))
                    subscriptions.Add(handlerType, messageType);

                logger.LogInformation($"Subscribe:Id={subscriptionId}");
            }
        }
        public void Unsubscribe<TEventMessage, TEventHandler>()
            where TEventMessage : IEventMessage
            where TEventHandler : IEventHandler<TEventMessage>
        {
            var messageType = typeof(TEventMessage);
            var handlerType = typeof(TEventHandler);
            var subscriptionId = GetSubscriptionId(messageType, handlerType);

            if (subscriptions.ContainsKey(handlerType))
            {
                subscriptions.Remove(handlerType);
                TryExecute(() => processStore.Delete(subscriptionId));

                logger.LogInformation($"Unsubscribe:Id={subscriptionId}");
            }
        }

        internal async Task ProcessAsync(CancellationToken stoppingToken)
        {
            var tasks = new List<Task>();
            while (!stoppingToken.IsCancellationRequested)
            {
                tasks.Clear();

                foreach (var subscription in subscriptions)
                {
                    var messageType = subscription.Value;
                    var handlerType = subscription.Key;
                    var subscriptionId = GetSubscriptionId(messageType, handlerType);

                    var timestamp = processStore.Get(subscriptionId)?.Timestamp ?? 0L;

                    var @event = eventStore.Get(messageType.FullName, timestamp);
                    if (@event == null)
                        continue;

                    var handler = scope.ServiceProvider.GetService(handlerType);
                    if (handler == null)
                        continue;

                    tasks.Add(Task.Run(async () =>
                    {
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(messageType);
                        var result = await (Task<bool>)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { @event.GetMsgObject(messageType) });
                        if (result)
                            TryExecute(() => processStore.Update(subscriptionId, @event.Timestamp));

                        logger.LogInformation($"Process an event:Id={@event.Id},Handler={handlerType.FullName},Result={result}");
                    }));
                }

                await Task.WhenAll(tasks);
                await Task.Delay(1000);
            }
        }

        string GetSubscriptionId(Type messageType, Type handlerType) => $"{messageType.FullName}>{handlerType.FullName}";
        bool TryExecute(Func<bool> func, int retryCount = 3)
        {
            if (func != null)
                return Policy.HandleResult<bool>(false).Retry(retryCount).Execute(func);

            return false;
        }
    }
}
