using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class Name : ValueObject
    {
        /// <summary>
        /// 名
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// 姓（无姓氏不填）
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
