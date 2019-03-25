using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 判断此集合为null或空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
            => source == null || source.Count() == 0;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> source, string separator = ",", Func<T, string> toString = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            toString = toString ?? (_ => _.ToString());

            return string.Join(separator, source.Select(toString));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="separatorSelector"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> separatorSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (separatorSelector == null)
                throw new ArgumentNullException(nameof(source));

            var output = new List<IEnumerable<T>>();
            var i = -1;
            var length = source.Count();
            for (int j = 0; j < length; j++)
            {
                if (separatorSelector(source.ElementAt(j)))
                {
                    var item = source.Where((_, index) => index > i && index < j).ToList();
                    if (item.Count() > 0)
                        output.Add(item);
                    i = j;
                }
            }
            if (i < length - 1)
            {
                var item = source.Where((_, index) => index > i).ToList();
                if (item.Count() > 0)
                    output.Add(item);
            }

            return output;
        }
    }
}
