using Brochure.Abstract;
using Brochure.Core.PluginsDI;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The db context.
    /// </summary>
    public abstract class DbContext : IAsyncDisposable, ITransaction
    {
        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        internal static IServiceProvider ServiceProvider => AccessServiceProvider.Service;

        public IsolationLevel IsolationLevel => _transaction == null ? IsolationLevel.Unspecified : _transaction.IsolationLevel;
        private bool _isRollbackOrCommit = false;
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
        /// <param name="objectFactory">The object factory.</param>
        /// <param name="connectFactory">The connect factory.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="sqlBuilder">The sql builder.</param>
        protected DbContext(
            IObjectFactory objectFactory,
            IConnectFactory connectFactory,
            ITransactionManager transactionManager,
            ISqlBuilder sqlBuilder, IServiceScope serviceScope)
        {
            _objectFactory = objectFactory;
            _connectFactory = connectFactory;
            _transactionManager = transactionManager;
            _sqlBuilder = sqlBuilder;
            _isBeginTransaction = false;
            _serviceScope = serviceScope;
        }

        public DbData Datas => _serviceScope.ServiceProvider.GetRequiredService<DbData>();
        public DbTable Tables => _serviceScope.ServiceProvider.GetRequiredService<DbTable>();
        public DbDatabase Databases => _serviceScope.ServiceProvider.GetRequiredService<DbDatabase>();
        public DbColumn Columns => _serviceScope.ServiceProvider.GetRequiredService<DbColumn>();
        public DbIndex Indexs => _serviceScope.ServiceProvider.GetRequiredService<DbIndex>();

        public string DatabaseName => _connectFactory.GetDatabase();

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        protected DbContext(bool isBeginTransaction = false)
        {
            _serviceScope = ServiceProvider?.CreateScope();
            _connectFactory = _serviceScope?.ServiceProvider.GetRequiredService<IConnectFactory>();
            _transactionManager = _serviceScope?.ServiceProvider.GetRequiredService<ITransactionManager>();
            _sqlBuilder = _serviceScope?.ServiceProvider.GetRequiredService<ISqlBuilder>();
            _isBeginTransaction = isBeginTransaction;
            BenginTransaction();
        }

        /// <summary>
        /// Disposes the async.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null && !_isRollbackOrCommit)
                await _transaction.CommitAsync();
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
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _isRollbackOrCommit = true;
            }
            throw new Exception("事务没有开启");
        }

        /// <summary>
        /// Rollbacks the.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _isRollbackOrCommit = true;
            }
            throw new Exception("事务没有开启");
        }

        /// <summary>
        /// Excutes the query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>A list of TS.</returns>
        public virtual async Task<IEnumerable<T>> ExcuteQueryAsync<T>(params ISql[] sqls) where T : class, new()
        {
            var defaultDatabase = _connectFactory.GetDatabase();
            var group = sqls.GroupBy(t => t.Database).ToDictionary(t => string.IsNullOrWhiteSpace(t.Key) ? defaultDatabase : t.Key, t => t.ToArray());
            var list = new List<T>();
            var taskList = new List<Task<IEnumerable<T>>>();
            foreach (var item in group)
            {
                taskList.Add(ExcuteQueryAsync<T>(item.Key, item.Value));
            }
            var rList = await Task.WhenAll(taskList);
            foreach (var item in rList)
            {
                list.AddRange(item);
            }
            return list;
        }

        /// <summary>
        /// Excutes the query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>A list of TS.</returns>
        public virtual async Task<IEnumerable<T>> ExcuteQueryAsync<T>(string database, params ISql[] sqls) where T : class, new()
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync(database);
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
            var defaultDatabase = _connectFactory.GetDatabase();
            var group = sqls.GroupBy(t => t.Database).ToDictionary(t => string.IsNullOrWhiteSpace(t.Key) ? defaultDatabase : t.Key, t => t.ToArray());
            var r = 0;
            var taskList = new List<Task<int>>();
            foreach (var item in group)
            {
                taskList.Add(ExcuteNoQueryAsync(item.Key, item.Value));
            }
            var rList = await Task.WhenAll(taskList);
            foreach (var item in rList)
            {
                r += item;
            }
            return r;
        }

        /// <summary>
        /// Excutes the no query.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An int.</returns>
        public virtual async Task<int> ExcuteNoQueryAsync(string database, params ISql[] sqls)
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync(database);
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
            var defaultDatabase = _connectFactory.GetDatabase();
            var group = sqls.GroupBy(t => t.Database).ToDictionary(t => string.IsNullOrWhiteSpace(t.Key) ? defaultDatabase : t.Key, t => t.ToArray());
            var taskList = new List<Task<object>>();
            foreach (var item in group)
            {
                taskList.Add(ExecuteScalarAsync(item.Key, item.Value));
            }
            var rList = await Task.WhenAll(taskList);
            return rList.LastOrDefault();
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="sqls">The sqls.</param>
        /// <returns>An object.</returns>
        public virtual async Task<object> ExecuteScalarAsync(string database, params ISql[] sqls)
        {
            var sql = _sqlBuilder.Build(sqls);
            var command = await CreateDbCommandAsync(database);
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return await command.ExecuteScalarAsync();
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
        private async Task<DbCommand> CreateDbCommandAsync(string databaseName)
        {
            var connect = await _connectFactory.CreateAndOpenConnectionAsync();
            if (!string.Equals(databaseName, connect.Database) && !string.IsNullOrWhiteSpace(databaseName))
            {
                connect.ChangeDatabase(databaseName);
            }
            var command = connect.CreateCommand();
            command.Transaction = await _transactionManager.GetDbTransactionAsync();
            return command;
        }
    }
}