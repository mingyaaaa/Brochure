using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    public interface IPluginServiceProvider : IServiceProvider, IDisposable { }
    public class PluginsServiceProvider : IPluginServiceProvider
    {
        private readonly IPluginManagers managers;
        private readonly ConcurrentDictionary<string, IServiceResolver> pluginServiceDic;
        private IServiceResolver originalProvider;

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
            var plugins = this.managers.GetPlugins ().OfType<P> ().ToList ();
            foreach (var item in plugins)
            {
                pluginServiceDic.TryAdd (item.Key.ToString (), item.Context.BuildPlugnScopeProvider ());
            }
        }
    }

    public static class Extend
    {
        public static IServiceProvider BuildCostumeServiceCollection (this IServiceCollection services)
        {
            var provider = services.BuildPlugnScopeProvider ();
            var managers = provider.GetService<IPluginManagers> ();
            return new PluginsServiceProvider (managers, provider);
        }

        public static IServiceResolver BuildPlugnScopeProvider (this IServiceCollection services)
        {
            var provider = services.BuildServiceContextProvider (t =>
            {
                var serviceDefinition = t.FirstOrDefault (t => t.ServiceType == typeof (IServiceScopeFactory));
                t.Remove (serviceDefinition);
                t.AddType<IServiceScopeFactory, PluginServiceScopeFactory> (Lifetime.Scoped);
                t.AddType<IPluginServiceProvider, PluginsServiceProvider> (Lifetime.Scoped);
            });
            return provider as IServiceResolver;
        }

    }

    public class ServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
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

    public class PluginServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceResolver serviceProvider;

        public PluginServiceScopeFactory (IServiceResolver serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public IServiceScope CreateScope ()
        {
            return new PluginServiceScope (serviceProvider.CreateScope ());
        }
    }

    public class PluginServiceScope : IServiceScope
    {
        private readonly IServiceResolver serviceProvider;

        public PluginServiceScope (IServiceResolver serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public IServiceProvider ServiceProvider => serviceProvider.GetService<IPluginServiceProvider> () as PluginsServiceProvider;

        public void Dispose ()
        {
            serviceProvider.Dispose ();
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
            var mProvider = service.BuildCostumeServiceCollection ();
            var p1 = new P1 (mProvider);
            managers.Regist (p1);
            p1.Context.AddSingleton<ITest, ImpTest> ();
            var a = mProvider.GetService<ITest> ();
            Assert.IsNotNull (a);
        }

        [TestMethod]
        public void TestServiceScope ()
        {
            var service = new ServiceCollection ();
            var managers = new PluginManagers ();
            service.AddSingleton<IPluginManagers> (managers);
            service.AddScoped<ITest1, ImpTest1> ();
            service.AddSingleton<ITest2, ImpTest2> ();
            var mProvider = service.BuildCostumeServiceCollection ();
            var p1 = new P1 (mProvider);
            managers.Regist (p1);
            p1.Context.AddScoped<ITest, ImpTest> ();
            var test2 = mProvider.GetService<ITest2> ();
            using (var scope = mProvider.CreateScope ())
            {
                var a1 = scope.ServiceProvider.GetService<ITest1> ();
                Assert.IsNotNull (a1);
                var a = scope.ServiceProvider.GetService<ITest> ();
                Assert.IsNotNull (a);
                var a2 = scope.ServiceProvider.GetService<ITest2> ();
                Assert.IsNotNull (a2);
                Assert.AreSame (a2, test2);
            }
        }

        [TestMethod]
        public void TestServiceTransient ()
        {
            var service = new ServiceCollection ();
            var managers = new PluginManagers ();
            service.AddSingleton<IPluginManagers> (managers);
            service.AddTransient<ITest1, ImpTest1> ();
            var mProvider = service.BuildCostumeServiceCollection ();
            var p1 = new P1 (mProvider);
            managers.Regist (p1);
            p1.Context.AddScoped<ITest, ImpTest> ();

            var a1 = mProvider.GetService<ITest1> ();
            var a2 = mProvider.GetService<ITest1> ();
            Assert.AreNotSame (a1, a2);

        }
    }

    public interface ITest : IDisposable
    {

    }

    public class ImpTest : ITest
    {
        public void Dispose ()
        {
            Trace.TraceInformation ("ImpTest");
        }
    }
    public interface ITest1 : IDisposable
    {

    }

    public class ImpTest1 : ITest1
    {
        public void Dispose ()
        {
            Trace.TraceInformation ("ImpTest1");
        }
    }
    public interface ITest2 : IDisposable
    {

    }

    public class ImpTest2 : ITest2
    {
        public void Dispose ()
        {
            Trace.TraceInformation ("ImpTest2");
        }
    }
    public class P : IPlugins
    {
        public PContext Context { get; }

        public Guid Key => Guid.NewGuid ();

        public string Name =>
            throw new NotImplementedException ();

        public string Version =>
            throw new NotImplementedException ();

        public string Author =>
            throw new NotImplementedException ();

        public string AssemblyName =>
            throw new NotImplementedException ();

        public Assembly Assembly =>
            throw new NotImplementedException ();

        public int Order =>
            throw new NotImplementedException ();

        public List<Guid> DependencesKey =>
            throw new NotImplementedException ();

        public P (IServiceProvider service)
        {
            Context = new PContext (service);
        }

        public Task StartAsync ()
        {
            throw new NotImplementedException ();
        }

        public Task ExitAsync ()
        {
            throw new NotImplementedException ();
        }

        public Task<bool> StartingAsync (out string errorMsg)
        {
            throw new NotImplementedException ();
        }

        public Task<bool> ExitingAsync (out string errorMsg)
        {
            throw new NotImplementedException ();
        }
    }
    public class PContext : ServiceCollection
    {
        public PContext (IServiceProvider services)
        {
            // foreach (var item in services)
            // {
            //     this.Append (item);
            // }
            // MainService = services;
        }
    }
    public class P1 : P
    {
        public P1 (IServiceProvider service) : base (service) { }
    }

    public class P2 : P
    {
        public P2 (IServiceProvider service) : base (service) { }
    }
}