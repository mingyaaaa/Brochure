using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public interface IPlugins
    {
        Guid Key { get; }
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string AssemblyName { get; }
        Assembly Assembly { get; }
        int Order { get; }
        List<Guid> DependencesKey { get; }
        Task StartAsync();
        Task ExitAsync();
        Task<bool> StartingAsync(out string errorMsg);
        Task<bool> ExitingAsync(out string errorMsg);
    }

}