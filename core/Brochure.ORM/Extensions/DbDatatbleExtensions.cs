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
    /// The db datatble extensions.
    /// </summary>
    public static class DbDatatbleExtensions
    {
        /// <summary>
        /// Tries the create table.
        /// </summary>
        /// <param name="dbTable">The db table.</param>
        /// <param name="databaseName"></param>
        /// <returns>A ValueTask.</returns>
        public static async Task<int> TryCreateTableAsync<T>(this DbTable dbTable, string databaseName)
        {
            var exist = await dbTable.IsExistTableAsync<T>(databaseName);
            if (!exist)
                return await dbTable.CreateTableAsync<T>(databaseName);
            return -1;
        }

        /// <summary>
        /// Tries the delete table async.
        /// </summary>
        /// <param name="dbTable">The db table.</param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryDeleteTableAsync<T>(this DbTable dbTable, string databaseName)
        {
            var exist = await dbTable.IsExistTableAsync<T>(databaseName);
            if (exist)
                return await dbTable.DeleteTableAsync<T>(databaseName);
            return -1;
        }

        /// <summary>
        /// Updates the table name async.
        /// </summary>
        /// <param name="dbTable"></param>
        /// <param name="tableName">The table name.</param>
        /// <param name="newTableName">The new table name.</param>
        /// <param name="databaseName"></param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryUpdateTableNameAsync(this DbTable dbTable, string tableName, string newTableName, string databaseName = "")
        {
            var isExist = await dbTable.IsExistTableAsync(tableName, databaseName);
            if (!isExist)
                return -1;
            return await dbTable.UpdateTableNameAsync(tableName, newTableName, databaseName);
        }
    }
}