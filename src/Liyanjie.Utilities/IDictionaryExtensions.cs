using System.Linq;
using System.Reflection;

namespace System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static TModel BuildModel<TModel>(this IDictionary<string, object> dictionary)
        where TModel : new()
        {
            var output = new TModel();

            foreach (var property in typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!property.CanWrite)
                    continue;

                var objectValue = dictionary[property.Name];

                object value = null;

                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var propertyElementType = property.PropertyType.HasElementType
                        ? property.PropertyType.GetElementType()
                        : property.PropertyType.IsConstructedGenericType
                            ? property.PropertyType.GenericTypeArguments[0]
                            : null;
                    var inputArray = Enumerable.Cast<object>((IEnumerable)objectValue);
                    var outputArray = Array.CreateInstance(propertyElementType ?? typeof(object), inputArray.Count());
                    inputArray
                        .Select(_ => propertyElementType == null ? _ : Convert.ChangeType(_, propertyElementType))
                        .ToArray()
                        .CopyTo(outputArray, 0);
                    value = outputArray;
                }
                else
                    value = Convert.ChangeType(objectValue, property.PropertyType);

                if (value == null)
                    continue;

                property.SetValue(output, value);
            }

            return output;
        }
    }
}
