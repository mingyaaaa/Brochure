using Brochure.Abstract;
using Brochure.Extensions;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.ORM
{
    /// <summary>
    /// The db sql.
    /// </summary>
    public abstract class DbSql
    {
        protected DbOption Option;
        protected readonly TypeMap _typeMap;
        protected readonly IDbProvider dbProvider;
        protected readonly IVisitProvider visitProvider;
        private readonly IQueryBuilder queryBuilder;

        protected DbSql(IDbProvider dbProvider, DbOption dbOption, IVisitProvider visitProvider, IQueryBuilder queryBuilder)
        {
            this.Option = dbOption;
            _typeMap = dbProvider.GetTypeMap();
            this.dbProvider = dbProvider;
            this.visitProvider = visitProvider;
            this.queryBuilder = queryBuilder;
        }

        /// <summary>
        /// Gets the delete sql.
        /// </summary>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>A Tuple.</returns>
        public virtual Tuple<string, List<IDbDataParameter>> GetDeleteSql<T>(Expression<Func<T, bool>> whereFunc)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter>();
            if (whereFunc != null)
            {
                var whereVisitor = visitProvider.Builder<WhereVisitor>();
                whereVisitor.Visit(whereFunc);
                whereSql = $"where { whereVisitor.GetSql()}";
                parms.AddRange(whereVisitor.GetParameters());
            }
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"delete from {dbProvider.FormatFieldName(tableName)} {whereSql}";
            return Tuple.Create(sql, parms);
        }

        /// <summary>
        /// Gets the delete sql.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Tuple.</returns>
        public virtual Tuple<string, List<IDbDataParameter>> GetDeleteSql<T>(IQuery query)
        {
            var whereSqlResult = query == null ? queryBuilder.Build(query) : new ParmsSqlResult();
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"delete from  {dbProvider.FormatFieldName(tableName)} {whereSqlResult.SQL}";
            return Tuple.Create(sql, whereSqlResult.Parameters);
        }

        /// <summary>
        /// Gets the insert sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Tuple.</returns>
        public virtual Tuple<string, List<IDbDataParameter>> GetInsertSql<T>(T obj)
        {
            var doc = obj.As<IRecord>();
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"insert into {dbProvider.FormatFieldName(tableName)}";
            var pams = new List<IDbDataParameter>();
            var fields = new List<string>();
            var valueList = new List<string>();
            foreach (var item in doc.Keys.ToList())
            {
                if (Option.IsUseParamers)
                {
                    var paramsKey = $"{dbProvider.GetParamsSymbol()}{item}";
                    var param = dbProvider.GetDbDataParameter();
                    param.ParameterName = paramsKey;
                    param.Value = doc[item];
                    if (param.Value != null)
                    {
                        fields.Add($"{dbProvider.FormatFieldName(item)}");
                        pams.Add(param);
                    }
                }
                else
                {
                    var t_value = dbProvider.GetObjectType(doc[item]);
                    if (t_value != null)
                    {
                        valueList.Add(t_value);
                        fields.Add($"{dbProvider.FormatFieldName(item)}");
                    }
                }
            }
            if (Option.IsUseParamers)
                sql = $"{sql}({fields.Join(",")}) values({pams.Join(",", t => t.ParameterName)})";
            else
                sql = $"{sql}({fields.Join(",")}) values({valueList.Join(",")})";
            return Tuple.Create(sql, pams);
        }

        /// <summary>
        /// Gets the update sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>A Tuple.</returns>
        public virtual Tuple<string, List<IDbDataParameter>> GetUpdateSql<T>(object obj, Expression<Func<T, bool>> whereFunc)
        {
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter>();
            if (whereFunc != null)
            {
                var whereVisitor = visitProvider.Builder<WhereVisitor>();
                whereVisitor.Visit(whereFunc);
                whereSql = whereVisitor.GetSql().ToString();
                parms.AddRange(whereVisitor.GetParameters());
            }
            var tableName = TableUtlis.GetTableName<T>();
            var doc = obj.As<IRecord>();
            var sql = $"update {dbProvider.FormatFieldName(tableName)} set ";
            var fieldList = new List<string>();
            foreach (var item in doc.Keys.ToList())
            {
                var fieldStr = string.Empty;
                if (Option.IsUseParamers)
                {
                    var param = dbProvider.GetDbDataParameter();
                    param.ParameterName = $"{dbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"{dbProvider.FormatFieldName(item)}={param.ParameterName}";
                    parms.Add(param);
                }
                else
                {
                    fieldStr = $"{dbProvider.FormatFieldName(item)}={dbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Add(fieldStr);
            }
            sql = $"{sql}{fieldList.Join(",")} {whereSql}";
            return Tuple.Create(sql, parms);
        }

        /// <summary>
        /// Gets the update sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>A Tuple.</returns>
        public virtual Tuple<string, List<IDbDataParameter>> GetUpdateSql<T>(object obj, IQuery query)
        {
            var whereSqlResult = query == null ? queryBuilder.Build(query) : new ParmsSqlResult();
            var tableName = TableUtlis.GetTableName<T>();
            var doc = obj.As<IRecord>();
            var sql = $"update {dbProvider.FormatFieldName(tableName)} set ";
            var fieldList = new List<string>();
            var parms = new List<IDbDataParameter>(whereSqlResult.Parameters);
            foreach (var item in doc.Keys.ToList())
            {
                var fieldStr = string.Empty;
                if (Option.IsUseParamers)
                {
                    var param = dbProvider.GetDbDataParameter();
                    param.ParameterName = $"{dbProvider.GetParamsSymbol()}{item}";
                    param.Value = doc[item];
                    fieldStr = $"{dbProvider.FormatFieldName(item)}={param.ParameterName}";
                    parms.Add(param);
                }
                else
                {
                    fieldStr = $"{dbProvider.FormatFieldName(item)}={dbProvider.GetObjectType(doc[item])}";
                }
                fieldList.Add(fieldStr);
            }
            sql = $"{sql}{fieldList.Join(",")} {whereSqlResult.SQL}";
            return Tuple.Create(sql, parms);
        }

        #region Database

        /// <summary>
        /// Gets the data base name count sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual string GetDataBaseNameCountSql(string databaseName)
        {
            return $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
        }

        /// <summary>
        /// Gets the delete database sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual string GetDeleteDatabaseSql(string databaseName)
        {
            return $"drop database {databaseName}";
        }

        /// <summary>
        /// Gets the all table name.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual string GetAllTableName(string databaseName)
        {
            return $"select table_name from information_schema.tables where table_schema='{databaseName}'";
        }

        /// <summary>
        /// Gets the create database sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual string GetCreateDatabaseSql(string databaseName)
        {
            return $"create database {databaseName}";
        }

        /// <summary>
        /// Gets the all database name sql.
        /// </summary>
        /// <returns>A string.</returns>
        public virtual string GetAllDatabaseNameSql()
        {
            return "select schema_name from information_schema.schemata";
        }

        #endregion Database

        #region DataTable

        /// <summary>
        /// Gets the create table sql.
        /// </summary>
        /// <returns>A string.</returns>
        public virtual string GetCreateTableSql<T>()
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            var columSqls = new List<string>();
            var tableName = TableUtlis.GetTableName<T>();
            var keys = new List<string>();
            foreach (var item in props)
            {
                var pType = item.PropertyType.Name;
                var isNullType = false;
                if (item.PropertyType.Name == typeof(Nullable<>).Name)
                {
                    isNullType = true;
                    pType = item.PropertyType.GenericTypeArguments[0].Name;
                }

                var aType = _typeMap.GetSqlType(pType);
                var columSql = $"{item.Name} {aType}";
                if (item.Name == "SequenceId")
                {
                    columSql = $"SequenceId int AUTO_INCREMENT";
                    columSqls.Add(columSql);
                    continue;
                }
                else if (item.Name == "Id")
                {
                    columSql = $"Id nvarchar(36)";
                    columSqls.Add(columSql);
                    continue;
                }
                var igAttribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (igAttribute != null)
                    continue;
                if (item.GetCustomAttribute(typeof(ColumnAttribute), true) is ColumnAttribute columnAttribute)
                {
                    if (string.IsNullOrWhiteSpace(columnAttribute.Name)) { columSql = columSql.Replace(item.Name, columnAttribute.Name); }
                    if (pType == nameof(TypeCode.Double) || pType == nameof(TypeCode.Single))
                    {
                        var length = columnAttribute.Length == -1 ? 15 : columnAttribute.Length;
                        columSql = $"{columSql}({length},6)";
                    }
                    else if (pType == nameof(TypeCode.DateTime) || pType == nameof(TypeCode.Byte))
                    {
                        columSql = $"{columSql}";
                    }
                    else if (pType == typeof(Guid).Name)
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
                    if (pType == nameof(TypeCode.Double) || pType == nameof(TypeCode.Single))
                        columSql = $"{columSql}(15,6)";
                    else if (pType == nameof(TypeCode.DateTime) || pType == nameof(TypeCode.Byte) || pType == nameof(TypeCode.Int32))
                        columSql = $"{columSql}";
                    else if (pType == typeof(Guid).Name)
                        columSql = $"{columSql}(36)";
                    else
                        columSql = $"{columSql}({255})";
                }
                if (item.GetCustomAttribute(typeof(KeyAttribute), true) is KeyAttribute _)
                {
                    keys.Add(item.Name);
                }
                var isNotNullAttribute = item.GetCustomAttribute(typeof(NotNullAttribute), true);
                if (item.PropertyType == typeof(string) && isNotNullAttribute == null)//如果是string类型默认是可null类型
                {
                    isNullType = true;
                }
                else if (isNotNullAttribute != null && isNullType)
                {
                    throw new NotSupportedException("可空类型不能被NotNull标记");
                }
                else if (!isNullType)
                {
                    columSql = $"{columSql} not null";
                }
                columSqls.Add(columSql);
            }
            if (keys.Count == 0) { columSqls.Add($"PRIMARY KEY ( Id )"); }
            else if (keys.Count == 1) { columSqls.Add($"PRIMARY KEY ( {keys[0]} )"); }
            else { throw new NotSupportedException("暂时不支持联合主键，请使用联合唯一索引代替"); }
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception("当前TabelName的值为null 无法创建表");
            var sql = $@"create table {tableName}({string.Join(",", columSqls)})";
            return sql;
        }

        /// <summary>
        /// Gets the table name count sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual string GetTableNameCountSql(string tableName)
        {
            return $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
        }

        /// <summary>
        /// Gets the update table name sql.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A string.</returns>
        public virtual string GetUpdateTableNameSql(string oldName, string newName)
        {
            return $"alter table {oldName} rename {newName}";
        }

        /// <summary>
        /// Gets the delete table sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual string GetDeleteTableSql(string tableName)
        {
            return $"drop table {tableName}";
        }

        #endregion DataTable

        #region Column

        /// <summary>
        /// Gets the colums sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual string GetColumsSql(string databaseName, string tableName)
        {
            return $"select column_name from information_schema.columns where table_schema='{databaseName}' and table_name='{tableName}'";
        }

        /// <summary>
        /// Gets the colums name count sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A string.</returns>
        public virtual string GetColumsNameCountSql(string databaseName, string tableName, string columnName)
        {
            return $"select COUNT(1) from information_schema.columns WHERE table_schema='{databaseName}' and table_name = '{tableName}' and column_name = '{columnName}'";
        }

        /// <summary>
        /// Gets the rename column name sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="odlName">The odl name.</param>
        /// <param name="newName">The new name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public virtual string GetRenameColumnNameSql(string tableName, string odlName, string newName, TypeCode typeCode, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType(typeCode.ToString());
            if (length < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(length, typeCode);
            return $"alter table {tableName} change column {odlName} {newName} {sqlType}{lengthStr}";
        }

        /// <summary>
        /// Gets the update column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public virtual string GetUpdateColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType(typeCode.ToString());
            var sql = $"alter table {tableName} modify {columnName} {sqlType}";
            if (length < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(length, typeCode);
            sql = $"{sql}{lengthStr}";
            if (isNotNull)
                sql = $"{sql} not null";
            return sql;
        }

        /// <summary>
        /// Gets the addll column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="isNotNull">If true, is not null.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public virtual string GetAddllColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var sqlType = _typeMap.GetSqlType(typeCode.ToString());
            var sql = $"alter table {tableName} add column {columnName} {sqlType}";
            if (length < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(length, typeCode);
            sql = $"{sql}{lengthStr}";
            if (isNotNull)
            {
                sql = $"{sql} not null";
            }
            return sql;
        }

        /// <summary>
        /// Gets the delete column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A string.</returns>
        public virtual string GetDeleteColumnSql(string tableName, string columnName)
        {
            return $"alter table {tableName} drop column {columnName}";
        }

        #endregion Column

        #region Index

        /// <summary>
        /// Gets the create index sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnNames">The column names.</param>
        /// <param name="indexName">The index name.</param>
        /// <param name="sqlIndex">The sql index.</param>
        /// <returns>A string.</returns>
        public virtual string GetCreateIndexSql(string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            return $"create {sqlIndex} {indexName} on {tableName}({string.Join(",", columnNames)})";
        }

        /// <summary>
        /// Gets the delete index sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <returns>A string.</returns>
        public virtual string GetDeleteIndexSql(string tableName, string indexName)
        {
            return $"drop index {indexName} on {tableName}";
        }

        #endregion Index

        /// <summary>
        /// Gets the length str.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="code">The code.</param>
        /// <returns>A string.</returns>
        private string GetLengthStr(int length, TypeCode code)
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