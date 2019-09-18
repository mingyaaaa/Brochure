using System;
using System.IO;
namespace Brochure.Utils
{
    public class ApplicationUtils
    {
        public static string GetPluginsPath ()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine (basePath, "Plugins");
        }
    }
}