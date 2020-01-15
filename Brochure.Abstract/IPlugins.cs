using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public interface IPlugins
    {
        Task StartAsync();

        Task ExitAsync();
        Guid Key { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string AssemblyName { get; }
        Assembly Assembly { get; }
        int Order { get; }
        List<Guid> DependencesKey { get; }

        Task<bool> StartingAsync();
        Task<bool> ExitingAsync();
    }

}