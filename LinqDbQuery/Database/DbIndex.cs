using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbIndex
    {
        protected DbQueryOption Option;
        public DbIndex (DbQueryOption option)
        {
            Option = option;
        }

        public Task<long> CreateIndexAsync (string[] columnNames, string indexName, string sqlIndex)
        {
            return Task.Run (() =>
            {
                return CreateIndex (columnNames, indexName, sqlIndex);
            });
        }
        public abstract long CreateIndex (string[] columnNames, string indexName, string sqlIndex);
        public Task<long> DeleteIndexAsync (string indexName)
        {
            return Task.Run (() =>
            {
                return DeleteIndex (indexName);
            });
        }
        public abstract long DeleteIndex (string indexName);
    }
}