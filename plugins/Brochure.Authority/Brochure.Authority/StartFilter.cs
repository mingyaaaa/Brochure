using System;
using System.Threading.Tasks;
using Brochure.Authority.Entities;
using Brochure.Core.Server;
using Brochure.ORM.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Authority
{
    /// <summary>
    /// The start config.
    /// </summary>
    public class StartConfig : IStarupConfigure
    {
        /// <summary>
        /// Configures the.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <param name="app">The app.</param>
        public async void Configure(Guid guid, IApplicationBuilder app)
        {
            // app.UseIdentityServer ();
            app.IntertMiddle("author-authentication", guid, 100, () => app.UseAuthentication());

            app.IntertMiddle("author-authorization", guid, 101, () => app.UseAuthorization());

            await InitDb(app);
        }

        /// <summary>
        /// Inits the db.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>A Task.</returns>
        private async Task InitDb(IApplicationBuilder applicationBuilder)
        {
            var log = applicationBuilder.ApplicationServices.GetRequiredService<ILogger<StartConfig>>();
            try
            {
                using var scope = applicationBuilder.ApplicationServices.CreateScope();
                var dbTable = scope.ServiceProvider.GetService<DbTable>();
                await dbTable.CreateTableAsync<UserRoleEntity>();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
            }
        }
    }
}