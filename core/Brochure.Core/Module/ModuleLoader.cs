using System;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Module
{
    /// <summary>
    /// The module loader.
    /// </summary>
    public class ModuleLoader : IModuleLoader
    {
        private readonly ILogger<ModuleLoader> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoader"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ModuleLoader(ILogger<ModuleLoader> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Loads the module.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly.</param>
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
                    item.Initialization(services.BuildPluginServiceProvider());
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