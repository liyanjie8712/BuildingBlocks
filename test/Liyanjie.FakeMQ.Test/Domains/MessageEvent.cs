using Liyanjie.FakeMQ;

namespace Liyanjie.FakeMQ.Test.Domains
{
    public class MessageEvent : IEventMessage
    {
        public string Message { get; set; }
    }
}
