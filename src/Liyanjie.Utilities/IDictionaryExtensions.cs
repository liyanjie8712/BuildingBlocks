using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
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
            return (TModel)dictionary.BuildModel(typeof(TModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static object BuildModel(this IDictionary<string, object> dictionary, Type modelType)
        {
            var output = Activator.CreateInstance(modelType);

            dictionary = dictionary.ToDictionary(_ => _.Key.ToLower(), _ => _.Value);
            foreach (var property in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!property.CanWrite)
                    continue;

                object value = null;
                if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                {
                    var _prev = $"{property.Name.ToLower()}.";
                    var _dic = dictionary
                        .Where(_ => _.Key.StartsWith(_prev))
                        .ToDictionary(_ => _.Key.Substring(_prev.Length), _ => _.Value);
                    value = _dic.BuildModel(property.PropertyType);
                }
                else
                {
                    if (!dictionary.TryGetValue(property.Name.ToLower(), out var objectValue))
                        continue;

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
                    else if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                    {
                        var _prev = $"{property.Name.ToLower()}.";
                        var _dic = dictionary.Where(_ => _.Key.StartsWith(_prev)).ToDictionary(_ => _.Key, _ => _.Value);
                        value = _dic.BuildModel(property.PropertyType);
                    }
                    else
                        value = Convert.ChangeType(objectValue, property.PropertyType);
                }

                if (value == null)
                    continue;

                property.SetValue(output, value);
            }

            return output;
        }
    }
}
