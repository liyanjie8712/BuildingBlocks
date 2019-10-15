using System;

namespace Liyanjie.FakeMQ
{
    public  class Process
    {
        public string Subscription { get; set; }
        public long Timestamp { get; set; } = DateTimeOffset.Now.Ticks;
    }
}
