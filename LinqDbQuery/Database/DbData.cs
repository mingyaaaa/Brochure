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
            var list = objs.ToList ();
            var tableName = TableUtlis.GetTableName<T> ();
            var sqlList = new List<string> ();
            var parms = new List<IDbDataParameter> ();
            var i = 0;
            foreach (var item in list)
            {
                var record = item.As<IRecord> ();
                var sqlTuple = GetInsertSql (tableName, record);
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
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (whereFunc != null)
            {
                var whereVisitor = new WhereVisitor (Option.DbProvider);
                whereVisitor.Visit (whereFunc);
                whereSql = whereVisitor.GetSql ().ToString ();
                parms.AddRange (whereVisitor.GetParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var sqlTuple = GetUpdateSql (tableName, obj.As<IRecord> ());
            var sql = sqlTuple.Item1 + whereSql;
            parms.AddRange (sqlTuple.Item2);
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (parms);
            return command.ExecuteNonQuery ();
        }

        public virtual int Delete<T> (Expression<Func<T, bool>> whereFunc)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (whereFunc != null)
            {
                var whereVisitor = new WhereVisitor (Option.DbProvider);
                whereVisitor.Visit (whereFunc);
                whereSql = whereVisitor.GetSql ().ToString ();
                parms.AddRange (whereVisitor.GetParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var sql = $"delete from {tableName} {whereSql}";
            var connect = Option.DbProvider.GetDbConnection ();
            var command = connect.CreateCommand ();
            command.CommandText = sql;
            command.Parameters.AddRange (parms);
            return command.ExecuteNonQuery ();
        }

        private Tuple<string, List<IDbDataParameter>> GetInsertSql (string tableName, IRecord doc)
        {
            var sql = $"insert into {tableName} ";
            var pams = new List<IDbDataParameter> ();
            var fields = doc.Keys.ToList ();
            var valueList = new List<string> ();
            foreach (var item in fields)
            {
                if (Option.IsUseParamers)
                {
                    var paramsKey = $"{Option.DbProvider.GetParamsSymbol()}{item}";
                    var param = Option.DbProvider.GetDbDataParameter ();
                    param.ParameterName = item;
                    param.Value = doc[item];
                    pams.Add (param);
                }
                else
                {
                    valueList.Add (Option.DbProvider.GetObjectType (doc[item]));
                }
            }
            if (Option.IsUseParamers)
                sql = $"{sql}({fields.Join(",")}) values({pams.Join(",",t=>t.ParameterName)})";
            else
                sql = $"{sql}({fields.Join(",")}) values({valueList.Join(",")})";
            return Tuple.Create (sql, pams);
        }

        private Tuple<string, List<IDbDataParameter>> GetUpdateSql (string tableName, IRecord doc)
        {
            var pams = new List<IDbDataParameter> ();
            var sql = $"update {tableName} set ";
            var fieldList = new List<string> ();
            foreach (var item in doc.Keys.ToList ())
            {
                var fieldStr = string.Empty;
                if (Option.IsUseParamers)
                {
                    var param = Option.DbProvider.GetDbDataParameter ();
                    param.ParameterName = $"{Option.DbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"{item}={param.ParameterName}";
                    pams.Add (param);
                }
                else
                {
                    fieldStr = $"{item}={Option.DbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Add (fieldStr);
            }
            sql = $"{sql}{fieldList.Join(",")}";
            return Tuple.Create (sql, pams);
        }
    }
}