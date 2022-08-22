using Brochure.ORM.Database;
using Brochure.User.Entrities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.User.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Inits the db.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        public static void InitDb(this IApplicationBuilder applicationBuilder)
        {
            var log = applicationBuilder.ApplicationServices.GetRequiredService<ILogger<UserPlugin>>();
            try
            {
                using var scope = applicationBuilder.ApplicationServices.CreateScope();
                var dbTable = scope.ServiceProvider.GetService<DbTable>();
                dbTable.CreateTableAsync<UserEntrity>().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
            }
        }
    }
}