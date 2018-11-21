using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;
using Brochure.Core.Core;

namespace Brochure.Core.Server.Implements
{
    public class ServerManager : IServerManager
    {
        public IServiceContainer Services { get; }
        public ServerManager()
        {
            Services = new ServiceContainer();
        }
        public ServerManager(IServiceCollection server)
        {
            Services = server.ToServiceContainer();
        }

        public IServiceProvider BuildProvider()
        {
            DI.ServiceProvider = Services.Build();
            return DI.ServiceProvider;
        }
    }
}
