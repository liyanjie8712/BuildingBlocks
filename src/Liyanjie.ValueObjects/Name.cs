using System.Collections.Generic;

namespace Liyanjie.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Name : ValueObject
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return MiddleName;
            yield return FamilyName;
        }
    }
}
