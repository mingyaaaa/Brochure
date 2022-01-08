using System;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core.Extenstions;
using Brochure.Core.PluginsDI;
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
        private readonly IReflectorUtil reflectorUtil;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoader"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="reflectorUtil"></param>
        public ModuleLoader(ILogger<ModuleLoader> logger, IReflectorUtil reflectorUtil)
        {
            this.logger = logger;
            this.reflectorUtil = reflectorUtil;
        }

        /// <summary>
        /// Loads the module.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly.</param>
        public void LoadModule(IServiceCollection services, Assembly assembly)
        {
            try
            {
                var modules = reflectorUtil.GetObjectOfBase<IModule>(assembly);
                if (!modules.Any())
                    return;
                foreach (var item in modules)
                {
                    item.ConfigModule(services);
                }
                foreach (var item in modules)
                {
                    item.Initialization(AccessServiceProvider.Service ?? services.BuildServiceProvider());
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