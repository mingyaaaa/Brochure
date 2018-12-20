using Brochure.Core;
using Brochure.Core.Server;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.Server.MySql
{
    public class MySqlDatabaseHub : IDatabaseHub
    {
        private readonly DbConnection dbConnection;
        public MySqlDatabaseHub(IClient client)
        {
            Client = client;
            dbConnection = DbConnectPool.GetDbConnection(DatabaseType.MySql, Client.DatabaseName);
        }
        public IClient Client { get; }

        public async Task<long> ChangeDatabaseAsync(string databaseName)
        {
            try
            {
                await Task.Run(() => dbConnection.ChangeDatabase(databaseName));
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

            var isExist = await IsExistDataBaseAsync(databaseName);
            if (isExist)
                return 1;
            var command = GetCommand();
            command.CommandText = $"create database {databaseName}";
            var r = await Excute(() => command.ExecuteNonQueryAsync(), -1);
            return r;
        }
        public async Task<long> DeleteDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"drop database {databaseName}";
            //执行删除方法  删除成功 默认返回了0  改变返回的值；
            var rr = await Excute(() => command.ExecuteNonQueryAsync(), -1);
            return rr == 0 ? 1 : -1;
        }


        public async Task<bool> IsExistDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
            var rr = await Excute(() => command.ExecuteScalarAsync(), null);
            return rr.As<int>() == 1;
        }
        #endregion

        private DbCommand GetCommand()
        {
            dbConnection.Open();
            return dbConnection.CreateCommand();
        }

        private async Task<T> Excute<T>(Func<Task<T>> func, T errorValue)
        {
            var r = default(T);
            try
            {
                r = await func();
            }
            catch (Exception ex)
            {
                r = errorValue;
            }
            finally
            {
                dbConnection.Close();
            }
            return r;
        }


        public void Dispose()
        {
            dbConnection?.Close();
        }

        ~MySqlDatabaseHub()
        {
            dbConnection?.Close();
        }
    }
}
