using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The i service collection extension.
    /// </summary>
    public static class IServiceCollectionExtension
    {

        /// <summary>
        /// Adds the my sql.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="action">The action.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddMySql(this IServiceCollection services, Action<MySqlOption> action = null)
        {
            services.AddVisit();
            services.TryAddScoped<DbDatabase, MySqlDbDatabase>();
            services.TryAddScoped<DbData, MySqlDbData>();
            services.TryAddScoped<DbTable, MySqlDbTable>();
            services.TryAddScoped<DbIndex, MySqlDbIndex>();
            services.TryAddScoped<DbColumns, MySqlDbColumns>();
            services.TryAddScoped<IDbProvider, MySqlDbProvider>();
            services.TryAddSingleton<DbOption>(t =>
            {
                var option = new MySqlOption();
                action?.Invoke(option);
                return option;
            });
            services.TryAddScoped<DbSql, MySqlDbSql>();
            services.TryAddScoped<ITransactionManager, MySqlTransactionManager>();
            services.TryAddScoped<DbContext, MySqlDbContext>();
            return services;
        }
    }
}
