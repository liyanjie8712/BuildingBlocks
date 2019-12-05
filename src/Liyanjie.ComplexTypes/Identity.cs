using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// Identity
    /// </summary>
    public class Identity<TType, TValue> : ValueObject
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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Identity<TType, TValue> Create(TType type, TValue value) => new Identity<TType, TValue>
        {
            Type = type,
            Value = value,
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
