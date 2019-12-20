using System;
using System.Collections.Generic;

namespace Liyanjie.ComplexTypes
{
    public class School<TType> : ValueObject
    {
        public TType Type { get; set; }
        public string Name { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? GraduatedDate { get; set; }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Name;
        }
    }
    public class School : School<string> { }
}
