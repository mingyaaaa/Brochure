using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin service provider.
    /// </summary>
    public interface IPluginServiceProvider : IServiceProvider, IAsyncDisposable, IDisposable
    {
        public bool IsDisposed { get; }

        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>An IPluginServiceProvider.</returns>
        IPluginServiceProvider CreateScope(IServiceCollection action);
    }

    public static class PluginServiceProviderExtensions
    {
        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <param name="pluginServiceProvider"></param>
        /// <param name="action"></param>
        /// <returns>An IPluginServiceProvider.</returns>
        public static IPluginServiceProvider CreateScope(this IPluginServiceProvider pluginServiceProvider, Action<IServiceCollection> action = null)
        {
            var a = new ServiceCollection();
            action?.Invoke(a);
            return pluginServiceProvider.CreateScope(a);
        }
    }
}