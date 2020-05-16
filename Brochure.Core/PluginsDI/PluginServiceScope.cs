using System;
using AspectCore.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    public class PluginServiceScope : IServiceScope
    {
        private readonly IServiceResolver serviceProvider;

        public PluginServiceScope (IServiceResolver serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public IServiceProvider ServiceProvider => serviceProvider.GetService<IPluginServiceProvider> () as PluginsServiceProvider;

        public void Dispose ()
        {
            serviceProvider.Dispose ();
        }
    }

}