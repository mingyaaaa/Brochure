using System;
using AspectCore.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    public class PluginServiceScope : IServiceScope, IServiceProvider
    {
        private readonly IServiceResolver serviceProvider;

        public PluginServiceScope (IServiceResolver serviceProvider)
        {
            this.serviceProvider = serviceProvider.CreateScope ();
        }
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => this;

        public void Dispose ()
        {
            serviceProvider.Dispose ();
        }

        public object GetService (Type serviceType)
        {
            return this.serviceProvider.GetService (serviceType);
        }
    }

}