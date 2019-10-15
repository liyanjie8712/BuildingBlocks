using System.Collections.Generic;

namespace Liyanjie.FakeMQ
{
    public interface IProcessStore
    {
        bool Add(Process process);
        Process Get(string subscription);
        bool Update(string subscription, long timestamp);
        bool Delete(string subscription);
    }
}
