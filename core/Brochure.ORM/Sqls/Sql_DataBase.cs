namespace Brochure.ORM
{
    /// <summary>
    /// The sql_ data base.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Databases the count.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql DatabaseCount(string databaseName)
        {
            return new CountDatabaseSql(databaseName);
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteDatabase(string databaseName)
        {
            return new DeleteDatabaseSql(databaseName);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql CreateDatabase(string databaseName)
        {
            return new CreateDatabaseSql(databaseName);
        }

        /// <summary>
        /// Gets the all database.
        /// </summary>
        /// <returns>An ISql.</returns>
        public static ISql GetAllDatabase()
        {
            return new AllDatabaseSql();
        }

        /// <summary>
        /// Gets the all table name.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetAllTableName(string databaseName)
        {
            return new AllTableNamesSql(databaseName);
        }
    }

    /// <summary>
    /// The count database.
    /// </summary>
    public class CountDatabaseSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountDatabase"/> class.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public CountDatabaseSql(string databaseName)
        {
            DatabaseName = databaseName;
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; }
    }

    /// <summary>
    /// The delete database sql.
    /// </summary>
    public class DeleteDatabaseSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountDatabase"/> class.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public DeleteDatabaseSql(string databaseName)
        {
            DatabaseName = databaseName;
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; }
    }

    /// <summary>
    /// The all table names sql.
    /// </summary>
    public class AllTableNamesSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountDatabase"/> class.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public AllTableNamesSql(string databaseName)
        {
            DatabaseName = databaseName;
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; }
    }

    /// <summary>
    /// The create database sql.
    /// </summary>
    public class CreateDatabaseSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountDatabase"/> class.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public CreateDatabaseSql(string databaseName)
        {
            DatabaseName = databaseName;
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; }
    }

    /// <summary>
    /// The all database sql.
    /// </summary>
    public class AllDatabaseSql : ISql
    {
    }
}