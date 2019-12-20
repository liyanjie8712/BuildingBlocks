﻿using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 证件
    /// </summary>
    public class Licence<TType> : ValueObject
    {
        /// <summary>
        /// 类型
        /// </summary>
        public TType Type { get; set; }

        /// <summary>
        /// 号码
        /// </summary>        
        public string Number { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Number;
        }
    }

    /// <summary>
    /// 证件
    /// </summary>
    public class Licence : Licence<string> { }
}
