using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public interface IBApplication
    {

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the builder.
        /// </summary>
        IApplicationBuilder Builder { get; }
    }
}