using System;

namespace Liyanjie.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// 配合DelimitedArrayModelBinderProvider使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DelimitedArrayAttribute : Attribute
    {
        /// <summary>
        /// 分隔符，默认为“,”
        /// </summary>
        public string Delimiter { get; set; } = ",";
    }
}
