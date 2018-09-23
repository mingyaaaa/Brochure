using System;
using System.Collections.Generic;
using System.Runtime.Loader;

namespace Brochure.Core
{
    public class PluginProxy : IPlugins
    {
        //此处存储的为PluginProxy
        private IPlugins _plugin;
        private AssemblyLoadContext _loadContext;
        public PluginProxy(IPlugins plugin, AssemblyLoadContext assemblyLoadContext)
        {
            _plugin = plugin;
            _loadContext = assemblyLoadContext;
        }
        public void Start()
        {
            _plugin.Start();
        }

        public void Exit()
        {
            _plugin.Exit();
        }

        public Guid Key => _plugin.Key;
        public string Name => _plugin.Name;
        public long Version => _plugin.Version;
        public string Author => _plugin.Author;
        public string AssemblyName => _plugin.AssemblyName;
        public List<Guid> DependencesKey => _plugin.DependencesKey;
        public bool Starting()
        {
            return _plugin.Starting();
        }

        public void Started()
        {
            _plugin.Started();
        }

        public bool Exiting()
        {
            return _plugin.Exiting();
        }

        public void Exited()
        {
            _plugin.Exited();
        }
    }
}
