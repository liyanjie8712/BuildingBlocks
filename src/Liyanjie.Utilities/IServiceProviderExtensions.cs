using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static TService GetServiceOrCreateInstance<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            return (TService)GetServiceOrCreateInstance(serviceProvider, typeof(TService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static object GetServiceOrCreateInstance(this IServiceProvider serviceProvider, Type serviceType)
        {
            var service = serviceProvider.GetService(serviceType);
            if (service != null)
                return service;

            var typeInfo = serviceType.GetTypeInfo();
            if (typeInfo.IsInterface || typeInfo.IsAbstract || !typeInfo.IsClass)
                return null;

            var constructor = serviceType.GetConstructors().FirstOrDefault();
            if (constructor == null)
                return Activator.CreateInstance(serviceType);

            var parameters = constructor.GetParameters()
                .Select(_ => serviceProvider.GetService(_.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters);
        }
    }
}
