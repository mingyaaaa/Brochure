using System;
using System.Collections.Generic;
using Brochure.Abstract;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class PluginMiddleContext : IApplicationBuilder, IPluginContextDescript
    {
        public PluginMiddleContext (IServiceProvider provider, Guid guid)
        {
            middle = provider.GetService<IMiddleManager> ();
            this.guid = guid;
            this.provider = provider;
        }
        private readonly IApplicationBuilder builder;
        private readonly Guid guid;
        private readonly IMiddleManager middle;
        private readonly IServiceProvider provider;

        public IServiceProvider ApplicationServices
        {
            get => provider;
            set =>
                throw new NotSupportedException ();
        }

        public IDictionary<string, object> Properties =>
            throw new NotSupportedException ();

        public IFeatureCollection ServerFeatures =>
            throw new NotSupportedException ();

        /// <summary>
        /// 当前类支持该方法
        /// </summary>
        /// <returns></returns> 
        public RequestDelegate Build ()
        {
            throw new NotSupportedException ("当前类不支持");
        }

        public IApplicationBuilder New ()
        {
            return new PluginMiddleContext (provider, guid);
        }

        public IApplicationBuilder Use (Func<RequestDelegate, RequestDelegate> middleware)
        {
            middle.AddMiddle (guid, middleware);
            return this;
        }

    }

}