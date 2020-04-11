using System;
using Brochure.Abstract;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Core
{
    public class UtilApplicationInit : IApplicationFunInit
    {
        private readonly IServiceCollection services;
        private IApplicationFunInit nextApplicationFunInit;

        public UtilApplicationInit(IServiceCollection services)
        {
            this.services = services;

        }

        public void Init()
        {
            services.AddSingleton<IJsonUtil>(new JsonUtil());
            services.AddSingleton<IReflectorUtil>(new ReflectorUtil());
            services.AddSingleton<IObjectFactory>(new Brochure.Abstract.ObjectFactory());
            services.AddSingleton<ISysDirectory>(new SysDirectory());
            services.AddSingleton<IPluginManagers>(new PluginManagers());
            nextApplicationFunInit?.Init();
        }

        public void SetNext(IApplicationFunInit nextApplicationFunInit)
        {
            this.nextApplicationFunInit = nextApplicationFunInit;
        }

    }
}
