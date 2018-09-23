using Brochure.Core.Interfaces;
using System.IO;

namespace Brochure.Core.Server
{
    public class ServerBootstrap : IBootstrap
    {
        private IocProxy _proxy { get; set; }

        public ServerBootstrap(IocProxy proxy)
        {
            _proxy = proxy;
        }
        public void Start()
        {
            //查询所有的插件信息
            var pluginManager = _proxy.GetServices<IPluginManagers>();
            var context = _proxy.GetServices<IContext>();
            var pluginPathDir = Core.PluginManagers.GetPluginPath();
            var configFiles = Directory.GetFiles(pluginPathDir, "plugin.json");
            foreach (var configFile in configFiles)
            {
                //判断权限  如果权限不在 就不加载对应的插件
                var configPluin = JsonUtil.ReadJson(configFile)?.As<IPlugins>();
                if (configPluin == null || !context.PluginAuth.HasAuth(configPluin.Key.ToString()))
                    continue;
                var plugin = pluginManager.GetPlugin(configFile);
                if (plugin.Starting())
                    plugin.Start();
            }
        }

        public void Exit(IPlugins[] plugins)
        {

        }
    }
}
