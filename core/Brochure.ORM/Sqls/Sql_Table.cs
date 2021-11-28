using System;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An ISql.</returns>
        public static ISql CreateTable(Type type)
        {
            return new CreateTableSql(type);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>An ISql.</returns>
        public static ISql CreateTable<T>()
        {
            return CreateTable(typeof(T));
        }

        /// <summary>
        /// Gets the count table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetCountTable(string tableName, string databaseName)
        {
            return new CountTableSql(tableName, databaseName);
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateTableName(string oldName, string newName)
        {
            return new UpdateTableNameSql(newName, oldName);
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteTable(string tableName)
        {
            return new DeleteTableSql(tableName);
        }
    }

    /// <summary>
    /// The create table sql.
    /// </summary>
    public class CreateTableSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTableSql"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public CreateTableSql(Type type)
        {
            TableType = type;
        }

        public Type TableType { get; }
    }

    /// <summary>
    /// The count table sql.
    /// </summary>
    public class CountTableSql : ISql
    {
        public string TableName { get; }

        public string Database { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountTableSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        public CountTableSql(string tableName, string database)
        {
            TableName = tableName;
            Database = database;
        }
    }

    /// <summary>
    /// The update table name sql.
    /// </summary>
    public class UpdateTableNameSql : ISql
    {
        public string OldName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTableNameSql"/> class.
        /// </summary>
        /// <param name="newName">The new name.</param>
        /// <param name="oldName">The old name.</param>
        public UpdateTableNameSql(string newName, string oldName)
        {
            NewName = newName;
            OldName = oldName;
        }

        public string NewName { get; }
    }

    /// <summary>
    /// The delete table sql.
    /// </summary>
    public class DeleteTableSql : ISql
    {
        public string TableName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTableSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        public DeleteTableSql(string tableName)
        {
            TableName = tableName;
        }
    }
}