using System.IO;

namespace Brochure.Core.Server
{
    public class ServerBootstrap : IBootstrap
    {
        private IocProxy _proxy;
        public ServerBootstrap(IocProxy proxy)
        {
            _proxy = proxy;
        }
        public void Start()
        {
            _proxy.AddSingleton<IPluginManagers, PluginManagers>();
            //查询所有的插件信息
            var pluginManager = _proxy.GetServices<IPluginManagers>();
            var pluginPathDir = PluginManagers.GetPluginPath();
            var configFiles = Directory.GetFiles(pluginPathDir, "plugin.json");
            PluginProxy proxyPlugin = null;
            foreach (var configFile in configFiles)
            {
                proxyPlugin = new PluginProxy(configFile);
                // pluginManager.LoadPlugin();
            }
        }

        public void Exit(IPlugins[] plugins)
        {
        }
    }
}
