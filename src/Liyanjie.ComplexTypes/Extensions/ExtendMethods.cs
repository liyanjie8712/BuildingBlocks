using System.Reflection;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtendMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origin"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Assign<T>(this T origin, T value)
            where T : ValueObject
        {
            var type = origin.GetType();
            var type_ComplexType = typeof(ValueObject);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (type_ComplexType.IsAssignableFrom(property.PropertyType))
                    (property.GetValue(origin) as ValueObject).Assign(property.GetValue(value) as ValueObject);
                else if (property.CanWrite && value != null)
                    property.SetValue(origin, property.GetValue(value));
            }
            return origin;
        }
    }
}
