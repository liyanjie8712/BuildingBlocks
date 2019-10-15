namespace Liyanjie.FakeMQ
{
    public interface IEventStore
    {
        bool Add(Event @event);
        Event Get(string type, long timestamp);
    }
}
