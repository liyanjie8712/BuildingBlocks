using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// Identity
    /// </summary>
    public class Identity<TType, TValue> : _ValueObject where TType : struct
    {
        /// <summary>
        /// 类型
        /// </summary>
        public TType Type { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identity"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static Identity<TType, TValue> Create(TType type, TValue identity, string remark) => new Identity<TType, TValue>
        {
            Type = type,
            Value = identity,
            Remark = remark,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Identity : Identity<int, string>
    {
    }
}
