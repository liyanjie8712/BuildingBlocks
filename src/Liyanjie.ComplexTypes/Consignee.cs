using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 收件人
    /// </summary>
    public class Consignee : _ValueObject
    {
        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Address.ADCode;
            yield return Address.Detail;
            yield return Contact.Type;
            yield return Contact.Name;
            yield return Contact.Number;
        }
    }
}
