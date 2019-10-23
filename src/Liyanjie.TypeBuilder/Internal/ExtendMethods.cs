#if NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liyanjie.TypeBuilder.Internal
{
    internal static class ExtendMethods
    {
        public static Type AsType(this System.Reflection.Emit.GenericTypeParameterBuilder builder)
        {
            return builder.UnderlyingSystemType;
        }
        
        public static Type AsType(this System.Reflection.Emit.TypeBuilder builder)
        {
            return builder.UnderlyingSystemType;
        }
    }
}
#endif
