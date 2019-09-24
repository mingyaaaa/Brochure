using System.Threading.Tasks;
namespace LinqDbQuery.Database
{
    public abstract class DbDatabase
    {
        protected DbQueryOption Option;
        public DbDatabase (DbQueryOption option)
        {
            Option = option;
        }
        public Task<long> CreateDatabaseAsync (string databaseName)
        {
            return Task.Run<long> (() =>
            {
                return CreateDatabase (databaseName);
            });
        }
        public abstract long CreateDatabase (string databaseName);

        public Task<long> DeleteDatabaseAsync (string databaseName)
        {
            return Task.Run<long> (() =>
            {
                return DeleteDatabase (databaseName);
            });
        }
        public abstract long DeleteDatabase (string databaseName);

        public Task<bool> IsExistDataBaseAsync (string databaseName)
        {
            return Task.Run (() =>
            {
                return IsExistDataBase (databaseName);
            });
        }
        public abstract bool IsExistDataBase (string databaseName);
    }
}