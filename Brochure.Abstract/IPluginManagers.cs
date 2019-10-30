using System;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    public interface IPluginManagers
    {
        void Regist (IPlugins plugin);

        void Remove (IPlugins plugin);

        IPlugins GetPlugin (Guid key);

        List<IPlugins> GetPlugins ();
    }
}