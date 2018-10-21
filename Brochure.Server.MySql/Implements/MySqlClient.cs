using Brochure.Core;
using Brochure.Core.Server;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums.sql;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlClient : IClient, IDbTransaction
    {
        private DbConnection dbConnection;
        private DbTransaction _dbTransaction;
        private string a;
        public string DatabaseName { get; set; }
        protected MySqlClient()
        {
            TypeMap = new MySqlTypeMap();
        }
        public MySqlClient(string database = null) : this()
        {
            DatabaseName = database;
        }
        public TypeMap TypeMap { get; }

        public ISqlParse SqlParse => new MySqlParse();

        public bool IsBeginTransaction { get; private set; }

        public DbFactory Factory { get; }

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
                if (!string.IsNullOrWhiteSpace(databaseName))
                    DatabaseName = databaseName;
            }
            var databasehub = await GetDatabaseHubAsync();
            await databasehub.ChangeDatabaseAsync(DatabaseName);
            return new MySqlTableHub(this);
        }

        public async Task<IDataHub> GetDataHubAsync<T>() where T : EntityBase
        {
            var tableName = DbUtil.GetTableName<T>();
            return await GetDataHubAsync(tableName);
        }

        public async Task<IDataHub> GetDataHubAsync(string tableName)
        {
            await Task.Delay(0);
            if (string.IsNullOrWhiteSpace(DatabaseName))
                throw new Exception("没有指定数据库");
            var connection = GetConnection();
            return new MySqlDataHub(this, connection, tableName, _dbTransaction);
        }

        public void SetDatabase(string databaseName)
        {
            DatabaseName = databaseName;
        }

        private DbConnection GetConnection()
        {
            if (IsBeginTransaction)
            {
                //如果开启事务则使用同一个连接
                if (dbConnection == null)
                    dbConnection = DbConnectPool.GetDbConnection(DatabaseType.MySql, DatabaseName);
                return dbConnection;
            }
            dbConnection = DbConnectPool.GetDbConnection(DatabaseType.MySql, DatabaseName);
            return dbConnection;
        }

        public void BeginTransaction()
        {
            IsBeginTransaction = true;
        }

        public void Commit()
        {
            _dbTransaction?.Commit();
            IsBeginTransaction = false;
            _dbTransaction = null;
        }

        public void Rollback()
        {
            _dbTransaction?.Rollback();
            IsBeginTransaction = false;
            _dbTransaction = null;
        }
    }
}
