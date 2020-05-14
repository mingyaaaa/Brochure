using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    public class CostumeServiceProvider : IServiceProvider
    {
        private readonly IPluginManagers managers;
        private readonly ConcurrentDictionary<string, IServiceProvider> pluginServiceDic;
        private IServiceProvider mProvider;
        public CostumeServiceProvider (IPluginManagers managers)
        {
            this.managers = managers;
            pluginServiceDic = new ConcurrentDictionary<string, IServiceProvider> ();
            PopuPlugin ();
        }
        public object GetService (Type serviceType)
        {
            var plugins = this.managers.GetPlugins ();
            if (plugins.Count != pluginServiceDic.Count)
            {
                PopuPlugin ();
            }
            foreach (var item in pluginServiceDic.Values.ToArray ())
            {
                var a = item.GetService (serviceType);
                if (a != null)
                    return a;
            }
            return mProvider.GetService (serviceType);
        }

        public void PopuPlugin ()
        {
            var plugins = this.managers.GetPlugins ().OfType<Plugins> ().ToList ();
            foreach (var item in plugins)
            {
                if (mProvider == null)
                    mProvider = item.Context.MainService;
                pluginServiceDic.TryAdd (item.Key.ToString (), item.Context.BuildServiceProvider ());
            }
        }
    }

    public static class Extend
    {
        public static IServiceProvider BuildCostumeServiceCollection (this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider ();
            var managers = provider.GetService<IPluginManagers> ();
            return new CostumeServiceProvider (managers);
        }
    }

    public class CServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        public IServiceCollection CreateBuilder (IServiceCollection services)
        {
            return services;
        }

        public IServiceProvider CreateServiceProvider (IServiceCollection containerBuilder)
        {
            return containerBuilder.BuildCostumeServiceCollection ();
        }
    }

    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestBuildService ()
        {
            var service = new ServiceCollection ();
            var managers = new PluginManagers ();
            service.AddSingleton<IPluginManagers> (managers);
            var mProvider = service.BuildServiceProvider ();
            var p1 = new P1 (mProvider);
            managers.Regist (p1);
            p1.Context.AddSingleton<ITest, ImpTest> ();

            var mm = service.BuildCostumeServiceCollection ();
            var a = mm.GetService<ITest> ();
            Assert.IsNotNull (a);
        }
    }

    public interface ITest
    {

    }

    public class ImpTest : ITest
    {

    }

    public class P1 : Plugins
    {
        public P1 (IServiceProvider service) : base (service) { }
    }
}