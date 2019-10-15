using System;
using System.Threading.Tasks;

using Liyanjie.FakeMQ;

using Liyanjie.FakeMQ.Test.Domains;

namespace Liyanjie.FakeMQ.Test.Infrastructure.EventHandlers
{
    public class MessageEventHandler : IEventHandler<MessageEvent>, IDisposable
    {
        readonly SqliteContext context;
        public MessageEventHandler(SqliteContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public async Task<bool> HandleAsync(MessageEvent @event)
        {
            context.Messages.Add(new Message
            {
                Content = @event.Message,
            });
            await context.SaveChangesAsync();

            return true;
        }
    }
}
