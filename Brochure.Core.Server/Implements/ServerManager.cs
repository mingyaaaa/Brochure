using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Brochure.Core.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
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

        public IServiceResolver BuildProvider()
        {
            DI.ServiceProvider = Services.Build();
            return DI.ServiceProvider;
        }
    }
}
