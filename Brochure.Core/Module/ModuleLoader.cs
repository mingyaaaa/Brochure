using System;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Module
{
    public class ModuleLoader : IModuleLoader
    {
        private readonly ILogger<ModuleLoader> logger;

        public ModuleLoader(ILogger<ModuleLoader> logger)
        {
            this.logger = logger;
        }
        public void LoadModule(IServiceProvider provider, IServiceCollection services, Assembly assembly)
        {
            try
            {
                var refUtils = provider.GetService<IReflectorUtil>();
                var modules = refUtils.GetObjectOfBase<IModule>(assembly);
                if (!modules.Any())
                    return;
                foreach (var item in modules)
                {
                    item.ConfigModule(services);
                }
                foreach (var item in modules)
                {
                    item.Initialization(services.BuildServiceProvider());
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }

        }
    }
}