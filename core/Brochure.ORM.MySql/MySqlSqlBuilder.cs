using Brochure.Abstract;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db sql.
    /// </summary>
    public class MySqlSqlBuilder : SqlBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlSqlBuilder"/> class.
        /// </summary>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="visitProvider">The visit provider.</param>
        /// <param name="typeMap"></param>
        public MySqlSqlBuilder(IDbProvider dbProvider, DbOption dbOption, IVisitProvider visitProvider, TypeMap typeMap) : base(visitProvider, dbProvider, dbOption, typeMap)
        {
        }

        /// <summary>
        /// Builds the create table.
        /// </summary>
        /// <param name="createTableSql">The create table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected override ISqlResult BuildCreateTable(CreateTableSql createTableSql)
        {
            var r = new ParmsSqlResult();
            var type = createTableSql.TableType;
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            var columSqls = new List<string>();
            var tableName = TableUtlis.GetTableName(type);
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
                    var indexSql = "UNIQUE KEY(SequenceId)";
                    columSqls.Add(columSql);
                    columSqls.Add(indexSql);
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
                        var length = columnAttribute.Length < 36 ? 0 : columnAttribute.Length;
                        columSql = $"{columSql}({length})";
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
                    else if (pType == nameof(TypeCode.DateTime) || pType == nameof(TypeCode.Byte) || pType == nameof(TypeCode.Int32) || pType == nameof(TypeCode.Int64)
                        || pType == nameof(TypeCode.Decimal))
                        columSql = $"{columSql}";
                    else if (pType == typeof(Guid).Name)
                        columSql = $"{columSql}(36)";
                    else
                        columSql = $"{columSql}(255)";
                }
                if (item.GetCustomAttribute(typeof(KeyAttribute), true) is KeyAttribute _)
                {
                    keys.Add(item.Name);
                }
                var isNotNullAttribute = item.GetCustomAttribute(typeof(NotNullAttribute), true);
                if (item.PropertyType == typeof(string) && isNotNullAttribute == null)
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
            var sql = $@"create table if not exists {tableName}({string.Join(",", columSqls)})";
            r.SQL = sql;
            return r;
        }

        /// <summary>
        /// Builds the table count.
        /// </summary>
        /// <param name="countTableSql">The count table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected override ISqlResult BuildTableCount(CountTableSql countTableSql)
        {
            var result = new ParmsSqlResult();
            result.SQL = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{countTableSql.TableName}' and TABLE_SCHEMA ='{countTableSql.Database}'";
            return result;
        }

        /// <summary>
        /// Builders the if sql.
        /// </summary>
        /// <param name="ifSql">The if sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected override ISqlResult BuilderIfSql(IfSql ifSql)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Builders the exist.
        /// </summary>
        /// <param name="existSql">The exist sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected override ISqlResult BuilderExist(ExistSql existSql)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Builds the delete table.
        /// </summary>
        /// <param name="deleteTableSql">The delete table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected override ISqlResult BuildDeleteTable(DeleteTableSql deleteTableSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"drop table if exists {deleteTableSql.TableName}";
            return r;
        }
    }
}