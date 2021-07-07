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
        readonly List<ServerPort> Ports = new();
        readonly List<ServerServiceDefinition> Services = new();
        readonly List<Func<IServiceProvider, ServerServiceDefinition>> ServiceFactories = new();

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Server CreateServer(IServiceProvider serviceProvider)
        {
            var server = new Server();
            foreach (var port in Ports)
            {
                server.Ports.Add(port);
            }
            foreach (var service in Services)
            {
                server.Services.Add(service);
            }
            foreach (var factory in ServiceFactories)
            {
                server.Services.Add(factory.Invoke(serviceProvider));
            }
            return server;
        }
    }
}
