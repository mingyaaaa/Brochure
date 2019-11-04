using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Brochure.Utils;

namespace Brochure.Core
{
    public class Plugins : IPlugins
    {
        private readonly string pluginBathPath;

        private readonly AssemblyLoadContext assemblyContext;

        public Plugins (AssemblyLoadContext assemblyContext)
        {
            pluginBathPath = PluginUtils.GetBasePluginsPath ();
            this.assemblyContext = assemblyContext;
        }

        public Plugins () : this (new PluginsLoadContext ()) { }

        public virtual void Start () { }

        public virtual void Exit ()
        {
            this.assemblyContext.Unload ();
        }

        public Guid Key { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public string Author { get; set; }
        public string AssemblyName { get; set; }
        public List<Guid> DependencesKey { get; set; }

        public virtual bool Starting ()
        {
            return true;
        }

        public virtual void Started () { }

        public virtual bool Exiting ()
        {
            return true;
        }

        public virtual void Exited () { }

        public Assembly GetAssembly ()
        {
            var assemblyPath = Path.Combine (pluginBathPath, Name, AssemblyName);
            return assemblyContext.LoadFromAssemblyPath (assemblyPath);
        }

        public AssemblyLoadContext GetAssemblyLoadContext ()
        {
            return assemblyContext;
        }
    }
}