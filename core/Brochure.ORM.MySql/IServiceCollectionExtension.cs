﻿using Brochure.ORM.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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
        /// <param name="builder"></param>
        /// <param name="action">The action.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddMySql(this IDbBuilder builder, Action<MySqlOption> action = null)
        {
            var services = builder.Service;
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
            services.TryAddScoped<ISqlBuilder, MySqlSqlBuilder>();
            services.TryAddScoped<ITransactionManager, MySqlTransactionManager>();
            services.TryAddScoped<DbContext, MySqlDbContext>();
            services.AddSingleton<TypeMap, MySqlTypeMap>();
            return services;
        }

        public static IServiceCollection AddMySql(this IDbBuilder builder, string connectString)
        {
            return builder.AddMySql(testc =>
            {
                testc.ConnectionString = connectString;
            });
        }
    }
}