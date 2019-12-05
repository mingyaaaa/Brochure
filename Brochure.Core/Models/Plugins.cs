using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public class Plugins : IPlugins
    {
        private readonly AssemblyLoadContext assemblyContext;
        protected IServiceCollection serviceDescriptors;

        public Plugins (AssemblyLoadContext assemblyContext)
        {
            this.assemblyContext = assemblyContext;
        }

        public IServiceProvider ServiceProvider { get; private set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public long Version { get; set; }
        public string Author { get; set; }
        public string AssemblyName { get; set; }
        public List<Guid> DependencesKey { get; set; }
        public Assembly Assembly { get; set; }

        /// <summary>
        /// 配置服务
        /// </summary>

        public virtual Task StartAsync ()
        {
            return Task.CompletedTask;
        }

        public virtual Task ExitAsync ()
        {
            this.assemblyContext.Unload ();
            return Task.CompletedTask;
        }

        public virtual Task<bool> StartingAsync ()
        {
            return Task.FromResult (true);
        }

        public virtual Task<bool> ExitingAsync ()
        {
            return Task.FromResult (true);
        }

        public AssemblyLoadContext GetAssemblyLoadContext ()
        {
            return assemblyContext;
        }
    }
}