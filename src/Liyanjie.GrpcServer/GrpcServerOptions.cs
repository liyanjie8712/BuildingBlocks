using System;
using System.Collections.Generic;

using Grpc.Core;

namespace Liyanjie.GrpcServer
{
    /// <summary>
    /// 
    /// </summary>
    public class GrpcServerOptions
    {
        readonly IList<ServerPort> Ports = new List<ServerPort>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public GrpcServerOptions AddPort(string host, int port, ServerCredentials credentials = default)
        {
            Ports.Add(new ServerPort(host, port, default == credentials ? ServerCredentials.Insecure : credentials));
            return this;
        }

        readonly IList<ServerServiceDefinition> Services = new List<ServerServiceDefinition>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDefinition"></param>
        /// <returns></returns>
        public GrpcServerOptions AddService(ServerServiceDefinition serviceDefinition)
        {
            Services.Add(serviceDefinition);
            return this;
        }

#if NETSTANDARD
        readonly IList<Func<IServiceProvider, ServerServiceDefinition>> ServiceFactories = new List<Func<IServiceProvider, ServerServiceDefinition>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceFactory"></param>
        /// <returns></returns>
        public GrpcServerOptions AddService(Func<IServiceProvider, ServerServiceDefinition> serviceFactory)
        {
            ServiceFactories.Add(serviceFactory);
            return this;
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Grpc.Core.Server CreateServer(
#if NETSTANDARD
            IServiceProvider serviceProvider
#endif
            )
        {
            var server = new Grpc.Core.Server();
            foreach (var port in Ports)
            {
                server.Ports.Add(port);
            }
            foreach (var service in Services)
            {
                server.Services.Add(service);
            }
#if NETSTANDARD
            foreach (var factory in ServiceFactories)
            {
                server.Services.Add(factory.Invoke(serviceProvider));
            }
#endif
            return server;
        }
    }
}
