using System;
using System.Threading.Tasks;

namespace MongoDB.Driver
{
    public static class MongoClientExtensions
    {
        public static async Task<bool> ExecWithTransactionAsync(
            this IMongoClient mongoClient,
            params Func<IClientSessionHandle, Task<bool>>[] funcs)
        {
            using var session = await mongoClient.StartSessionAsync();
            session.StartTransaction();

            var flag = true;
            foreach (var item in funcs)
            {
                flag = flag && await item(session);
            }

            if (flag)
                await session.CommitTransactionAsync();
            else
                await session.AbortTransactionAsync();

            return flag;
        }

        public static Task<bool> ExecWithTransactionAsync(
            this IMongoDatabase mongoDatabase,
            params Func<IClientSessionHandle, Task<bool>>[] funcs)
            => ExecWithTransactionAsync(mongoDatabase.Client, funcs);

        public static Task<bool> ExecWithTransactionAsync(
            this IMongoContext mongoContext,
            params Func<IClientSessionHandle, Task<bool>>[] funcs)
            => ExecWithTransactionAsync(mongoContext.MongoClient, funcs);
    }
}
