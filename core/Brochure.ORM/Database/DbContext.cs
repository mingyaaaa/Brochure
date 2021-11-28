using Brochure.Abstract;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The db context.
    /// </summary>
    public abstract class DbContext : IAsyncDisposable
    {
        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        internal static IServiceProvider ServiceProvider { get; set; }

        private IServiceScope _serviceScope;
        private readonly IObjectFactory _objectFactory;
        private readonly IConnectFactory _connectFactory;
        private readonly ITransactionManager _transactionManager;
        private readonly ISqlBuilder _sqlBuilder;
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
        protected DbContext(
            IObjectFactory objectFactory,
            IConnectFactory connectFactory,
            ITransactionManager transactionManager,
            ISqlBuilder sqlBuilder)
        {
            _objectFactory = objectFactory;
            _connectFactory = connectFactory;
            _transactionManager = transactionManager;
            _sqlBuilder = sqlBuilder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        protected DbContext(bool isBeginTransaction = false)
        {
            _serviceScope = ServiceProvider?.CreateScope();
            this._connectFactory = _serviceScope?.ServiceProvider.GetRequiredService<IConnectFactory>();
            _transactionManager = _serviceScope?.ServiceProvider.GetRequiredService<ITransactionManager>();
            _isBeginTransaction = isBeginTransaction;
        }

        /// <summary>
        /// Disposes the async.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public async ValueTask DisposeAsync()
        {
            await _transaction?.CommitAsync();
            _serviceScope?.Dispose();
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
        public async Task CommitAsync()
        {
            await _transaction?.CommitAsync();
            await DisposeAsync();
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public async Task Rollback()
        {
            await _transaction?.RollbackAsync();
            await DisposeAsync();
        }

        /// <summary>
        /// Excutes the query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>A list of TS.</returns>
        public virtual async Task<IEnumerable<T>> ExcuteQueryAsync<T>(params ISql[] sqls) where T : class, new()
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
            var list = new List<T>();
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// Excutes the query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>A list of TS.</returns>
        public virtual Task<IEnumerable<T>> ExcuteQueryAsync<T>(IEnumerable<ISql> sqls) where T : class, new()
        {
            return ExcuteQueryAsync<T>(sqls.ToArray());
        }

        /// <summary>
        /// Excutes the no query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An int.</returns>
        public virtual async Task<int> ExcuteNoQueryAsync(params ISql[] sqls)
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Excutes the no query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An int.</returns>
        public virtual Task<int> ExcuteNoQueryAsync(IEnumerable<ISql> sqls)
        {
            return ExcuteNoQueryAsync(sqls.ToArray());
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An object.</returns>
        public virtual async Task<object> ExecuteScalarAsync(params ISql[] sqls)
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return command.ExecuteScalarAsync();
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An object.</returns>
        public virtual Task<object> ExecuteScalarAsync(IEnumerable<ISqlResult> sqls)
        {
            return ExecuteScalarAsync(sqls.ToArray());
        }

        /// <summary>
        /// Creates the db command.
        /// </summary>
        /// <returns>An IDbCommand.</returns>
        private async Task<DbCommand> CreateDbCommandAsync()
        {
            var connect = await _connectFactory.CreateAndOpenConnectionAsync();
            var command = connect.CreateCommand();
            command.Transaction = await _transactionManager.GetDbTransactionAsync();
            return command;
        }

        /// <summary>
        /// Changes the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public virtual void ChangeDatabase(string databaseName)
        {
            var connection = _connectFactory.CreateConnection();
            connection.ChangeDatabase(databaseName);
        }
    }
}