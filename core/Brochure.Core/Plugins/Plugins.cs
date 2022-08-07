using Brochure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Brochure.Core
{
    /// <summary>
    /// The plugins.
    /// </summary>
    public abstract class Plugins : IPlugins
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        public IPluginContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugins"/> class.
        /// </summary>
        public Plugins() : this(new PluginContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plugins"/> class.
        /// </summary>
        /// <param name="pluginContexts">The plugin contexts.</param>
        public Plugins(IPluginContext pluginContexts)
        {
            Order = int.MaxValue;
            Context = pluginContexts;
            AuthKey = new List<Guid>();
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the dependences key.
        /// </summary>
        public List<Guid> DependencesKey { get; set; }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string PluginDirectory { get; internal set; }

        /// <summary>
        /// Gets or sets the plugin configuration.
        /// </summary>
        public IConfiguration Configuration { get; internal set; }

        /// <summary>
        /// Gets or sets the auth key.
        /// </summary>
        public List<Guid> AuthKey { get; set; }

        public IPluginScope Scope { get; internal set; }

        /// <summary>
        /// 配置服务
        /// </summary>
        public virtual Task StartAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Exits the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public virtual Task ExitAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Startings the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public virtual Task<bool> StartingAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Exitings the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public virtual Task<bool> ExitingAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        public virtual void ConfigureService(IServiceCollection services)
        {
        }

        /// <summary>
        /// Configs the application.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void ConfigApplication(IApplicationBuilder serviceProvider)
        {
        }
    }
}