using System;
using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 状态
    /// </summary>
    /// <typeparam name="TStatus"></typeparam>
    public class Status<TStatus> : _ValueObject
        where TStatus : struct
    {
        /// <summary>
        /// 值
        /// </summary>
        public TStatus Value { get; set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTimeOffset ChangeTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static Status<TStatus> Create(TStatus status, string remark = null) => new Status<TStatus>
        {
            Value = status,
            Remark = remark,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return ChangeTime;
            yield return Remark;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Status : Status<int> { }
}
