using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 位置
    /// </summary>
    public class Geolocation : _ValueObject
    {
        /// <summary>
        /// 所在经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 所在纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Longitude;
            yield return Latitude;
        }
    }
}
