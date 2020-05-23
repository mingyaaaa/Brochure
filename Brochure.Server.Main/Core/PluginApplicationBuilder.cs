using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class PluginApplicationBuilder : IApplicationBuilder
    {

        private readonly IApplicationBuilder applicationBuilder;
        private readonly IMiddleManager middleManager;
        private readonly IList<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>> ();

        public PluginApplicationBuilder (IApplicationBuilder builder)
        {
            applicationBuilder = builder;
            middleManager = ApplicationServices.GetService<IMiddleManager> ();
        }
        public IServiceProvider ApplicationServices
        {
            get => applicationBuilder.ApplicationServices;
            set => applicationBuilder.ApplicationServices = value;
        }

        public IDictionary<string, object> Properties => applicationBuilder.Properties;

        public IFeatureCollection ServerFeatures => applicationBuilder.ServerFeatures;

        public RequestDelegate Build ()
        {
            return middleManager.UseMiddle ();
        }

        public IApplicationBuilder New ()
        {
            return new PluginApplicationBuilder (applicationBuilder.New ());
        }

        public IApplicationBuilder Use (Func<RequestDelegate, RequestDelegate> middleware)
        {
            middleManager.AddMiddle (middleware);
            return this;
        }
    }
}