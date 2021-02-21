using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginLoader
    {
        ValueTask<IPlugins> LoadPlugin(IServiceProvider service, string pluginConfigPath);
        ValueTask<IPlugins> LoadPlugin(IServiceCollection service, string pluginConfigPath);

        ValueTask<bool> UnLoad(Guid key);
    }
}