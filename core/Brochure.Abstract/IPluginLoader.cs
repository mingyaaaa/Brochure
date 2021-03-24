using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginLoader
    {
        ValueTask LoadPlugin(IServiceProvider service);
        ValueTask<IPlugins> LoadPlugin(IServiceProvider service, string pluginConfigPath);
        ValueTask<IPlugins> LoadPlugin(IServiceCollection service, string pluginConfigPath);

        ValueTask<bool> UnLoad(Guid key);
    }
}