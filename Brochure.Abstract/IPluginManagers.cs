using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginManagers
    {
        Task ResolverPlugins(IServiceCollection serviceDescriptors, Func<IPluginOption, Task<bool>> func);

        void Regist(IPlugins plugin);

        void UnLoad(IPlugins plugin);

        void Remove(IPlugins plugin);

        IPlugins GetPlugin(Guid key);

        List<IPlugins> GetPlugins();

        string GetBasePluginsPath();

        long GetPluginVersion(string version);

    }
}