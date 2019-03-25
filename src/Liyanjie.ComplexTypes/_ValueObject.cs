using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.ComplexTypes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class _ValueObject
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
            var otherValues = ((_ValueObject)obj).GetAtomicValues().GetEnumerator();
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
                .Select(_ => _ != null ? _.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public _ValueObject Clone()
        {
            return this.MemberwiseClone() as _ValueObject;
        }
    }
}
