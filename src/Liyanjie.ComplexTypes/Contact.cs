﻿using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class Contact<TType> : ValueObject
    {
        /// <summary>
        /// 联系方式
        /// </summary>
        public TType Type { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// 号码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Name;
            yield return Number;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Contact : Contact<string> { }
}
