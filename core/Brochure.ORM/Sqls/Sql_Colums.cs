using System;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Gets the colums names.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetColumsNames(string tableName, string database = "")
        {
            return new ColumsNamesSql(tableName, database);
        }

        /// <summary>
        /// Gets the columns count.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetColumnsCount(string tableName, string columnName, string database = "")
        {
            return new CountColumsSql(tableName, columnName, database);
        }

        /// <summary>
        /// Gets the rename column name sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetRenameColumnNameSql(string tableName, string oldName, string newName, TypeCode typeCode, int length = 0, string database = "")
        {
            return new RenameColumnSql(tableName, oldName, newName, typeCode, length, database);
        }

        /// <summary>
        /// Gets the update column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetUpdateColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string database = "")
        {
            return new UpdateColumnsSql(tableName, columnName, typeCode, isNotNull, length, database);
        }

        /// <summary>
        /// Gets the add column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetAddColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string database = "")
        {
            return new AddColumnsSql(tableName, columnName, typeCode, isNotNull, length, database);
        }

        /// <summary>
        /// Gets the delete column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetDeleteColumnSql(string tableName, string columnName, string database = "")
        {
            return new DeleteColumsSql(tableName, columnName, database);
        }
    }

    /// <summary>
    /// The colums names sql.
    /// </summary>
    public class ColumsNamesSql : ISql
    {
        public string TableName { get; }

        public string Database { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumsNamesSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="databaseName">The database name.</param>
        public ColumsNamesSql(string tableName, string database)
        {
            TableName = tableName;
            Database = database;
        }
    }

    /// <summary>
    /// The count colums sql.
    /// </summary>
    public class CountColumsSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountColumsSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnsName">The colums name.</param>
        /// <param name="database">The database.</param>
        public CountColumsSql(string tableName, string columnsName, string database)
        {
            TableName = tableName;
            ColumnsName = columnsName;
            Database = database;
        }

        public string Database { get; set; }

        public string TableName { get; }

        public string ColumnsName { get; }
    }

    /// <summary>
    /// The rename column sql.
    /// </summary>
    public class RenameColumnSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameColumnSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="lentgh">The lentgh.</param>
        /// <param name="database">The database.</param>
        public RenameColumnSql(string tableName, string oldName, string newName, TypeCode typeCode, int lentgh, string database)
        {
            Database = database;
            TableName = tableName;
            OldName = oldName;
            NewName = newName;
            TypeCode = typeCode;
            Lentgh = lentgh;
        }

        public string Database { get; set; }

        public string TableName { get; }

        public string OldName { get; }

        public string NewName { get; }

        public TypeCode TypeCode { get; }

        public int Lentgh { get; }
    }

    /// <summary>
    /// The update columns sql.
    /// </summary>
    public class UpdateColumnsSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateColumnsSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="database">The database.</param>
        /// <param name="lentgh">The lentgh.</param>
        public UpdateColumnsSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int lentgh, string database)
        {
            TableName = tableName;
            ColumnName = columnName;
            TypeCode = typeCode;
            Lentgh = lentgh;
            Database = database;
            IsNotNull = isNotNull;
        }

        public string Database { get; set; }

        public string TableName { get; }

        public string ColumnName { get; }

        public TypeCode TypeCode { get; }

        public int Lentgh { get; }

        public bool IsNotNull { get; }
    }

    /// <summary>
    /// The add columns sql.
    /// </summary>
    public class AddColumnsSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddColumnsSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="database">The database.</param>
        /// <param name="lentgh">The lentgh.</param>
        public AddColumnsSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int lentgh, string database)
        {
            TableName = tableName;
            ColumnName = columnName;
            TypeCode = typeCode;
            Lentgh = lentgh;
            Database = database;
            IsNotNull = isNotNull;
        }

        public string Database { get; set; }

        public string TableName { get; }

        public string ColumnName { get; }

        public string NewName { get; }

        public TypeCode TypeCode { get; }

        public int Lentgh { get; }

        public bool IsNotNull { get; }
    }

    /// <summary>
    /// The delete colums sql.
    /// </summary>
    public class DeleteColumsSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteColumsSql"/> class.
        /// </summary>
        public DeleteColumsSql()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteColumsSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columsName">The colums name.</param>
        /// <param name="database">The database.</param>
        public DeleteColumsSql(string tableName, string columsName, string database)
        {
            TableName = tableName;
            ColumsName = columsName;
            Database = database;
        }

        public string Database { get; set; }

        public string TableName { get; }

        public string ColumsName { get; }
    }
}