using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core.Server
{
    /// <summary>
    /// The plugin application builder.
    /// </summary>
    public class PluginApplicationBuilder : IApplicationBuilder
    {

        private readonly IApplicationBuilder applicationBuilder;
        private static readonly object lockObject = new object();
        private readonly IMiddleManager manager;
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginApplicationBuilder"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public PluginApplicationBuilder(IApplicationBuilder builder)
        {
            applicationBuilder = builder;
            manager = builder.ApplicationServices.GetService<IMiddleManager>();
        }
        /// <summary>
        /// Gets or sets the application services.
        /// </summary>
        public IServiceProvider ApplicationServices
        {
            get => applicationBuilder.ApplicationServices;
            set => applicationBuilder.ApplicationServices = value;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public IDictionary<string, object> Properties => applicationBuilder.Properties;

        /// <summary>
        /// Gets the server features.
        /// </summary>
        public IFeatureCollection ServerFeatures => applicationBuilder.ServerFeatures;

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <returns>A RequestDelegate.</returns>
        public RequestDelegate Build()
        {
            return UseMiddle();
        }

        /// <summary>
        /// News the.
        /// </summary>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder New()
        {
            return new PluginApplicationBuilder(applicationBuilder.New());
        }

        /// <summary>
        /// Uses the.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            manager.MiddleAction?.Invoke(middleware);
            return this;
        }
        /// <summary>
        /// Uses the middle.
        /// </summary>
        /// <returns>A RequestDelegate.</returns>
        public RequestDelegate UseMiddle()
        {
            return StartPipe();
        }

        /// <summary>
        /// Default404S the end pipe.
        /// </summary>
        /// <returns>A RequestDelegate.</returns>
        private RequestDelegate Default404EndPipe()
        {
            return context =>
            {
                // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
                // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
                var endpoint = context.GetEndpoint();
                var endpointRequestDelegate = endpoint?.RequestDelegate;
                if (endpointRequestDelegate != null)
                {
                    var message =
                        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint.DisplayName}'. " +
                        $"Please register the EndpointMiddleware using '{nameof(IApplicationBuilder)}.UseEndpoints(...)' if using " +
                        $"routing.";
                    throw new InvalidOperationException(message);
                }
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// Starts the pipe.
        /// </summary>
        /// <returns>A RequestDelegate.</returns>
        private RequestDelegate StartPipe()
        {
            RequestDelegate app = null;
            int pluginCount = -1;
            return t =>
            {
                var pluginManagers = t.RequestServices.GetService<IPluginManagers>();
                var count = pluginManagers.GetPlugins().Count;
                lock (lockObject)
                {
                    if (pluginCount != count)
                    {
                        pluginCount = count;
                        app = Default404EndPipe();
                        var middleManager = t.RequestServices.GetService<IMiddleManager>();
                        var middleCollection = middleManager.GetMiddlesList();
                        var list = middleCollection.OrderBy(t => t.Order).Select(t => t.MiddleFactory).ToList();
                        list.Reverse();
                        foreach (var item in list)
                        {
                            app = item(app);
                        }
                    }
                }
                return app.Invoke(t);
            };
        }

    }
}