using System.Reflection;
using System.Runtime.Loader;

namespace Brochure.Core
{
    public class LoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
