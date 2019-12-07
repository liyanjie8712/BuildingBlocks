using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="ifPredicate"></param>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        public static IQueryable<T> IfWhere<T>(this IQueryable<T> source, Func<bool> ifPredicate, Expression<Func<T, bool>> wherePredicate)
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
