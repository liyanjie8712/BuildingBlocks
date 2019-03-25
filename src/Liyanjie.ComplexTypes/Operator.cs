using System;
using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 操作人
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class Operator<TType> : _ValueObject
        where TType : struct
    {
        /// <summary>
        /// 类型
        /// </summary>
        public TType Type { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public Guid? Identity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Identity;
            yield return Remark;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Operator : Operator<int> { }
}
