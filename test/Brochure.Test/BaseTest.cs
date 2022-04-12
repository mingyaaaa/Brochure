using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;

namespace Brochure.Test
{
    /// <summary>
    /// The base test.
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class.
        /// </summary>
        public BaseTest()
        {
            Service = new ServiceCollection();
            MockService = new Dictionary<Type, object>();
            Log = new Mock<ILogger<BaseTest>>();
            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
            Fixture.Customizations.Add(new TypeRelay(typeof(Plugins), typeof(TestPlugin)));
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        protected IServiceCollection Service { get; }

        protected IFixture Fixture;

        /// <summary>
        /// Gets the mock service.
        /// </summary>
        protected Dictionary<Type, object> MockService { get; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        protected Mock<ILogger<BaseTest>> Log { get; }

        /// <summary>
        /// Inits the base service.
        /// </summary>
        public void InitBaseService()
        {
            var json = new Mock<IJsonUtil>();
            Service.AddLogging();

            SetMockService(json);
            SetMockService(new Mock<ISystemUtil>());
            SetMockService(new Mock<ISysDirectory>());
            SetMockService(new Mock<IReflectorUtil>());
            SetMockService(new Mock<IObjectFactory>());
            SetMockService(new Mock<IPluginManagers>());
            SetMockService(new Mock<ILoggerFactory>());
            Log.Setup(t => t.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(),
               this, It.IsAny<Exception>(), It.IsAny<Func<BaseTest, Exception, string>>()
           ));
        }

        /// <summary>
        /// Sets the mock service.
        /// </summary>
        /// <param name="mockService">The mock service.</param>
        private void SetMockService<T>(IMock<T> mockService) where T : class
        {
            Service.TryAddSingleton<T>(mockService.Object);
            MockService.Add(typeof(T), mockService);
        }

        /// <summary>
        /// Gets the mock service.
        /// </summary>
        /// <returns>A Mock.</returns>
        public Mock<T> GetMockService<T>() where T : class
        {
            var type = typeof(T);
            if (!MockService.ContainsKey(type))
                throw new Exception("Mock服务没有注册");
            return (Mock<T>)MockService[type];
        }
    }

    /// <summary>
    /// The test plugin.
    /// </summary>
}