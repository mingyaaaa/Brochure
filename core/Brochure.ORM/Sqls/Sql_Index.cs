using System.Collections.Generic;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql.
    /// </summary>
    public partial class Sql
    {
        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnsNames">The columns names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <param name="database">The database.</param>
        /// <returns>An ISql.</returns>
        public static ISql CreateIndex(string tableName, string[] columnsNames, string indexName, string sqlIndex, string database = "")
        {
            return new CreateIndexSql(tableName, columnsNames, indexName, sqlIndex, database);
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="database">The database.</param>
        /// <returns>An ISql.</returns>
        public static ISql DeleteIndex(string tableName, string indexName, string database = "")
        {
            return new DeleteIndexSql(tableName, indexName, database);
        }

        /// <summary>
        /// Counts the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="database">The database.</param>
        /// <returns>An ISql.</returns>
        public static ISql CountIndex(string tableName, string indexName, string database = "")
        {
            return new CountIndexSql(tableName, indexName, database);
        }

        /// <summary>
        /// Renames the index.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="oldIndexName">The old index name.</param>
        /// <param name="newIndexName">The new index name.</param>
        /// <param name="database">The database.</param>
        /// <returns>An ISql.</returns>
        public static ISql RenameIndex(string tableName, string oldIndexName, string newIndexName, string database = "")
        {
            return new RenameIndexSql(tableName, oldIndexName, newIndexName, database);
        }
    }

    /// <summary>
    /// The create index sql.
    /// </summary>
    public class CreateIndexSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIndexSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <param name="database">The database.</param>
        public CreateIndexSql(string tableName, IEnumerable<string> columnNames, string indexName, string sqlIndex, string database)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            IndexName = indexName;
            SqlIndex = sqlIndex;
            Database = database;
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the index name.
        /// </summary>
        public string IndexName { get; }

        /// <summary>
        /// Gets the column names.
        /// </summary>
        public IEnumerable<string> ColumnNames { get; }

        /// <summary>
        /// Gets the sql index.
        /// </summary>
        public string SqlIndex { get; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
    }

    /// <summary>
    /// The delete index sql.
    /// </summary>
    public class DeleteIndexSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIndexSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="database">The database.</param>
        public DeleteIndexSql(string tableName, string indexName, string database)
        {
            TableName = tableName;
            IndexName = indexName;
            Database = database;
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the index name.
        /// </summary>
        public string IndexName { get; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
    }

    /// <summary>
    /// The index count sql.
    /// </summary>
    public class CountIndexSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountIndexSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="database">The database.</param>
        public CountIndexSql(string tableName, string indexName, string database)
        {
            TableName = tableName;
            Database = database;
            IndexName = indexName;
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the index name.
        /// </summary>
        public string IndexName { get; set; }
    }

    /// <summary>
    /// The rename index sql.
    /// </summary>
    public class RenameIndexSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameIndexSql"/> class.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="oldIndexName">The old index name.</param>
        /// <param name="newIndexName">The new index name.</param>
        /// <param name="database">The database.</param>
        public RenameIndexSql(string tableName, string oldIndexName, string newIndexName, string database)
        {
            TableName = tableName;
            Database = database;
            OldIndexName = oldIndexName;
            NewIndexName = newIndexName;
        }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the old index name.
        /// </summary>
        public string OldIndexName { get; set; }

        /// <summary>
        /// Gets or sets the new index name.
        /// </summary>
        public string NewIndexName { get; set; }
    }
}