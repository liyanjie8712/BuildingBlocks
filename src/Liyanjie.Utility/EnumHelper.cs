using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Liyanjie.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举类型的值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>() where T : struct
        {
            var type = typeof(T);
            return type.GetTypeInfo().IsEnum
                ? Enum.GetValues(type).Cast<T>().ToList()
                : throw new ArgumentException("enumType must be Enum type!");
        }
    }
}
