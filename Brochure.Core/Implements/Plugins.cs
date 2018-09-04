using System;
using System.Collections.Generic;

namespace Brochure.Core
{
    public class Plugins : IPlugins
    {
        public Plugins()
        {
        }
        public void Start()
        {
        }

        public void Exit()
        {
        }

        public Guid Key { get; }
        public string Name { get; }
        public long Version { get; }
        public string Author { get; }
        public string AssemblyName { get; }
        public List<Guid> DependencesKey { get; }
    }
}
