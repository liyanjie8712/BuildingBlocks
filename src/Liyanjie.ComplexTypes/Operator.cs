using System;
using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 操作人
    /// </summary>
    /// <typeparam name="TStatus"></typeparam>
    public class Operator<TStatus> : ValueObject
    {
        /// <summary>
        /// 类型
        /// </summary>
        public TStatus Status { get; set; }

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
        /// <param name="status"></param>
        /// <param name="identity"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static Operator<TStatus> Create(TStatus status, Guid? identity, string remark = default) => new Operator<TStatus>
        {
            Status = status,
            Identity = identity,
            Remark = remark,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return Identity;
            yield return Remark;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Operator : Operator<int> { }
}
