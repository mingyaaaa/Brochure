using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using Brochure.Abstract;
using Brochure.Core.Core;

namespace Brochure.Core
{
    public class Plugins : IPlugins
    {
        private readonly AssemblyLoadContext assemblyContext;

        public Plugins (AssemblyLoadContext assemblyContext)
        {
            this.assemblyContext = assemblyContext;
        }

        public Plugins () : this (new PluginsLoadContext ()) { }

        public virtual void Start () { }

        public virtual void Exit ()
        {
            this.assemblyContext.Unload ();
        }

        public Guid Key { get; }
        public string Name { get; }
        public long Version { get; }
        public string Author { get; }
        public string AssemblyName { get; }
        public List<Guid> DependencesKey { get; }

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
    }
}