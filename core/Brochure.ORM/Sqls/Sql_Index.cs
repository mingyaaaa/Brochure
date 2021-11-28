using System.Collections.Generic;

namespace Brochure.ORM
{
    public partial class Sql
    {
        public static ISql CreateIndex(string tableName, string[] columnsNames, string indexName, string sqlIndex, string database = "")
        {
            return new CreateIndexSql(tableName, columnsNames, indexName, sqlIndex, database);
        }

        public static ISql DeleteIndex(string tableName, string indexName, string database = "")
        {
            return new DeleteIndexSql(tableName, indexName, database);
        }
    }

    public class CreateIndexSql : ISql
    {
        public CreateIndexSql(string tableName, IEnumerable<string> columnNames, string indexName, string sqlIndex, string database)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            IndexName = indexName;
            SqlIndex = sqlIndex;
            Database = database;
        }

        public string TableName { get; }
        public string IndexName { get; }
        public IEnumerable<string> ColumnNames { get; }

        public string SqlIndex { get; }
        public string Database { get; set; }
    }

    public class DeleteIndexSql : ISql
    {
        public DeleteIndexSql(string tableName, string indexName, string database)
        {
            TableName = tableName;
            IndexName = indexName;
            Database = database;
        }

        public string TableName { get; }
        public string IndexName { get; }
        public string Database { get; set; }
    }
}