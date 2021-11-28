using System.Collections.Generic;

namespace Brochure.ORM
{
    public partial class Sql
    {
        public static ISql CreateIndex(string tableName, string[] columnsNames, string indexName, string sqlIndex)
        {
            return new CreateIndexSql(tableName, columnsNames, indexName, sqlIndex);
        }

        public static ISql DeleteIndex(string tableName, string indexName)
        {
            return new DeleteIndexSql(tableName, indexName);
        }
    }

    public class CreateIndexSql : ISql
    {
        public CreateIndexSql(string tableName, IEnumerable<string> columnNames, string indexName, string sqlIndex)
        {
            TableName = tableName;
            ColumnNames = columnNames;
            IndexName = indexName;
            SqlIndex = sqlIndex;
        }

        public string TableName { get; }
        public string IndexName { get; }
        public IEnumerable<string> ColumnNames { get; }

        public string SqlIndex { get; }
    }

    public class DeleteIndexSql : ISql
    {
        public DeleteIndexSql(string tableName, string indexName)
        {
            TableName = tableName;
            IndexName = indexName;
        }

        public string TableName { get; }
        public string IndexName { get; }
    }
}