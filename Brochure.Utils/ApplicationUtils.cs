using System;
using System.IO;
namespace Brochure.Utils
{
    public static class ApplicationUtils
    {
        public static string GetPluginsPath ()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine (basePath, "Plugins");
        }
    }
}