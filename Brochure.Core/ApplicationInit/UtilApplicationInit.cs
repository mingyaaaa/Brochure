using System;
using Brochure.Abstract;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Core
{
    public class UtilApplicationInit : IApplicationFunInit
    {
        private readonly IServiceCollection services;
        private IApplicationFunInit nextApplicationFunInit;

        public UtilApplicationInit (IServiceCollection services)
        {
            this.services = services;

        }

        public void Init ()
        {
            services.TryAddSingleton<IJsonUtil> (new JsonUtil ());
            services.TryAddSingleton<IReflectorUtil> (new ReflectorUtil ());
            services.TryAddSingleton<IObjectFactory> (new Brochure.Abstract.ObjectFactory ());
            services.TryAddSingleton<ISysDirectory> (new SysDirectory ());
            services.TryAddSingleton<IPluginManagers> (new PluginManagers ());
            nextApplicationFunInit?.Init ();
        }

        public void SetNext (IApplicationFunInit nextApplicationFunInit)
        {
            this.nextApplicationFunInit = nextApplicationFunInit;
        }

    }
}