using Brochure.Core.Interfaces;
using System;

namespace Brochure.Core.Core
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
