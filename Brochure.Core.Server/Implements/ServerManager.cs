using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            return Services.Build();
        }
    }
}
