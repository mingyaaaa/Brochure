using Brochure.Core;
using Brochure.Core.Server;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums.sql;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.Server.MySql
{
    public class MySqlDatabaseHub : IDatabaseHub
    {
        public MySqlDatabaseHub(IClient client)
        {
            Client = client;
        }
        public IClient Client { get; }

        public async Task<long> ChangeDatabaseAsync(string databaseName)
        {
            var connection = DbConnectPool.GetDbConnection(DatabaseType.MySql, databaseName);
            try
            {
                await Task.Run(() => connection.ChangeDatabase(databaseName));
                return 1;
            }
            catch (System.Exception e)
            {
                return -1;
            }

        }

        #region Database
        public async Task<long> CreateDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            var isExist = await IsExistDataBaseAsync(databaseName);
            if (isExist)
                return 1;
            command.CommandText = $"create database {databaseName}";
            return await command.ExecuteNonQueryAsync();
        }
        public async Task<long> DeleteDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"drop database {databaseName}";
            //执行删除方法  删除成功 默认返回了0  改变返回的值；
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>() == 1;
        }
        #endregion

        private DbCommand GetCommand()
        {
            var connection = DbConnectPool.GetDbConnection(DatabaseType.MySql, Client.DatabaseName);
            return connection.CreateCommand();
        }
    }
}
