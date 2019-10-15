using System;
using System.Text.Json;

namespace Liyanjie.FakeMQ
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; } = DateTimeOffset.Now.Ticks;

        internal object GetMsgObject(Type messageType) => JsonSerializer.Deserialize(Message, messageType);

        internal static string GetMsgString<TMessage>(TMessage t) where TMessage : IEventMessage => JsonSerializer.Serialize(t);
    }
}
