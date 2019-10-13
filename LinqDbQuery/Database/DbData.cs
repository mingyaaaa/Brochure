using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AspectCore.Injector;
using Brochure.Core;
using LinqDbQuery.Extensions;

namespace LinqDbQuery.Database
{
    public abstract class DbData
    {
        protected DbData (DbQueryOption dbOption)
        {
            Option = dbOption;
        }

        protected DbQueryOption Option;

        public virtual int Insert<T> (T obj)
        {
            var record = obj.As<IRecord> ();
            var tableName = TableUtlis.GetTableName<T> ();
            var sqlTuple = GetInsertSql (tableName, record);
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
            return -1;
        }

        public virtual T Update<T> (object obj, Expression<Func<T, bool>> Func)
        {
            return (T) (object) null;
        }

        public virtual int Delete<T> (Expression<Func<T, bool>> fun)
        {
            return -1;
        }

        private Tuple<string, List<IDbDataParameter>> GetInsertSql (string tableName, IRecord doc)
        {
            var sql = $"insert into {tableName} ";
            var pams = new List<IDbDataParameter> ();

            var fields = doc.Keys.ToList ();
            foreach (var item in fields)
            {
                var paramsKey = $"{Option.DbProvider.GetParamsSymbol()}{item}";
                var param = Option.DbProvider.GetDbDataParameter ();
                param.ParameterName = item;
                param.Value = doc[item];
                pams.Add (param);
            }
            sql = $"{sql}({string.Join(",", fields)}) values({string.Join(",", pams.Select(t=>t.ParameterName))})";
            return Tuple.Create (sql, pams);
        }
    }
}