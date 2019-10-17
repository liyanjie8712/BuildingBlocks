using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryStringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static TModel BuildModel<TModel>(this QueryString queryString) where TModel : new()
        {
            var output = new TModel();

            if (!queryString.HasValue)
                return output;

            return new QueryCollection(QueryHelpers.ParseNullableQuery(queryString.Value)).BuildModel<TModel>();
        }
    }
}
