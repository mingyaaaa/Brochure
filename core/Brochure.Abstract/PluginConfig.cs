using System;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    public class PluginConfig
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string AssemblyName { get; set; }
        public int Order { get; set; }

        public string PluginPath { get; set; }

        public List<Guid> DependencesKey { get; set; }
    }
}