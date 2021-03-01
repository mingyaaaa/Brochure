using System;
using System.Collections.Generic;
using Brochure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core.Server
{
    /// <summary>
    /// The plugin middle context.
    /// </summary>
    public class PluginMiddleContext : IApplicationBuilder, IPluginContextDescript
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMiddleContext"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="guid">The guid.</param>
        public PluginMiddleContext(IServiceProvider provider, Guid guid)
        {
            _middle = provider.GetService<IMiddleManager>();
            this._guid = guid;
            this._provider = provider;
        }
        private readonly Guid _guid;
        private readonly IMiddleManager _middle;
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Gets or sets the application services.
        /// </summary>
        public IServiceProvider ApplicationServices
        {
            get => _provider;
            set =>
                throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public IDictionary<string, object> Properties =>
            throw new NotSupportedException();

        /// <summary>
        /// Gets the server features.
        /// </summary>
        public IFeatureCollection ServerFeatures =>
            throw new NotSupportedException();

        /// <summary>
        /// 当前类支持该方法
        /// </summary>
        /// <returns></returns> 
        public RequestDelegate Build()
        {
            throw new NotSupportedException("当前类不支持");
        }

        /// <summary>
        /// News the.
        /// </summary>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder New()
        {
            return new PluginMiddleContext(_provider, _guid);
        }

        /// <summary>
        /// Uses the.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            throw new NotSupportedException("当前类不支持");
        }

    }

}