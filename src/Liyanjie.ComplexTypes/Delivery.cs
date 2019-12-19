using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class Delivery<TIdentity> : ValueObject
    {
        /// <summary>
        /// 
        /// </summary>
        public TIdentity Identity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return TrackingNumber;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Delivery : Delivery<string>
    {
    }
}
