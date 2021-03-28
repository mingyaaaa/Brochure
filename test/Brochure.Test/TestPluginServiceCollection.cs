using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Module;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{

    [TestClass]
    public class TestPluginServiceCollection : BaseTest
    {

        public TestPluginServiceCollection()
        {

            Service.TryAddSingleton<IPluginManagers>(new PluginManagers());
            Service.AddSingleton<IModuleLoader, ModuleLoader>();
            Service.AddSingleton<IPluginLoadAction, DefaultLoadAction>();
            Service.AddSingleton<IPluginUnLoadAction, DefaultUnLoadAction>();
            Service.AddSingleton<IPluginContext, PluginContext>();
            Service.AddSingleton<IPluginLoader, PluginLoader>();
            base.InitBaseService();
            var objectFactoryMock = GetMockService<IObjectFactory>();
            objectFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).Returns(new ConcurrentDictionary<Guid, IPlugins>());

        }

        [TestMethod]
        public void TestBuildService()
        {
            var mProvider = Service.BuildPluginServiceProvider();
            var managers = mProvider.GetService<IPluginManagers>();
            var p1 = new P1(mProvider);
            managers.Regist(p1);
            var p1Context = p1.Context;
            p1Context.Services.AddSingleton<ITestInterceptor, ImpTestInterceptor>();
            var a = mProvider.GetService<ITestInterceptor>();
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void TestServiceScope()
        {
            Service.AddScoped<ITest1, ImpTest1>();
            Service.AddSingleton<ITest2, ImpTest2>();
            var mProvider = Service.BuildPluginServiceProvider();
            var managers = mProvider.GetService<IPluginManagers>();
            var p1 = new P1(mProvider);
            managers.Regist(p1);
            var p1Context = p1.Context;
            p1Context.Services.AddScoped<ITestInterceptor, ImpTestInterceptor>();
            var test2 = mProvider.GetService<ITest2>();
            using (var scope = mProvider.CreateScope())
            {
                var a1 = scope.ServiceProvider.GetService<ITest1>();
                Assert.IsNotNull(a1);
                var a = scope.ServiceProvider.GetService<ITestInterceptor>();
                Assert.IsNotNull(a);
                var a2 = scope.ServiceProvider.GetService<ITest2>();
                Assert.IsNotNull(a2);
                Assert.AreSame(a2, test2);
            }
        }

        [TestMethod]
        public void TestServiceTransient()
        {
            Service.AddTransient<ITest1, ImpTest1>();
            var mProvider = Service.BuildPluginServiceProvider();
            var managers = mProvider.GetService<IPluginManagers>();
            var p1 = new P1(mProvider);
            managers.Regist(p1);
            var p1Context = p1.Context;
            p1Context.Services.AddScoped<ITestInterceptor, ImpTestInterceptor>();
            var a1 = mProvider.GetService<ITest1>();
            var a2 = mProvider.GetService<ITest1>();
            Assert.AreNotSame(a1, a2);

        }

        [TestMethod]
        public void TestSingletonSame()
        {
            Service.AddSingleton<ITest1, ImpTest1>();
            var provider = Service.BuildPluginServiceProvider();
            var managers = provider.GetService<IPluginManagers>();
            var test1 = provider.GetService<ITest1>();
            var p1 = new P1(provider);
            managers.Regist(p1);
            var context = p1.Context;
            context.Services.AddSingleton<ITest2, ImpTest2>();
            var test2 = provider.GetService<ITest1>();
            Assert.AreSame(test1, test2);
        }

        [TestMethod]
        public void TestGerniSame()
        {
            Service.AddOptions();
            Service.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, ConfigureRouteOptions>(
                _ => new ConfigureRouteOptions()));
            var oprovider = Service.BuildServiceContextProvider();
            var managers = oprovider.GetService<IPluginManagers>();
            var options = oprovider.GetService<IOptions<RouteOptions>>();
            Assert.IsNotNull(options.Value);
            var provider = Service.BuildPluginServiceProvider();
            var test1 = provider.GetService<IOptions<RouteOptions>>();
            Assert.IsNotNull(test1.Value);
            var p1 = new P1(provider);
            managers.Regist(p1);
            var test2 = provider.GetService<IOptions<RouteOptions>>();
        }

        [TestMethod]
        public void TestCollection()
        {

            Service.AddSingleton<ITest1, ImpTest1>();
            var provider = Service.BuildPluginServiceProvider();
            var managers = provider.GetService<IPluginManagers>();
            var collection = provider.GetService<IEnumerable<ITest1>>();
            Assert.IsNotNull(collection);
            Assert.AreEqual(1, collection.Count());
            var p1 = new P1(provider);
            managers.Regist(p1);
            var context = p1.Context;
            context.Services.AddSingleton<ITest1, ImpTest11>();
            collection = provider.GetService<IEnumerable<ITest1>>();
            Assert.AreEqual(2, collection.Count());
        }

        [TestMethod("保证IPluginManager在所有容器中的实例一致")]
        public async Task TestBuilderPluginService()
        {
            var provider = Service.BuildPluginServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var p1 = new P1(provider);
            manager.Regist(p1);

            var provider1 = Service.BuildPluginServiceProvider();
            var manager1 = provider1.GetService<IPluginManagers>();
            Assert.AreSame(manager, manager1);
        }

        [TestMethod("测试插件中使用主程序的依赖服务")]
        public void Test1()
        {
            Service.AddSingleton<ImpTest2>();
            var provider = Service.BuildPluginServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var p1 = new P1(provider);
            var pluginService = p1.Context;
            pluginService.Services.AddSingleton<ImpTest3>();
            manager.Regist(p1);
            manager = provider.GetService<IPluginManagers>();
        }

    }

    public class TestOption
    {
        public int P1 { get; set; } = 1;
        public int P2 { get; set; } = 1;
    }


    public interface ITestInterceptor : IDisposable
    {

    }

    public class ImpTestInterceptor : ITestInterceptor
    {
        public void Dispose()
        {
            Trace.TraceInformation("ImpTest");
        }
    }
    public interface ITest1 : IDisposable
    {

    }

    public class ImpTest1 : ITest1
    {
        public void Dispose()
        {
            Trace.TraceInformation("ImpTest1");
        }
    }

    public class ImpTest11 : ITest1
    {
        public void Dispose()
        {

            Trace.TraceInformation("ImpTest11");
        }
    }

    public interface ITest2 : IDisposable
    {

    }

    public class ImpTest2 : ITest2
    {
        public void Dispose()
        {
            Trace.TraceInformation("ImpTest2");
        }
    }
    public class ImpTest3
    {
        public class Option
        {
        }
        public ImpTest3(ImpTest2 impTest2)
        {
        }
    }

    public class Option<T>
    {

    }

    public class P1 : Plugins
    {
        public P1(IServiceProvider service) : base(new PluginContext()) { }
    }

    public class P2 : Plugins
    {
        public P2(IServiceProvider service) : base(new PluginContext()) { }
    }


    public class ConfigureRouteOptions : IConfigureOptions<RouteOptions>
    {

        public ConfigureRouteOptions()
        {

        }

        public void Configure(RouteOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}