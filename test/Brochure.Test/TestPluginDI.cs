﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Brochure.Test
{
    /// <summary>
    /// The service interface test.
    /// </summary>
    [TestClass]
    public class TestPluginDI
    {
        private IServiceCollection service;
        /// <summary>

        /// Initializes a new instance of the <see cref="TestPluginDI"/> class.
        /// </summary>
        public TestPluginDI()
        {
            service = new ServiceCollection();
            service.AddBrochureCore();
        }

        private IContainer CreateContainer(IServiceCollection serviceDescriptors)
        {
            var builder = new ContainerBuilder();
            builder.Populate(serviceDescriptors);
            return builder.Build();
        }

        private ILifetimeScope CreatePluginScope(IContainer container, Action<IServiceCollection> action)
        {
            var typeCache = container.Resolve<PluginServiceTypeCache>();
            var service = new ServiceCollection();
            var scope = container.BeginLifetimeScope(t =>
             {
                 action(service);
                 t.Populate(service);
             });
            typeCache.AddPluginServiceType("test", scope, service);
            return scope;
        }

        /// <summary>
        /// Tests the add single service.
        /// </summary>
        [TestMethod]
        public void TestAppSingleton_PluginSingleton()
        {
            service.AddSingleton<AppSingleton_P_Singleton>();
            var container = CreateContainer(service);
            var scope = CreatePluginScope(container, t =>
               {
                   t.AddSingleton<PluginA>();
               });
            var app = container.Resolve<AppSingleton_P_Singleton>();
            Assert.IsNotNull(app);
            Assert.IsNotNull(app.PluginA);
            Assert.IsTrue(app.PluginA.Value.TryGetTarget(out var plugina));

            var app2 = container.Resolve<AppSingleton_P_Singleton>();
            Assert.AreSame(app, app2);
            app2.PluginA.Value.TryGetTarget(out var plugina1);
            Assert.AreSame(plugina, plugina1);

            scope.Dispose();

            var app3 = container.Resolve<AppSingleton_P_Singleton>();
            app3.PluginA.Value.TryGetTarget(out var plugina2);
            Assert.IsNull(plugina2);
        }

        [TestMethod]
        public void TestAppScope_PluginSingle()
        {
            service.AddScoped<AppScope_PSingleton>();
            var container = CreateContainer(service);
            var scope = CreatePluginScope(container, t =>
            {
                t.AddSingleton<PluginA>();
            });
            var app = container.Resolve<AppScope_PSingleton>();
            using var appScope = container.BeginLifetimeScope();

            var app1 = appScope.Resolve<AppScope_PSingleton>();
            Assert.AreNotSame(app, app1);
            Assert.IsNotNull(app);
            Assert.IsNotNull(app.PluginA);
            Assert.IsTrue(app.PluginA.Value.TryGetTarget(out var plugina));

            var app2 = appScope.Resolve<AppScope_PSingleton>();
            Assert.AreNotSame(app, app2);
            app2.PluginA.Value.TryGetTarget(out var plugina1);
            Assert.AreSame(plugina, plugina1);
            Assert.AreSame(app1, app2);
            scope.Dispose();

            var app3 = appScope.Resolve<AppScope_PSingleton>();
            app3.PluginA.Value.TryGetTarget(out var plugina2);
            Assert.IsNull(plugina2);
        }

        #region PluginSingleton

        public class AppSingleton_P_Singleton
        {
            public IPluginSingleton<PluginA> PluginA { get; }

            public AppSingleton_P_Singleton(IPluginSingleton<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        public class AppScope_PSingleton
        {
            public IPluginSingleton<PluginA> PluginA { get; }

            public AppScope_PSingleton(IPluginSingleton<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        public class PluginA
        {
        }

        public class PluginB
        {
        }

        #endregion PluginSingleton

        #region PluginScope

        public class AppScope_PScope
        {
            public IPluginScope<PluginA> PluginA { get; }

            public AppScope_PScope(IPluginScope<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        #endregion PluginScope
    }
}