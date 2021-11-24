using Brochure.Abstract;
using Brochure.Extensions;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Extensions;
using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Database
{
    /// <summary>
    /// The db data.
    /// </summary>
    public abstract class DbData : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbData"/> class.
        /// </summary>
        /// <param name="dbOption">The db option.</param>
        /// <param name="dbSql">The db sql.</param>
        /// <param name="transactionManager">The transaction manager.</param>
        /// <param name="connectFactory">The connect factory.</param>
        /// <param name="objectFactory">The object factory.</param>
        protected DbData(DbOption dbOption, DbSql dbSql, ITransactionManager transactionManager, IConnectFactory connectFactory, IObjectFactory objectFactory, IQueryBuilder queryBuilder)
        {
            Option = dbOption;
            this._dbSql = dbSql;
            this._transactionManager = transactionManager;
            this._connectFactory = connectFactory;
            _objectFactory = objectFactory;
            this.queryBuilder = queryBuilder;
        }

        protected DbOption Option;
        private readonly DbSql _dbSql;
        private readonly ITransactionManager _transactionManager;
        private readonly IConnectFactory _connectFactory;
        private readonly IObjectFactory _objectFactory;
        private readonly IQueryBuilder queryBuilder;

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int Insert<T>(T obj) where T : class
        {
            return InsertMany<T>(new List<T>() { obj });
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int InsertMany<T>(IEnumerable<T> objs) where T : class
        {
            var list = objs.ToList();
            var sqlList = new List<ISql>();
            foreach (var item in list)
            {
                var t_sql = _dbSql.GetInsertSql(item);
                sqlList.Add(t_sql);
            }
            var sql = queryBuilder.Build(sqlList);
            var command = CreateDbCommand();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int Update<T>(object obj, Expression<Func<T, bool>> whereFunc)
        {
            var sqlTuple = _dbSql.GetUpdateSql<T>(obj, whereFunc);
            var sql = sqlTuple.SQL;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(sqlTuple.Parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int Update<T>(object obj, IQuery query)
        {
            var sqlTuple = _dbSql.GetUpdateSql<T>(obj, query);
            var sql = sqlTuple.SQL;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(sqlTuple.Parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int Delete<T>(Expression<Func<T, bool>> whereFunc)
        {
            var tuple = _dbSql.GetDeleteSql<T>(whereFunc);
            var sql = tuple.SQL;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(tuple.Parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int Delete<T>(IQuery query)
        {
            var tuple = _dbSql.GetDeleteSql<T>(query);
            var command = CreateDbCommand();
            command.CommandText = tuple.SQL;
            command.Parameters.AddRange(tuple.Parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public virtual IEnumerable<T> Find<T>(IQuery<T> query) where T : class, new()
        {
            var queryResult = queryBuilder.Build(query);
            var parms = queryResult.Parameters;
            var connect = _connectFactory.CreateAndOpenConnection();
            var command = connect.CreateCommand();
            command.CommandText = queryResult.SQL;
            command.Parameters.AddRange(parms);
            using var reader = command.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public virtual IEnumerable<T> Find<T>(IQuery query) where T : class, new()
        {
            var queryResult = queryBuilder.Build(query);
            var parms = queryResult.Parameters;
            var connect = _connectFactory.CreateAndOpenConnection();
            var command = connect.CreateCommand();
            command.CommandText = queryResult.SQL;
            command.Parameters.AddRange(parms);
            using var reader = command.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
        }

        public virtual IEnumerable<T> ExcuteQuery<T>(params ISql[] sqls)
        {
            var sql = queryBuilder.Build(sqls);
            var command = CreateDbCommand();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            using var reader = command.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
        }

        public virtual IEnumerable<T> ExcuteQuery<T>(IEnumerable<ISql> sqls)
        {
            return ExcuteQuery<T>(sqls.ToArray());
        }

        public virtual int ExcuteNoQuery(params ISql[] sqls)
        {
            var sql = queryBuilder.Build(sqls);
            var command = CreateDbCommand();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return command.ExecuteNonQuery();
        }

        public virtual int ExcuteNoQuery(IEnumerable<ISql> sqls)
        {
            return ExcuteNoQuery(sqls.ToArray());
        }

        public virtual object ExecuteScalar(params ISql[] sqls)
        {
            var sql = queryBuilder.Build(sqls);
            var command = CreateDbCommand();
            command.CommandText = sql.SQL;
            command.Parameters.AddRange(sql.Parameters);
            return command.ExecuteScalar();
        }

        public virtual object ExecuteScalar(IEnumerable<ISql> sqls)
        {
            return ExecuteScalar(sqls.ToArray());
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Creates the db command.
        /// </summary>
        /// <returns>An IDbCommand.</returns>
        private IDbCommand CreateDbCommand()
        {
            var connect = _connectFactory.CreateAndOpenConnection();
            var command = connect.CreateCommand();
            command.Transaction = _transactionManager.GetDbTransaction();
            return command;
        }
    }
}