using System;
using Brochure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The b application.
    /// </summary>
    public class BApplication : IBApplication
    {
        /// <summary>
        /// Gets or sets the application part manager.
        /// </summary>
        public ApplicationPartManager ApplicationPartManager { get; set; }

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets or sets the builder.
        /// </summary>
        public IApplicationBuilder Builder { get; set; }
    }
}