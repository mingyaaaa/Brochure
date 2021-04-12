using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// The application option.
    /// </summary>
    public class ApplicationOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOption"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public ApplicationOption(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

    }
}