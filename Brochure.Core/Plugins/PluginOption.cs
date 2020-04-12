using Brochure.Abstract;

namespace Brochure.Core.Models
{
    public class PluginOption : IPluginOption
    {

        public PluginOption (IPlugins plugins)
        {
            this.Plugin = plugins;
        }

        public IPlugins Plugin { get; }

    }
}