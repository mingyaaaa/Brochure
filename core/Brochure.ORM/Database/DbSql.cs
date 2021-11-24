using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Brochure.Core.Extenstions;
using Brochure.Extensions;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Querys;
using Brochure.ORM.Utils;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSql"/> class.
        /// </summary>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="visitProvider">The visit provider.</param>
        /// <param name="queryBuilder">The query builder.</param>
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
        public virtual ISql GetDeleteSql<T>(Expression<Func<T, bool>> whereFunc)
        {
            ParmsSqlResult result = new ParmsSqlResult();
            var whereSql = string.Empty;
            var parms = new List<IDbDataParameter>();
            var tableTypeDic = new Dictionary<int, Type>();
            if (whereFunc != null)
            {
                var whereVisitor = visitProvider.Builder<WhereVisitor>();
                whereVisitor.Visit(whereFunc);
                whereSql = $"where { whereVisitor.GetSql()}";
                parms.AddRange(whereVisitor.GetParameters());
                tableTypeDic.AddRange(whereVisitor.GetTableDic());
            }
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"delete from {dbProvider.FormatFieldName(tableName)} {whereSql}";
            var stringBuild = new StringBuilder(sql);
            var t_p = queryBuilder.RenameParameter(stringBuild, parms);
            queryBuilder.RenameTableType(stringBuild, tableTypeDic);
            result.SQL = stringBuild.ToString();
            result.Parameters.AddRange(t_p);
            return result;
        }

        /// <summary>
        /// Gets the delete sql.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A Tuple.</returns>
        public virtual ISql GetDeleteSql<T>(IQuery query)
        {
            var result = new ParmsSqlResult();
            var whereSqlResult = query == null ? queryBuilder.Build(query) : new ParmsSqlResult();
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"delete from  {dbProvider.FormatFieldName(tableName)} {whereSqlResult.SQL}";
            result.SQL = sql;
            result.Parameters.AddRange(whereSqlResult.Parameters);
            return result;
        }

        /// <summary>
        /// Gets the insert sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A Tuple.</returns>
        public virtual ISql GetInsertSql<T>(T obj) where T : class
        {
            var result = new ParmsSqlResult();
            var doc = EntityUtil.AsTableRecord(obj);
            var tableName = TableUtlis.GetTableName<T>();
            var sql = $"insert into {dbProvider.FormatFieldName(tableName)}";
            var pams = new List<IDbDataParameter>();
            var fields = new List<string>();
            var valueList = new List<string>();
            foreach (var item in doc.Keys.ToList())
            {
                if (Option.IsUseParamers)
                {
                    var param = dbProvider.GetDbDataParameter();
                    param.ParameterName = Guid.NewGuid().ToString();
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
            result.SQL = sql;
            result.Parameters.AddRange(pams);
            return result;
        }

        /// <summary>
        /// Gets the update sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>A Tuple.</returns>
        public virtual ISql GetUpdateSql<T>(object obj, Expression<Func<T, bool>> whereFunc)
        {
            var result = new ParmsSqlResult();
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
                    param.ParameterName = Guid.NewGuid().ToString();
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
            var stringBuilder = new StringBuilder(sql);
            var t_p = queryBuilder.RenameParameter(stringBuilder, parms);
            result.SQL = stringBuilder.ToString();
            result.Parameters.AddRange(t_p);
            return result;
        }

        /// <summary>
        /// Gets the update sql.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="query">The query.</param>
        /// <returns>A Tuple.</returns>
        public virtual ISql GetUpdateSql<T>(object obj, IQuery query)
        {
            var result = new ParmsSqlResult();
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
            result.SQL = sql;
            result.Parameters.AddRange(parms);
            return result;
        }

        #region Database

        /// <summary>
        /// Gets the data base name count sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetDataBaseNameCountSql(string databaseName)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
            return result;
        }

        /// <summary>
        /// Gets the delete database sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetDeleteDatabaseSql(string databaseName)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"drop database {databaseName}";
            return result;
        }

        /// <summary>
        /// Gets the all table name.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetAllTableName(string databaseName)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"select table_name from information_schema.tables where table_schema='{databaseName}'";
            return result;
        }

        /// <summary>
        /// Gets the create database sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetCreateDatabaseSql(string databaseName)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"create database {databaseName}";
            return result;
        }

        /// <summary>
        /// Gets the all database name sql.
        /// </summary>
        /// <returns>A string.</returns>
        public virtual ISql GetAllDatabaseNameSql()
        {
            var result = new ParmsSqlResult();
            result.SQL = "select schema_name from information_schema.schemata";
            return result;
        }

        #endregion Database

        #region DataTable

        /// <summary>
        /// Gets the create table sql.
        /// </summary>
        /// <returns>A string.</returns>
        public virtual ISql GetCreateTableSql<T>()
        {
            var r = new ParmsSqlResult();
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
                var isSequence = item.GetCustomAttribute(typeof(SequenceAttribute), true) != null;
                if (isSequence)
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
            r.SQL = sql;
            return r;
        }

        /// <summary>
        /// Gets the table name count sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetTableNameCountSql(string tableName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
            return r;
        }

        /// <summary>
        /// Gets the update table name sql.
        /// </summary>
        /// <param name="oldName">The old name.</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetUpdateTableNameSql(string oldName, string newName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"alter table {oldName} rename {newName}";
            return r;
        }

        /// <summary>
        /// Gets the delete table sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetDeleteTableSql(string tableName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"drop table {tableName}";
            return r;
        }

        #endregion DataTable

        #region Column

        /// <summary>
        /// Gets the colums sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetColumsSql(string databaseName, string tableName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"select column_name from information_schema.columns where table_schema='{databaseName}' and table_name='{tableName}'";
            return r;
        }

        /// <summary>
        /// Gets the colums name count sql.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetColumsNameCountSql(string databaseName, string tableName, string columnName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"select COUNT(1) from information_schema.columns WHERE table_schema='{databaseName}' and table_name = '{tableName}' and column_name = '{columnName}'";
            return r;
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
        public virtual ISql GetRenameColumnNameSql(string tableName, string odlName, string newName, TypeCode typeCode, int length = 0)
        {
            var r = new ParmsSqlResult();
            var sqlType = _typeMap.GetSqlType(typeCode.ToString());
            if (length < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(length, typeCode);
            r.SQL = $"alter table {tableName} change column {odlName} {newName} {sqlType}{lengthStr}";
            return r;
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
        public virtual ISql GetUpdateColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var r = new ParmsSqlResult();
            var sqlType = _typeMap.GetSqlType(typeCode.ToString());
            var sql = $"alter table {tableName} modify {columnName} {sqlType}";
            if (length < 0)
                throw new ArgumentException("长度不能为小于0");
            var lengthStr = GetLengthStr(length, typeCode);
            sql = $"{sql}{lengthStr}";
            if (isNotNull)
                sql = $"{sql} not null";
            r.SQL = sql;
            return r;
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
        public virtual ISql GetAddllColumnSql(string tableName, string columnName, TypeCode typeCode, bool isNotNull, int length = 0)
        {
            var r = new ParmsSqlResult();
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
            r.SQL = sql;
            return r;
        }

        /// <summary>
        /// Gets the delete column sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetDeleteColumnSql(string tableName, string columnName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"alter table {tableName} drop column {columnName}";
            return r;
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
        public virtual ISql GetCreateIndexSql(string tableName, string[] columnNames, string indexName, string sqlIndex)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"create {sqlIndex} {indexName} on {tableName}({string.Join(",", columnNames)})";
            return r;
        }

        /// <summary>
        /// Gets the delete index sql.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="indexName">The index name.</param>
        /// <returns>A string.</returns>
        public virtual ISql GetDeleteIndexSql(string tableName, string indexName)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"drop index {indexName} on {tableName}";
            return r;
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