using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Liyanjie.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// services.AddMvc(options => 
    /// { 
    ///     options.ModelBinderProviders.Insert(0, new DelimitedArrayModelBinderProvider());
    /// });
    /// </summary>
    public class DelimitedArrayModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!context.Metadata.IsEnumerableType)
                return null;

            var propertyName = context.Metadata.PropertyName;
            if (string.IsNullOrEmpty(propertyName))
                return null;

            var propertyAttribute = context.Metadata.ContainerType.GetProperty(propertyName).GetCustomAttributes<DelimitedArrayAttribute>(false).FirstOrDefault();
            if (propertyAttribute == null)
                return null;

            return new DelimitedArrayModelBinder(propertyAttribute.Delimiter);
        }
    }
}
