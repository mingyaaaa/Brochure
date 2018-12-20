using System;
using System.Data;
using System.Data.Common;

namespace Brochure.Core.Server
{
    public class DbConnectionProxy : DbConnection
    {

        private DbConnection dbConnection;
        public DatabaseType DatabaseType { get; }
        public DbConnectionProxy(DbConnection connection, DatabaseType databaseType)
        {
            dbConnection = connection;
            DatabaseType = databaseType;
        }
        public override string ConnectionString { get => dbConnection.ConnectionString; set => dbConnection.ConnectionString = value; }

        public override string Database => dbConnection.Database;

        public override string DataSource => dbConnection.DataSource;

        public override string ServerVersion => dbConnection.ServerVersion;

        public override ConnectionState State => dbConnection.State;

        public override void ChangeDatabase(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new Exception("当前指定的数据库为null");
            if (dbConnection.Database != databaseName)
                dbConnection.ChangeDatabase(databaseName);

        }

        public override void Close()
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection?.Close();
        }

        public override void Open()
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection?.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return dbConnection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return dbConnection.CreateCommand();
        }
    }
}
