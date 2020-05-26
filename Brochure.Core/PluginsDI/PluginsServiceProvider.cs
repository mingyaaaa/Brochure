using System;
using System.Collections.Concurrent;
using System.Linq;
using AspectCore.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.Extenstions;

namespace Brochure.Core.PluginsDI
{
    public class PluginsServiceProvider : IPluginServiceProvider
    {
        private readonly IPluginManagers managers;
        private readonly ConcurrentDictionary<string, IServiceResolver> pluginServiceDic;
        private readonly IServiceResolver originalProvider;

        public PluginsServiceProvider (IPluginManagers managers, IServiceResolver serviceProvider)
        {
            this.managers = managers;
            pluginServiceDic = new ConcurrentDictionary<string, IServiceResolver> ();
            originalProvider = serviceProvider;
            PopuPlugin ();
        }
        public void Dispose ()
        {
            originalProvider.Dispose ();
            foreach (var item in pluginServiceDic.Values.ToList ())
            {
                item.Dispose ();
            }
        }

        public object GetService (Type serviceType)
        {
            var plugins = this.managers.GetPlugins ();
            if (plugins.Count != pluginServiceDic.Count)
            {
                PopuPlugin ();
            }
            var obj = originalProvider.GetService (serviceType);
            if (obj == null)
            {
                foreach (var item in pluginServiceDic.Values.ToArray ())
                {
                    obj = item.GetService (serviceType);
                    if (obj != null)
                        return obj;
                }
            }
            return obj;
        }

        public void PopuPlugin ()
        {
            var plugins = this.managers.GetPlugins ().OfType<Plugins> ().ToList ();
            foreach (var item in plugins)
            {
                var pluginsServiceCollection = item.Context.GetPluginContext<PluginServiceCollectionContext> ();
                pluginServiceDic.TryAdd (item.Key.ToString (), pluginsServiceCollection.BuildPlugnScopeProvider ());
            }
        }
    }
}