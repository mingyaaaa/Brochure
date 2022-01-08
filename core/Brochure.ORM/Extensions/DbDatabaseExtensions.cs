using Brochure.ORM.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Extensions
{
    /// <summary>
    /// The db database extensions.
    /// </summary>
    public static class DbDatabaseExtensions
    {
        /// <summary>
        /// Tries the create table.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="databaseName"></param>
        /// <returns>A ValueTask.</returns>
        public static async Task<int> TryCreateDatabaseAsync(this DbDatabase database, string databaseName)
        {
            var exist = await database.IsExistDataBaseAsync(databaseName);
            if (!exist)
                return await database.CreateDatabaseAsync(databaseName);
            return -1;
        }

        /// <summary>
        /// Tries the delete table async.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryDeleteDatabaseAsync<T>(this DbDatabase database, string databaseName)
        {
            var exist = await database.IsExistDataBaseAsync(databaseName);
            if (exist)
                return await database.CreateDatabaseAsync(databaseName);
            return -1;
        }
    }
}