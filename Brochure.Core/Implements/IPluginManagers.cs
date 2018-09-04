using System;
using System.Collections.Generic;

namespace Brochure.Core
{
    public interface IPluginManagers
    {
        void Regist(IPlugins plugin);

        void Remove(Guid key);

        IPlugins GetPlugin(Guid key);

        List<IPlugins> GetPlugins();
    }
}
