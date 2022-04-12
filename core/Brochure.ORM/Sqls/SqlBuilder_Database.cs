namespace Brochure.ORM
{
    /// <summary>
    /// The sql builder.
    /// </summary>
    public partial class SqlBuilder
    {
        /// <summary>
        /// Builds the count for database.
        /// </summary>
        /// <param name="countDatabaseSql">The count database sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildCountForDatabase(CountDatabaseSql countDatabaseSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{countDatabaseSql.Database}'";
            return result;
        }

        /// <summary>
        /// Builds the delete database.
        /// </summary>
        /// <param name="deleteDatabaseSql">The delete database sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildDeleteDatabase(DeleteDatabaseSql deleteDatabaseSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"drop database {deleteDatabaseSql.Database}";
            return result;
        }

        /// <summary>
        /// Builds the create database.
        /// </summary>
        /// <param name="createDatabaseSql">The create database sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildCreateDatabase(CreateDatabaseSql createDatabaseSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"create database {createDatabaseSql.Database}";
            return result;
        }

        /// <summary>
        /// Builds the all database.
        /// </summary>
        /// <param name="allDatabaseSql">The all database sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildAllDatabase(AllDatabaseSql allDatabaseSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = "select schema_name from information_schema.schemata";
            return result;
        }

        /// <summary>
        /// Builds the all table name.
        /// </summary>
        /// <param name="allTableNamesSql">The all table names sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildAllTableName(AllTableNamesSql allTableNamesSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"select table_name from information_schema.tables where table_schema='{allTableNamesSql.Database}'";
            return result;
        }
    }
}