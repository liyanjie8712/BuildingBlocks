using System;
using System.Collections;
using System.Linq;
using System.Reflection;

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
        public static TModel Cast<TModel>(this QueryString queryString) where TModel : new()
        {
            var output = new TModel();

            if (!queryString.HasValue)
                return output;

            var query = new QueryCollection(QueryHelpers.ParseNullableQuery(queryString.Value));

            foreach (var property in typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!property.CanWrite)
                    continue;

                var stringValues = query[property.Name];

                object value = null;

                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var propertyElementType = property.PropertyType.HasElementType
                        ? property.PropertyType.GetElementType()
                        : property.PropertyType.IsConstructedGenericType
                            ? property.PropertyType.GenericTypeArguments[0]
                            : null;
                    var inputArray = Enumerable.ToArray(stringValues);
                    var outputArray = Array.CreateInstance(propertyElementType ?? typeof(object), inputArray.Length);
                    stringValues
                        .Select(_ => propertyElementType == null ? _ : Convert.ChangeType(_, propertyElementType))
                        .ToArray()
                        .CopyTo(outputArray, 0);
                    value = outputArray;
                }
                else
                    value = Convert.ChangeType(stringValues.FirstOrDefault(), property.PropertyType);

                if (value == null)
                    continue;

                property.SetValue(output, value);
            }

            return output;
        }
    }
}
