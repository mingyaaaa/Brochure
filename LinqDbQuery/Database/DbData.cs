using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AspectCore.Injector;
using Brochure.Core;
using LinqDbQuery.Extensions;
using LinqDbQuery.Visitors;

namespace LinqDbQuery.Database
{
    public abstract class DbData
    {
        protected DbData (DbQueryOption dbOption, DbSql dbSql)
        {
            Option = dbOption;
            this._dbSql = dbSql;
        }

        protected DbQueryOption Option;
        private readonly DbSql _dbSql;

        public virtual int Insert<T> (T obj)
        {
            var sqlTuple = _dbSql.GetInsertSql (obj);
            var sql = sqlTuple.Item1;
            var parms = sqlTuple.Item2;
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
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
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.CommandText = sqlList.Join (";", null);
            command.Parameters.AddRange (parms);
            return command.ExecuteNonQuery ();
        }

        public virtual int Update<T> (object obj, Expression<Func<T, bool>> whereFunc)
        {
            var sqlTuple = _dbSql.GetUpdateSql<T> (obj, whereFunc);
            var sql = sqlTuple.Item1;
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (sqlTuple.Item2);
            return command.ExecuteNonQuery ();
        }

        public virtual int Delete<T> (Expression<Func<T, bool>> whereFunc)
        {
            var tuple = _dbSql.GetDeleteSql<T> (whereFunc);
            var sql = tuple.Item1;
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (tuple.Item2);
            return command.ExecuteNonQuery ();
        }
    }
}