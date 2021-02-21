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
    public class PluginApplicationBuilder : IApplicationBuilder
    {

        private readonly IApplicationBuilder applicationBuilder;
        private static readonly object lockObject = new object();
        private readonly IMiddleManager manager;
        public PluginApplicationBuilder(IApplicationBuilder builder)
        {
            applicationBuilder = builder;
            manager = builder.ApplicationServices.GetService<IMiddleManager>();
        }
        public IServiceProvider ApplicationServices
        {
            get => applicationBuilder.ApplicationServices;
            set => applicationBuilder.ApplicationServices = value;
        }

        public IDictionary<string, object> Properties => applicationBuilder.Properties;

        public IFeatureCollection ServerFeatures => applicationBuilder.ServerFeatures;

        public RequestDelegate Build()
        {
            return UseMiddle();
        }

        public IApplicationBuilder New()
        {
            return new PluginApplicationBuilder(applicationBuilder.New());
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            manager.MiddleAction?.Invoke(middleware);
            return this;
        }
        public RequestDelegate UseMiddle()
        {
            return StartPipe();
        }

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
                app.Invoke(t);
                return Task.CompletedTask;
            };
        }

    }
}