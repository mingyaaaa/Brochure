using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core
{
    public class PluginsLoadContext : AssemblyLoadContext
    {
        public IServiceCollection ServiceContrainer;
        public PluginsLoadContext (IServiceCollection services)
        {
            this.ServiceContrainer = services;
        }
        public virtual Assembly LoadAssembly (string path)
        {
            return LoadFromAssemblyPath (path);
        }
    }

}