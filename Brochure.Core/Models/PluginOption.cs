using System;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Models
{
    public class PluginOption
    {
        private readonly IPlugins plugins;

        public PluginOption(IPlugins plugins)
        {
            this.plugins = plugins;
        }

        public void AddStarupConfigureServices(IServiceCollection serviceDescriptors, IReflectorUtil reflectorUtil)
        {
            var configureServices = reflectorUtil.GetObjectByInterface<IStartupConfigureServices>(plugins.Assembly);
            foreach (var item in configureServices)
            {
                item.ConfigureService(serviceDescriptors);
            }
        }
        public Assembly GetAssembly()
        {
            return plugins.Assembly;
        }
    }
}
