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
        public virtual int Insert<T>(T obj)
        {
            var sqlTuple = _dbSql.GetInsertSql(obj);
            var sql = sqlTuple.Item1;
            var parms = sqlTuple.Item2;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(parms);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>An int.</returns>
        [Transaction]
        public virtual int InsertMany<T>(IEnumerable<T> objs)
        {
            var list = objs.ToList();
            var tableName = TableUtlis.GetTableName<T>();
            var sqlList = new List<string>();
            var parms = new List<IDbDataParameter>();
            var i = 0;
            foreach (var item in list)
            {
                var sqlTuple = _dbSql.GetInsertSql(item);
                var sql = sqlTuple.Item1;
                if (i == 0)
                {
                    parms.AddRange(sqlTuple.Item2);
                    sqlList.Add(sql);
                }
                else
                {
                    foreach (var pp in sqlTuple.Item2)
                    {
                        var newParmsmName = pp.ParameterName + i;
                        sql.Replace(pp.ParameterName, newParmsmName);
                        pp.ParameterName = newParmsmName;
                        parms.Add(pp);
                    }
                }
                i++;
            }

            var command = CreateDbCommand();
            command.CommandText = sqlList.Join(";", null);
            command.Parameters.AddRange(parms);
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
            var sql = sqlTuple.Item1;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(sqlTuple.Item2);
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
            var sql = sqlTuple.Item1;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(sqlTuple.Item2);
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
            var sql = tuple.Item1;
            var command = CreateDbCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(tuple.Item2);
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
            command.CommandText = tuple.Item1;
            command.Parameters.AddRange(tuple.Item2);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Queries the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A list of TS.</returns>
        public virtual IEnumerable<T> Query<T>(IQuery<T> query) where T : class, new()
        {
            var queryResult = queryBuilder.Build(query);
            var parms = queryResult.Parameters;
            var connect = _connectFactory.CreateAndOpenConnection();
            var command = connect.CreateCommand();
            command.CommandText = queryResult.SQL;
            command.Parameters.AddRange(parms);
            var reader = command.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
        }

        ///// <summary>
        ///// Queries the.
        ///// </summary>
        ///// <param name="query">The query.</param>
        ///// <returns>A list of TS.</returns>
        public virtual IEnumerable<T> Query<T>(IQuery query) where T : class, new()
        {
            var queryResult = queryBuilder.Build(query);
            var parms = queryResult.Parameters;
            var connect = _connectFactory.CreateAndOpenConnection();
            var command = connect.CreateCommand();
            command.CommandText = queryResult.SQL;
            command.Parameters.AddRange(parms);
            var reader = command.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                var t = _objectFactory.Create<T>(new DataReaderGetValue(reader));
                list.Add(t);
            }
            return list;
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
            var connect = _connectFactory.CreateConnection();
            var command = connect.CreateCommand();
            command.Transaction = _transactionManager.GetDbTransaction();
            return command;
        }
    }
}