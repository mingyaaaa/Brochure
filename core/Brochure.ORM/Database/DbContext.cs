using Brochure.Abstract.PluginDI;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The db context.
    /// </summary>
    public abstract class DbContext : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        public DbDatabase Database { get; }
        /// <summary>
        /// Gets the tables.
        /// </summary>
        public DbTable Tables { get; }
        /// <summary>
        /// Gets the columns.
        /// </summary>
        public DbColumns Columns { get; }
        /// <summary>
        /// Gets the indexs.
        /// </summary>
        public DbIndex Indexs { get; }
        /// <summary>
        /// Gets the datas.
        /// </summary>
        public DbData Datas { get; }

        private IDbConnection _connection;

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        internal static IServiceProvider ServiceProvider { get; set; }

        private IServiceScope _serviceScope;
        private readonly IConnectFactory _connectFactory;
        private readonly ITransactionManager _transactionManager;
        private readonly bool _isBeginTransaction;
        private ITransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="dbDatabase">The db database.</param>
        /// <param name="dbTable">The db table.</param>
        /// <param name="dbColumns">The db columns.</param>
        /// <param name="dbIndex">The db index.</param>
        /// <param name="dbData">The db data.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="visitProvider">The visit provider.</param>
        public DbContext(DbDatabase dbDatabase,
            DbTable dbTable, DbColumns dbColumns,
            DbIndex dbIndex, DbData dbData,
            IConnectFactory connectFactory,
            ITransactionManager transactionManager)
        {
            this.Datas = dbData;
            _connectFactory = connectFactory;
            this.Indexs = dbIndex;
            this.Columns = dbColumns;
            this.Tables = dbTable;
            this.Database = dbDatabase;
            _transactionManager = transactionManager;
            _connection = connectFactory.CreateAndOpenConnection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public DbContext(bool isBeginTransaction = false)
        {
            _serviceScope = ServiceProvider.CreateScope();
            this.Datas = _serviceScope.ServiceProvider.GetRequiredService<DbData>();
            this.Indexs = _serviceScope.ServiceProvider.GetRequiredService<DbIndex>();
            this.Columns = _serviceScope.ServiceProvider.GetRequiredService<DbColumns>();
            this.Tables = _serviceScope.ServiceProvider.GetRequiredService<DbTable>();
            this.Database = _serviceScope.ServiceProvider.GetRequiredService<DbDatabase>();
            this._connectFactory = _serviceScope.ServiceProvider.GetRequiredService<IConnectFactory>();
            _transactionManager = _serviceScope.ServiceProvider.GetRequiredService<ITransactionManager>();
            _connection = _connectFactory.CreateAndOpenConnection();
            _isBeginTransaction = isBeginTransaction;
            BenginTransaction();
        }

        /// <summary>
        /// Disposes the async.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public ValueTask DisposeAsync()
        {
            _serviceScope?.Dispose();
            _connection?.Dispose();
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            _transaction?.Commit();
            _serviceScope?.Dispose();
            _connection?.Dispose();
            _transactionManager.RemoveTransaction(_transaction);
        }

        /// <summary>
        /// Bengins the transaction.
        /// </summary>
        /// <returns>An ITransaction.</returns>
        public ITransaction BenginTransaction()
        {
            if (_isBeginTransaction)
            {
                _transaction = new Transaction(_connectFactory);
                _transactionManager.AddTransaction(_transaction);
            }
            else
            {
                _transaction = new InnerTransaction();
                _transactionManager.AddTransaction(_transaction);
            }
            return _transaction;
        }

        /// <summary>
        /// Commits the.
        /// </summary>
        public void Commit()
        {
            _transaction?.Commit();
            Dispose();
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public void Rollback()
        {
            _transaction?.Rollback();
            Dispose();
        }
    }
}