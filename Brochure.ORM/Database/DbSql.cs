using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Extensions;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Visitors;
namespace Brochure.ORM
{
    public abstract class DbSql
    {
        protected DbOption Option;
        private readonly TypeMap _typeMap;
        protected readonly IDbProvider dbProvider;

        protected DbSql (IDbProvider dbProvider, DbOption dbOption)
        {
            this.Option = dbOption;
            _typeMap = dbProvider.GetTypeMap ();
            this.dbProvider = dbProvider;
        }

        public virtual Tuple<string, List<IDbDataParameter>> GetDeleteSql<T> (Expression<Func<T, bool>> whereFunc)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (whereFunc != null)
            {
                var whereVisitor = new WhereVisitor (dbProvider);
                whereVisitor.Visit (whereFunc);
                whereSql = whereVisitor.GetSql ().ToString ();
                parms.AddRange (whereVisitor.GetParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var sql = $"delete from [{tableName}] {whereSql}";
            return Tuple.Create (sql, parms);
        }

        public virtual Tuple<string, List<IDbDataParameter>> GetDeleteSql<T> (IQuery query)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (query != null)
            {
                whereSql = query.GetWhereSql ();
                parms.AddRange (query.GetDbDataParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var sql = $"delete from [{tableName}] {whereSql}";
            return Tuple.Create (sql, parms);
        }
        public virtual Tuple<string, List<IDbDataParameter>> GetInsertSql<T> (T obj)
        {
            var doc = obj.As<IRecord> ();
            var tableName = TableUtlis.GetTableName<T> ();
            var sql = $"insert into [{tableName}]";
            var pams = new List<IDbDataParameter> ();
            var fields = new List<string> ();
            var valueList = new List<string> ();
            foreach (var item in doc.Keys.ToList ())
            {
                if (dbProvider.IsUseParamers)
                {
                    var paramsKey = $"{dbProvider.GetParamsSymbol()}{item}";
                    var param = dbProvider.GetDbDataParameter ();
                    param.ParameterName = item;
                    param.Value = doc[item];
                    if (param.Value != null)
                    {
                        fields.Add ($"[{item}]");
                        pams.Add (param);
                    }
                }
                else
                {
                    var t_value = dbProvider.GetObjectType (doc[item]);
                    if (t_value != null)
                    {
                        valueList.Add (t_value);
                        fields.Add ($"[{item}]");
                    }
                }
            }
            if (dbProvider.IsUseParamers)
                sql = $"{sql}({fields.Join(",")}) values({pams.Join(",",t=>t.ParameterName)})";
            else
                sql = $"{sql}({fields.Join(",")}) values({valueList.Join(",")})";
            return Tuple.Create (sql, pams);
        }

        public virtual Tuple<string, List<IDbDataParameter>> GetUpdateSql<T> (object obj, Expression<Func<T, bool>> whereFunc)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (whereFunc != null)
            {
                var whereVisitor = new WhereVisitor (dbProvider);
                whereVisitor.Visit (whereFunc);
                whereSql = whereVisitor.GetSql ().ToString ();
                parms.AddRange (whereVisitor.GetParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var doc = obj.As<IRecord> ();
            var sql = $"update [{tableName}] set ";
            var fieldList = new List<string> ();
            foreach (var item in doc.Keys.ToList ())
            {
                var fieldStr = string.Empty;
                if (dbProvider.IsUseParamers)
                {
                    var param = dbProvider.GetDbDataParameter ();
                    param.ParameterName = $"{dbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"[{item}]={param.ParameterName}";
                    parms.Add (param);
                }
                else
                {
                    fieldStr = $"[{item}]={dbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Add (fieldStr);
            }
            sql = $"{sql}{fieldList.Join(",")} {whereSql}";
            return Tuple.Create (sql, parms);
        }
        public virtual Tuple<string, List<IDbDataParameter>> GetUpdateSql<T> (object obj, IQuery query)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter> ();
            if (query != null)
            {
                whereSql = query.GetWhereSql ();
                parms.AddRange (query.GetDbDataParameters ());
            }
            var tableName = TableUtlis.GetTableName<T> ();
            var doc = obj.As<IRecord> ();
            var sql = $"update [{tableName}] set ";
            var fieldList = new List<string> ();
            foreach (var item in doc.Keys.ToList ())
            {
                var fieldStr = string.Empty;
                if (dbProvider.IsUseParamers)
                {
                    var param = dbProvider.GetDbDataParameter ();
                    param.ParameterName = $"{dbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"[{item}]={param.ParameterName}";
                    parms.Add (param);
                }
                else
                {
                    fieldStr = $"[{item}]={dbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Add (fieldStr);
            }
            sql = $"{sql}{fieldList.Join(",")} {whereSql}";
            return Tuple.Create (sql, parms);
        }

        #region Database

        public virtual string GetDataBaseNameCountSql (string databaseName)
        {
            return $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
        }

        public virtual string GetDeleteDatabaseSql (string databaseName)
        {
            return $"drop database {databaseName}";
        }

        public virtual string GetAllTableName (string databaseName)
        {
            return $"select table_name from information_schema.tables where table_schema='{databaseName}'";
        }

        public virtual string GetCreateDatabaseSql (string databaseName)
        {
            return $"create database {databaseName}";
        }

        public virtual string GetAllDatabaseNameSql ()
        {
            return "select schema_name from information_schema.schemata";
        }
        #endregion

        #region DataTable

        public virtual string GetCreateTableSql<T> ()
        {
            var type = typeof (T);
            var typeInfo = type.GetTypeInfo ();
            var props = typeInfo.GetRuntimeProperties ();
            var columSqls = new List<string> ();
            var tableName = TableUtlis.GetTableName<T> ();
            var keys = new List<string> ();
            foreach (var item in props)
            {
                var pType = item.PropertyType.Name;
                var aType = _typeMap.GetSqlType (pType);
                var columSql = $"{item.Name} {aType}";
                if (item.Name == "SequenceId")
                {
                    columSql = $"SequenceId int AUTO_INCREMENT";
                    columSqls.Add (columSql);
                    continue;
                }
                else if (item.Name == "Id")
                {
                    columSql = $"Id nvarchar(36)";
                    columSqls.Add (columSql);
                    continue;
                }
                var igAttribute = item.GetCustomAttribute (typeof (IngoreAttribute), true);
                if (igAttribute != null)
                    continue;
                if (item.GetCustomAttribute (typeof (ColumnAttribute), true) is ColumnAttribute columnAttribute)
                {
                    if (string.IsNullOrWhiteSpace (columnAttribute.Name)) { columSql = columSql.Replace (item.Name, columnAttribute.Name); }
                    if (pType == nameof (TypeCode.Double) || pType == nameof (TypeCode.Single))
                    {
                        var length = columnAttribute.Length == -1 ? 15 : columnAttribute.Length;
                        columSql = $"{columSql}({length},6)";
                    }
                    else if (pType == nameof (TypeCode.DateTime) || pType == nameof (TypeCode.Byte))
                    {
                        columSql = $"{columSql}";
                    }
                    else
                    {
                        var length = columnAttribute.Length == -1 ? 255 : columnAttribute.Length;
                        columSql = $"{columSql}({length})";
                    }
                }
                else
                {
                    if (pType == nameof (TypeCode.Double) || pType == nameof (TypeCode.Single))
                        columSql = $"{columSql}(15,6)";
                    else if (pType == nameof (TypeCode.DateTime) || pType == nameof (TypeCode.Byte) || pType == nameof (TypeCode.Int32))
                        columSql = $"{columSql}";
                    else
                        columSql = $"{columSql}({255})";
                }
                if (item.GetCustomAttribute (typeof (KeyAttribute), true) is KeyAttribute _)
                {
                    keys.Add (item.Name);
                }
                var isNotNullAttribute = item.GetCustomAttribute (typeof (NotNullAttribute), true);
                if (isNotNullAttribute != null)
                    columSql = $"{columSql} not null";
                columSqls.Add (columSql);
            }
            if (keys.Count == 0) { columSqls.Add ($"PRIMARY KEY ( Id )"); }
            else if (keys.Count == 1) { columSqls.Add ($"PRIMARY KEY ( {keys[0]} )"); }
            else { throw new NotSupportedException ("暂时不支持联合主键，请使用联合唯一索引代替"); }
            if (string.IsNullOrWhiteSpace (tableName))
                throw new Exception ("当前TabelName的值为null 无法创建表");
            var sql = $@"create table {tableName}({string.Join(",", columSqls)})";
            return sql;
        }

        public virtual string GetTableNameCountSql (string tableName)
        {
            return $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
        }

        public virtual string GetUpdateTableNameSql (string oldName, string newName)
        {
            return $"alter table {oldName} rename {newName}";
        }

        public virtual string GetDeleteTableSql (string tableName)
        {
            return $"drop table {tableName}";
        }
        #endregion

        #region Column

        public virtual string GetColumsSql (string databaseName, string tableName)
        {
            return $"select column_name from information_schema.columns where table_schema='{databaseName}' and table_name='{tableName}'";
        }

        public virtual string GetColumsNameCountSql (string databaseName, string tableName, string columnName)
        {
            return $"select COUNT(1) from information_schema.columns WHERE table_schema='{databaseName}' and table_name = '{tableName}' and column_name = '{columnName}'";
        }

        public virtual string GetRenameColumnNameSql (string tableName, string odlName, string newName, TypeCode typeCode, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType (typeCode.ToString ());
            if (length < 0)
                throw new ArgumentException ("长度不能为小于0");
            var lengthStr = GetLengthStr (length, typeCode);
            return $"alter table {tableName} change column {odlName} {newName} {sqlType}{lengthStr}";
        }

        public virtual string GetUpdateColumnSql (string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType (typeCode.ToString ());
            var sql = $"alter table {tableName} modify {columnName} {sqlType}";
            if (length < 0)
                throw new ArgumentException ("长度不能为小于0");
            var lengthStr = GetLengthStr (length, typeCode);
            sql = $"{sql}{lengthStr}";
            if (isNotNull)
                sql = $"{sql} not null";
            return sql;
        }

        public virtual string GetAddllColumnSql (string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType (typeCode.ToString ());
            var sql = $"alter table {tableName} add column {columnName} {sqlType}";
            if (length < 0)
                throw new ArgumentException ("长度不能为小于0");
            var lengthStr = GetLengthStr (length, typeCode);
            sql = $"{sql}{lengthStr}";
            if (isNotNull)
            {
                sql = $"{sql} not null";
            }
            return sql;
        }

        public virtual string GetDeleteColumnSql (string tableName, string columnName)
        {
            return $"alter table {tableName} drop column {columnName}";
        }

        #endregion

        #region Index
        public virtual string GetCreateIndexSql (string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            return $"create {sqlIndex} {indexName} on {tableName}({string.Join(",", columnNames)})";
        }

        public virtual string GetDeleteIndexSql (string tableName, string indexName)
        {
            return $"drop index {indexName} on {tableName}";
        }
        #endregion

        private string GetLengthStr (int length, TypeCode code)
        {

            if (code == TypeCode.Double || code == TypeCode.Single)
            {
                length = length <= 0 ? 15 : length;
                return $"({length},6)";
            }
            else if (code != TypeCode.DateTime &&
                code != TypeCode.Byte &&
                code != TypeCode.Boolean
            )
            {
                length = length <= 0 ? 255 : length;
                return $"({length})";
            }
            return string.Empty;
        }
    }
}