using System;
using System.Data.Common;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Server;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlTableHub : IDataTableHub
    {
        private DbConnection dbConnection;
        public MySqlTableHub (IClient client)
        {
            Client = client;
            dbConnection = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
        }

        public IClient Client { get; }
        #region Table
        public async Task<long> RegistTableAsync<T> () where T : EntityBase
        {
            return await CreateTableAsync<T> ();
        }
        public async Task<long> CreateTableAsync<T> () where T : EntityBase
        {

            var tableName = DbUtil.GetTableName<T> ();
            var r = await IsExistTableAsync (tableName);
            if (r)
                return 1;
            var command = GetCommand ();
            command.CommandText = DbUtil.GetCreateTableSql<T> (Client.TypeMap);
            var rr = await Excute (() => command.ExecuteNonQueryAsync (), -1);
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistTableAsync (string tableName)
        {
            var command = GetCommand ();
            command.CommandText = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";

            var rr = await Excute (() => command.ExecuteScalarAsync (), null);
            return rr.As<int> () == 1;
        }
        public async Task<long> DeleteTableAsync (string tableName)
        {
            var command = GetCommand ();
            command.CommandText = $"drop table  {tableName}";
            var rr = await Excute (() => command.ExecuteNonQueryAsync (), -1);
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateTableNameAsync (string olderName, string tableName)
        {

            var r = await IsExistTableAsync (olderName);
            if (!r)
                return 0;
            var command = GetCommand ();
            command.CommandText = $"alter table {olderName} rename {tableName};";
            var rr = await Excute (() => command.ExecuteNonQueryAsync (), -1);
            return rr == 0 ? 1 : -1;
        }
        private DbCommand GetCommand ()
        {
            dbConnection.Open ();
            return dbConnection.CreateCommand ();

        }
        private async Task<T> Excute<T> (Func<Task<T>> func, T errorValue)
        {
            var r = default (T);
            try
            {
                r = await func.Invoke ();
            }
            catch (Exception ex)
            {
                r = errorValue;
            }
            finally
            {
                dbConnection.Close ();
            }
            return r;
        }
        public void Dispose ()
        {
            dbConnection.Close ();
        }

        ~MySqlTableHub ()
        {
            dbConnection.Close ();
        }
        #endregion
    }
}