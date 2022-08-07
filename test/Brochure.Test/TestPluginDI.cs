using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.AspectCore;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

        private IServiceResolver CreateContainer(IServiceCollection serviceDescriptors)
        {
            var builder = serviceDescriptors.ToServiceContext(null);
            return builder.Build();
        }

        private IPluginScope CreatePluginScope(IServiceResolver container, Action<IServiceCollection> action)
        {
            var pluginProvider = new AspectPluginScopeFactory(container);
            var service = new ServiceCollection();
            action?.Invoke(service);
            var scope = pluginProvider.CreateScope(service);
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
            using var appScope = container.CreateScope();

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

        /// <summary>
        /// The app singleton_ p_ singleton.
        /// </summary>
        public class AppSingleton_P_Singleton
        {
            /// <summary>
            /// Gets the plugin a.
            /// </summary>
            public ISingletonService<PluginA> PluginA { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="AppSingleton_P_Singleton"/> class.
            /// </summary>
            /// <param name="pluginSingleton">The plugin singleton.</param>
            public AppSingleton_P_Singleton(ISingletonService<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        /// <summary>
        /// The app scope_ p singleton.
        /// </summary>
        public class AppScope_PSingleton
        {
            /// <summary>
            /// Gets the plugin a.
            /// </summary>
            public ISingletonService<PluginA> PluginA { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="AppScope_PSingleton"/> class.
            /// </summary>
            /// <param name="pluginSingleton">The plugin singleton.</param>
            public AppScope_PSingleton(ISingletonService<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        /// <summary>
        /// The plugin a.
        /// </summary>
        public class PluginA
        {
        }

        /// <summary>
        /// The plugin b.
        /// </summary>
        public class PluginB
        {
        }

        #endregion PluginSingleton

        #region PluginScope

        /// <summary>
        /// The app scope_ p scope.
        /// </summary>
        public class AppScope_PScope
        {
            /// <summary>
            /// Gets the plugin a.
            /// </summary>
            public IScopeService<PluginA> PluginA { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="AppScope_PScope"/> class.
            /// </summary>
            /// <param name="pluginSingleton">The plugin singleton.</param>
            public AppScope_PScope(IScopeService<PluginA> pluginSingleton)
            {
                PluginA = pluginSingleton;
            }
        }

        #endregion PluginScope
    }
}