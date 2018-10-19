using Brochure.Core;
using Brochure.Core.Server;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums.sql;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlTableHub : IDataTableHub
    {
        public MySqlTableHub(IClient client)
        {
            Client = client;
        }
        public string DatabaseName { get; }

        public IClient Client { get; }
        #region Table
        public async Task<long> RegistTableAsync<T>() where T : EntityBase
        {
            return await CreateTableAsync<T>();
        }
        public async Task<long> CreateTableAsync<T>() where T : EntityBase
        {
            var command = GetCommand();
            var tableName = DbUtil.GetTableName<T>();
            var r = await IsExistTableAsync(tableName);
            if (r)
                return 1;
            command.CommandText = DbUtil.GetCreateTableSql<T>(Client.TypeMap);
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistTableAsync(string tableName)
        {
            var command = GetCommand();
            command.CommandText = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>() == 1;
        }
        public async Task<long> DeleteTableAsync(string tableName)
        {
            var command = GetCommand();
            command.CommandText = $"drop table  {tableName}";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateTableNameAsync(string olderName, string tableName)
        {
            var command = GetCommand();
            var r = await IsExistTableAsync(olderName);
            if (!r)
                return 0;
            command.CommandText = $"alter table {olderName} rename {tableName};";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        private DbCommand GetCommand()
        {
            var connect = DbConnectPool.GetDbConnection(DatabaseType.MySql, Client.DatabaseName);
            return connect.CreateCommand();

        }
        #endregion
    }
}
