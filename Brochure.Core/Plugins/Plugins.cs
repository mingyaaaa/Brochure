using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public abstract class Plugins : IPlugins
    {
        public IPluginContext Context { get; }
        public Plugins (IServiceProvider service)
        {
            Order = int.MaxValue;
            Context = service.GetService<IPluginContext> ();
        }

        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string AssemblyName { get; set; }
        public List<Guid> DependencesKey { get; set; }
        public Assembly Assembly { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// 配置服务
        /// </summary>
        public virtual Task StartAsync ()
        {
            return Task.CompletedTask;
        }

        public virtual Task ExitAsync ()
        {
            return Task.CompletedTask;
        }

        public virtual Task<bool> StartingAsync (out string errorMsg)
        {
            errorMsg = string.Empty;
            return Task.FromResult (true);
        }

        public virtual Task<bool> ExitingAsync (out string errorMsg)
        {
            errorMsg = string.Empty;
            return Task.FromResult (true);
        }
    }
}