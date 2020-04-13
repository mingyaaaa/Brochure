using System.Reflection;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Module
{
    public class ModuleLoader : IModuleLoader
    {
        public void LoadModule (IServiceCollection services, Assembly assembly)
        {
            var refUtils = services.GetServiceInstance<IReflectorUtil> ();
            var modules = refUtils.GetObjectOfAbsoluteBase<IModule> (assembly);
            foreach (var item in modules)
            {
                item.Initialization (services);
            }
        }
    }
}