using System.Reflection;

namespace Liyanjie.TypeBuilder
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TypeBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetProperty(string name)
        {
            return GetType().GetTypeInfo().GetProperty(name).GetValue(this, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, object value)
        {
            GetType().GetTypeInfo().GetProperty(name).SetValue(this, value, null);
        }
    }
}
