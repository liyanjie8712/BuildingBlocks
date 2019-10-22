using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = ((ValueObject)obj).GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(_ => _ == null ? 0 : _.GetHashCode())
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ValueObject Clone()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}
