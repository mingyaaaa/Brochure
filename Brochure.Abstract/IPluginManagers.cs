using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginManagers
    {
        void ResolverPlugins(IServiceCollection serviceDescriptors, Func<IPluginOption, bool> func);

        void Regist(IPlugins plugin);

        void UnLoad(IPlugins plugin);

        void Remove(IPlugins plugin);

        IPlugins GetPlugin(Guid key);

        List<IPlugins> GetPlugins();

        string GetBasePluginsPath();

        long GetPluginVersion(string version);

    }
}