using System;

namespace Brochure.Abstract
{
    public interface IPluginLoadAction
    {
        void Invoke (IPlugins plugins);
    }

    public interface IPluginUnLoadAction
    {
        void Invoke (IPlugins plugins);
    }
}