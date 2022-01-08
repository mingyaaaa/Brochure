using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    /// <summary>
    /// The module loader.
    /// </summary>
    public interface IModuleLoader
    {
        /// <summary>
        /// Loads the module.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly.</param>
        void LoadModule(IServiceCollection services, Assembly assembly);
    }
}