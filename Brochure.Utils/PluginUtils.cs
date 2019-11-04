using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Brochure.Abstract;

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