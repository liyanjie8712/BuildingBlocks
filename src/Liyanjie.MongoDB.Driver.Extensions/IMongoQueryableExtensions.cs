using System;
using System.Linq.Expressions;

using MongoDB.Driver.Linq;

namespace MongoDB.Driver.Linq
{
    public static class IMongoQueryableExtensions
    {
        public static IMongoQueryable<TSource> IfWhere<TSource>(this IMongoQueryable<TSource> source,
            Func<bool> ifPredicate,
            Expression<Func<TSource, bool>> wherePredicate)
        {
            if (ifPredicate == null)
                throw new ArgumentNullException(nameof(ifPredicate));

            if (wherePredicate == null)
                throw new ArgumentNullException(nameof(wherePredicate));

            return ifPredicate.Invoke()
                ? source.Where(wherePredicate)
                : source;
        }
    }
}
