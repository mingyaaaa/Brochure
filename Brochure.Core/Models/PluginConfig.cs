using System;
using System.Collections.Generic;

namespace Brochure.Core.Models
{
    public class PluginConfig
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public string Author { get; set; }
        public string AssemblyName { get; set; }
        public List<Guid> DependencesKey { get; set; }
    }
}