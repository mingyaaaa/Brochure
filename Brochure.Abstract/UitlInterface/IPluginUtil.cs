using System.Collections.Generic;
using Brochure.Abstract;

namespace Brochure.Utils
{
    public interface IPluginUtil
    {
        string GetBasePluginsPath ();

        long GetPluginVersion (string version);
    }
}