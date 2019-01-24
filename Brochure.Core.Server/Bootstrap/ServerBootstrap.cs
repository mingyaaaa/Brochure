using AspectCore.Injector;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Brochure.Core.Server
{
    public class ServerBootstrap : IBootstrap
    {
        [FromContainer]
        private IMvcCoreBuilder mvcCoreBuilder { get; set; }

        public ServerBootstrap()
        {
        }

        public void Start()
        {
            //查询所有的插件信息
            var pluginManager = new PluginManagers(mvcCoreBuilder);
            var context = new Context();
            var pluginPathDir = PluginManagers.GetPluginPath();
            var configFiles = Directory.GetFiles(pluginPathDir, "plugin.json");
            foreach (var configFile in configFiles)
            {
                //判断权限  如果权限不在 就不加载对应的插件
                var configPluin = JsonUtil.ReadJsonFile(configFile)?.As<IPlugins>();
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