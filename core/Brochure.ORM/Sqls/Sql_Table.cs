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
        public static ISql CreateTable(Type type, string database = "")
        {
            return new CreateTableSql(type, database);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>An ISql.</returns>
        public static ISql CreateTable<T>(string database = "")
        {
            return CreateTable(typeof(T), database);
        }

        /// <summary>
        /// Gets the count table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetCountTable(string tableName, string database = "")
        {
            return new CountTableSql(tableName, database);
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>An ISql.</returns>
        public static ISql UpdateTableName(string oldName, string newName, string database = "")
        {
            return new UpdateTableNameSql(newName, oldName, database);
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteTable(string tableName, string database = "")
        {
            return new DeleteTableSql(tableName, database);
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
        public CreateTableSql(Type type, string database)
        {
            TableType = type;
            Database = database;
        }

        public Type TableType { get; }

        public string Database { get; set; }
    }

    /// <summary>
    /// The count table sql.
    /// </summary>
    public class CountTableSql : ISql
    {
        public string TableName { get; }

        public string Database { get; set; }

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
        public UpdateTableNameSql(string newName, string oldName, string database)
        {
            NewName = newName;
            OldName = oldName;
            Database = database;
        }

        public string NewName { get; }

        public string Database { get; set; }
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
        public DeleteTableSql(string tableName, string database)
        {
            TableName = tableName;
            Database = database;
        }

        public string Database { get; set; }
    }
}