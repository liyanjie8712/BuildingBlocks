using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Liyanjie.AspNetCore.Mvc.Extensions
{
    /// <summary>
    /// [ModelBinder(BinderType = typeof(DelimitedArrayModelBinder))]
    /// </summary>
    public class DelimitedArrayModelBinder : IModelBinder
    {
        readonly string delimiter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        public DelimitedArrayModelBinder(string delimiter = ",")
        {
            this.delimiter = delimiter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            if (!bindingContext.ModelMetadata.IsEnumerableType)
                return Task.FromResult(0);

            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (values == ValueProviderResult.None)
                return Task.FromResult(0);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, values);

            var elementType = bindingContext.ModelMetadata.ElementType;
            var converter = TypeDescriptor.GetConverter(elementType);

            try
            {
                var value = values.SelectMany(_ => _.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(__ => converter.ConvertFromString(__))).ToArray();
                var typedValue = Array.CreateInstance(elementType, value.Length);
                value.CopyTo(typedValue, 0);
                bindingContext.Result = ModelBindingResult.Success(typedValue);
            }
            catch (Exception e)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, e.Message);
            }

            return Task.FromResult(0);
        }
    }
}
