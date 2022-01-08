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
        /// <param name="tableName">The table name.</param>
        /// <param name="databaseName"></param>
        /// <returns>An ISql.</returns>
        public static ISql GetColumsNames(string tableName, string databaseName = "")
        {
            return new ColumsNamesSql(tableName, databaseName);
        }

        /// <summary>
        /// Gets the columns count.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetColumnsCount(string tableName, string columnName, string databaseName = "")
        {
            return new CountColumsSql(tableName, columnName, databaseName);
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
        public static ISql GetRenameColumnNameSql(string tableName, string oldName, string newName, TypeCode typeCode, int length = 0, string databaseName = "")
        {
            return new RenameColumnSql(tableName, oldName, newName, typeCode, length, databaseName);
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
        public static ISql GetUpdateColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            return new UpdateColumnsSql(tableName, columnName, typeCode, isNotNull, length, databaseName);
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
        public static ISql GetAddColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0, string databaseName = "")
        {
            return new AddColumnsSql(tableName, columnName, typeCode, isNotNull, length, databaseName);
        }

        /// <summary>
        /// Gets the delete column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>An ISql.</returns>
        public static ISql GetDeleteColumnSql(string tableName, string columnName, string databaseName = "")
        {
            return new DeleteColumsSql(tableName, columnName, databaseName);
        }
    }

    /// <summary>
    /// The colums names sql.
    /// </summary>
    public class ColumsNamesSql : ISql
    {
        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumsNamesSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="databaseName">The database name.</param>
        public ColumsNamesSql(string tableName, string databaseName)
        {
            TableName = tableName;
            Database = databaseName;
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

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the columns name.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the old name.
        /// </summary>
        public string OldName { get; }

        /// <summary>
        /// Gets the new name.
        /// </summary>
        public string NewName { get; }

        /// <summary>
        /// Gets the type code.
        /// </summary>
        public TypeCode TypeCode { get; }

        /// <summary>
        /// Gets the lentgh.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the type code.
        /// </summary>
        public TypeCode TypeCode { get; }

        /// <summary>
        /// Gets the lentgh.
        /// </summary>
        public int Lentgh { get; }

        /// <summary>
        /// Gets a value indicating whether not is null.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets the new name.
        /// </summary>
        public string NewName { get; }

        /// <summary>
        /// Gets the type code.
        /// </summary>
        public TypeCode TypeCode { get; }

        /// <summary>
        /// Gets the lentgh.
        /// </summary>
        public int Lentgh { get; }

        /// <summary>
        /// Gets a value indicating whether not is null.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the colums name.
        /// </summary>
        public string ColumsName { get; }
    }
}