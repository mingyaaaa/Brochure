using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbTable
    {
        protected DbQueryOption Option;

        protected DbTable (DbQueryOption option)
        {
            Option = option;
        }

        public Task<long> CreateTableAsync (string tableName)
        {
            return Task.Run (() => CreateTable (tableName));
        }

        public abstract long CreateTable (string tableName);

        public Task<bool> IsExistTableAsync (string tableName)
        {
            return Task.Run (() => IsExistTable (tableName));
        }

        public abstract bool IsExistTable (string tableName);

        public Task<long> DeleteTableAsync (string tableName)
        {
            return Task.Run (() => DeleteTable (tableName));
        }

        public abstract long DeleteTable (string tableName);

        public Task<long> UpdateTableNameAsync (string tableName)
        {
            return Task.Run (() => UpdateTableName (tableName));
        }

        public abstract long UpdateTableName (string tableName);
    }
}