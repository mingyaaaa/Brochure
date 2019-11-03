using System;
using System.IO;

namespace Brochure.Utils
{
    public static class PluginUtils
    {
        public static string GetBasePluginsPath ()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pluginPath = Path.Combine (basePath, "Plugins");
            return pluginPath;
        }
    }
}