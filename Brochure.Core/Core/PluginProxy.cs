using System;

namespace Brochure.Core
{
    public class PluginProxy
    {
        private IPlugins _plugin;
        public Func<Guid, bool> StartingHandle;
        public Func<Guid, bool> EndingHandle;
        public PluginProxy(IPlugins plugin)
        {
            _plugin = plugin;
        }

        public PluginProxy(string configPath)
        {
            var record = JsonUtil.ReadJson(configPath);
            _plugin = record.As<IPlugins>();
        }
        public void Start()
        {
            if (StartingHandle != null && StartingHandle.Invoke(_plugin.Key))
                _plugin.Start();
        }

        public void Exit()
        {
            if (EndingHandle != null && EndingHandle.Invoke(_plugin.Key))
                _plugin.Start();
        }
    }
}
