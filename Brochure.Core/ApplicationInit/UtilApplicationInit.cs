using System;
using Brochure.Abstract;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Core
{
    public class UtilApplicationInit : IApplicationFunInit
    {
        private readonly IServiceCollection services;
        private readonly IApplicationFunInit nextApplicationFunInit;

        public UtilApplicationInit(IServiceCollection services, IApplicationFunInit nextApplicationFunInit)
        {
            this.services = services;
            this.nextApplicationFunInit = nextApplicationFunInit;
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


    }
}
