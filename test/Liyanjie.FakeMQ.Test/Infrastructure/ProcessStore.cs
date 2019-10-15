using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Liyanjie.FakeMQ;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.FakeMQ.Test.Infrastructure
{
    public class ProcessStore : IProcessStore, IDisposable
    {
        readonly SqliteContext db;
        public ProcessStore(SqliteContext db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            this.db?.Dispose();
        }

        public bool Add(Process process)
        {
            if (db.Processes.Any(_ => _.Subscription == process.Subscription))
                return true;

            db.Processes.Add(process);

            return Save();
        }
        public Process Get(string subscription)
        {
            return db.Processes.AsNoTracking().SingleOrDefault(_ => _.Subscription == subscription);
        }
        public bool Update(string subscription, long timestamp)
        {
            var item = db.Processes.SingleOrDefault(_ => _.Subscription == subscription);
            if (item == null)
                return true;

            item.Timestamp = timestamp;

            return Save();
        }
        public bool Delete(string subscription)
        {
            var item = db.Processes.SingleOrDefault(_ => _.Subscription == subscription);
            if (item == null)
                return true;

            db.Processes.Remove(item);

            return Save();
        }

        bool Save()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }
    }
}
