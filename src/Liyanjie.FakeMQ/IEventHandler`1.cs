using System.Threading.Tasks;

namespace Liyanjie.FakeMQ
{
    public interface IEventHandler<TEventMessage>
        where TEventMessage : IEventMessage
    {
        Task<bool> HandleAsync(TEventMessage @event);
    }
}
