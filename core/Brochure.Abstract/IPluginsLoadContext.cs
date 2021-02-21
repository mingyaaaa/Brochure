using System.Reflection;
namespace Brochure.Abstract
{
    public interface IPluginsLoadContext
    {
        Assembly LoadAssembly (AssemblyName assemblyName);

        void UnLoad ();
    }
}