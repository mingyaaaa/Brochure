using Brochure.Core.Interfaces;

namespace Brochure.Core.Server.Bootstrap
{
    public class ServerBootstrap : IBootstrap
    {
        //[FromContainer]
        //protected PluginManagers pluginManagers { get; set; }
        public void Start()
        {
            // AssemblyLoadContext.Default.LoadFromAssemblyPath();
        }

        public void Exit(IPlugins[] plugins)
        {
        }
    }
}
