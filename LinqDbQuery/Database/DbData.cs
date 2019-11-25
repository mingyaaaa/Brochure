using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Brochure.Extensions;
using LinqDbQuery.Atrributes;
using LinqDbQuery.Extensions;
namespace LinqDbQuery.Database
{
    public abstract class DbData : IDisposable
    {
        private IDbConnection dbConnection;

        protected DbData (DbOption dbOption, DbSql dbSql, TransactionManager transactionManager)
        {
            Option = dbOption;
            this._dbSql = dbSql;
            dbConnection = Option.GetDbConnection ();
            this.transactionManager = transactionManager;
        }

        protected DbOption Option;
        private readonly DbSql _dbSql;
        private readonly TransactionManager transactionManager;

        [Transaction]
        public virtual int Insert<T> (T obj)
        {
            var sqlTuple = _dbSql.GetInsertSql (obj);
            var sql = sqlTuple.Item1;
            var parms = sqlTuple.Item2;
            var command = CreateDbCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (parms);
            return command.ExecuteNonQuery ();
        }

        public virtual int InsertMany<T> (IEnumerable<T> objs)
        {
            var list = objs.ToList ();
            var tableName = TableUtlis.GetTableName<T> ();
            var sqlList = new List<string> ();
            var parms = new List<IDbDataParameter> ();
            var i = 0;
            foreach (var item in list)
            {
                var sqlTuple = _dbSql.GetInsertSql (item);
                var sql = sqlTuple.Item1;
                if (i == 0)
                {
                    parms.AddRange (sqlTuple.Item2);
                    sqlList.Add (sql);
                }
                else
                {
                    foreach (var pp in sqlTuple.Item2)
                    {
                        var newParmsmName = pp.ParameterName + i;
                        sql.Replace (pp.ParameterName, newParmsmName);
                        pp.ParameterName = newParmsmName;
                        parms.Add (pp);
                    }
                }
                i++;
            }

            var command = CreateDbCommand ();
            command.CommandText = sqlList.Join (";", null);
            command.Parameters.AddRange (parms);
            return command.ExecuteNonQuery ();
        }

        public virtual int Update<T> (object obj, Expression<Func<T, bool>> whereFunc)
        {
            var sqlTuple = _dbSql.GetUpdateSql<T> (obj, whereFunc);
            var sql = sqlTuple.Item1;
            var command = CreateDbCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (sqlTuple.Item2);
            return command.ExecuteNonQuery ();
        }

        public virtual int Delete<T> (Expression<Func<T, bool>> whereFunc)
        {
            var tuple = _dbSql.GetDeleteSql<T> (whereFunc);
            var sql = tuple.Item1;
            var command = CreateDbCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (tuple.Item2);
            return command.ExecuteNonQuery ();
        }

        public void Dispose () { }

        private IDbCommand CreateDbCommand ()
        {
            var connect = Option.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.Transaction = transactionManager.GetDbTransaction ();
            return command;
        }
    }
}