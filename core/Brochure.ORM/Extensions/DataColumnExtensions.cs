using Brochure.ORM.Database;
using System;
using System.Threading.Tasks;

namespace Brochure.ORM.Extensions
{
    /// <summary>
    /// The data column extensions.
    /// </summary>
    public static class DataColumnExtensions
    {
        /// <summary>
        /// Tries the add column async.
        /// </summary>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryAddColumnAsync<T>(this DbColumns dbColumns, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            var exist = await dbColumns.IsExistColumnAsync<T>(columnName);
            if (!exist)
                return await dbColumns.AddColumnsAsync<T>(columnName, typeCode, isNotNull, length, databaseName);
            return -1;
        }

        /// <summary>
        /// Tries the add column async.
        /// </summary>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryAddColumnAsync(this DbColumns dbColumns, string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            var exist = await dbColumns.IsExistColumnAsync(tableName, columnName);
            if (!exist)
                return await dbColumns.AddColumnsAsync(tableName, columnName, typeCode, isNotNull, length, databaseName);
            return -1;
        }

        /// <summary>
        /// Tries the rename column name async.
        /// </summary>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="oldColumnName">The old column name.</param>
        /// <param name="newColumnName">The new column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryRenameColumnNameAsync<T>(this DbColumns dbColumns, string oldColumnName, string newColumnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            var exist = await dbColumns.IsExistColumnAsync<T>(oldColumnName);
            if (!exist)
                return await dbColumns.RenameColumnAsync<T>(oldColumnName, newColumnName, typeCode, isNotNull, length, databaseName);
            return -1;
        }

        /// <summary>
        /// Tries the rename column name async.
        /// </summary>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="oldColumnName">The old column name.</param>
        /// <param name="newColumnName">The new column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A Task.</returns>
        public static async Task<int> TryRenameColumnNameAsync(this DbColumns dbColumns, string tableName, string oldColumnName, string newColumnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            var exist = await dbColumns.IsExistColumnAsync(tableName, oldColumnName);
            if (!exist)
                return await dbColumns.RenameColumnAsync(tableName, oldColumnName, newColumnName, typeCode, isNotNull, length, databaseName);
            return -1;
        }
    }
}