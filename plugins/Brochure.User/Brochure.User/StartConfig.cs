using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Microsoft.Extensions.DependencyInjection;
using Brochure.ORM;
using Brochure.User.Entrities;

namespace Brochure.User
{
    /// <summary>
    /// The start config.
    /// </summary>
    public class StartConfig : IStarupConfigure
    {
        /// <summary>
        /// Configures the.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="builder">The builder.</param>
        public void Configure(Guid key, IApplicationBuilder builder)
        {
            InitDb(builder);
        }

        /// <summary>
        /// Inits the db.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        private async void InitDb(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DbContext>();
            var exist = dbContext.GetDbTable().IsExistTable(nameof(UserEntrity).ToLower());
            await dbContext.GetDbTable().CreateTableAsync<UserEntrity>();
        }
    }
}
