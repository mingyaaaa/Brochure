using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The connect factory.
    /// </summary>
    public interface IConnectFactory : IDisposable
    {
        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        DbConnection CreateConnection();

        /// <summary>
        /// Creates the and open connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        Task<DbConnection> CreateAndOpenConnectionAsync();
    }

    /// <summary>
    /// The connect factory.
    /// </summary>
    public class ConnectFactory : IConnectFactory
    {
        private readonly IDbProvider provider;
        private DbConnection dbConnection;
        private readonly DbOption dbOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectFactory"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="dbOption">The db option.</param>
        public ConnectFactory(IDbProvider provider, DbOption dbOption)
        {
            this.provider = provider;
            this.dbOption = dbOption;
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        public DbConnection CreateConnection()
        {
            if (dbConnection != null)
                return dbConnection;
            dbConnection = provider.GetDbConnection();
            if (string.IsNullOrWhiteSpace(dbOption.ConnectionString))
                throw new Exception("请设置数据库连接字符串");
            dbConnection.ConnectionString = dbOption.ConnectionString;
            dbOption.DatabaseName = dbConnection.Database;
            return dbConnection;
        }

        /// <summary>
        /// Creates the and open connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        public async Task<DbConnection> CreateAndOpenConnectionAsync()
        {
            var connect = CreateConnection();
            if (connect.State == ConnectionState.Closed)
                await connect.OpenAsync().ConfigureAwait(false);
            return connect;
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            if (this.dbConnection == null)
                return;
            if (this.dbConnection.State != ConnectionState.Closed)
                this.dbConnection.Close();
            this.dbConnection.Dispose();
            this.dbConnection = null;
        }
    }
}