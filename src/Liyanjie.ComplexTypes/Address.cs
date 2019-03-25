using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 地址
    /// </summary>
    public class Address : _ValueObject
    {
        /// <summary>
        /// 行政区划
        /// </summary>
        public string ADCode { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ADCode;
            yield return Detail;
        }
    }
}
