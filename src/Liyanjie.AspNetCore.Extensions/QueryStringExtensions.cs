﻿using System.Linq;
using System.Reflection;

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
        public static TModel BuildModel<TModel>(this QueryString queryString)
            where TModel : new()
        {
            if (!queryString.HasValue)
                return new TModel();

            return QueryHelpers.ParseNullableQuery(queryString.Value)
                .ToDictionary(_ => _.Key, _ => (object)_.Value)
                .BuildModel<TModel>();
        }
    }
}
