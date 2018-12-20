using Brochure.Core;
using Brochure.Core.Server;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlClient : IClient
    {
        private DbConnection dbConnection;
        private DbTransaction _dbTransaction;
        private string a;
        public string DatabaseName { get; set; }
        protected MySqlClient()
        {
            TypeMap = new MySqlTypeMap();
            SqlParse = new MySqlParse();
        }
        public MySqlClient(string database = null) : this()
        {
            DatabaseName = database;
        }
        public TypeMap TypeMap { get; }

        public ISqlParse SqlParse { get; }


        public async Task<IDatabaseHub> GetDatabaseHubAsync()
        {
            await Task.Delay(0);
            return new MySqlDatabaseHub(this);
        }

        public async Task<IDataTableHub> GetDataTableHubAsync(string databaseName = null)
        {
            if (string.IsNullOrWhiteSpace(DatabaseName))
            {
                if (string.IsNullOrWhiteSpace(databaseName))
                    throw new Exception("指定数据库字段为空");
                DatabaseName = databaseName;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(databaseName) && databaseName != DatabaseName)
                {
                    var databasehub = await GetDatabaseHubAsync();
                    await databasehub.ChangeDatabaseAsync(DatabaseName);
                    DatabaseName = databaseName;
                }
            }
            return new MySqlTableHub(this);
        }

        public async Task<IDataHub> GetDataHubAsync<T>(IDbTransaction dbTransaction = null) where T : EntityBase
        {
            var tableName = DbUtil.GetTableName<T>();
            return await GetDataHubAsync(tableName);
        }

        public async Task<IDataHub> GetDataHubAsync(string tableName, IDbTransaction dbTransaction = null)
        {
            await Task.Delay(0);
            if (string.IsNullOrWhiteSpace(DatabaseName))
                throw new Exception("没有指定数据库");
            var connection = GetConnection();
            return new MySqlDataHub(this, tableName, dbTransaction);
        }

        public void SetDatabase(string databaseName)
        {
            DatabaseName = databaseName;
        }

        private DbConnection GetConnection()
        {
            dbConnection = DbConnectPool.GetDbConnection(DatabaseType.MySql, DatabaseName);
            return dbConnection;
        }

        public IDbTransaction BeginTransaction()
        {
            var connection = GetConnection();
            return new MySqlDbTransaction(connection.BeginTransaction());
        }
    }
}
