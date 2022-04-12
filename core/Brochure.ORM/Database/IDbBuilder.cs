using Microsoft.Extensions.DependencyInjection;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db builder.
    /// </summary>
    public interface IDbBuilder
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        IServiceCollection Service { get; }
    }

    /// <summary>
    /// The db builder.
    /// </summary>
    public class DbBuilder : IDbBuilder
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public DbBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        public IServiceCollection Service => _services;
    }
}