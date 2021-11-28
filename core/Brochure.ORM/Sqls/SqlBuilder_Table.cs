using Brochure.ORM.Atrributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Brochure.ORM
{
    public partial class SqlBuilder
    {
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="createTableSql">The create table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildCreateTable(CreateTableSql createTableSql)
        {
            var type = createTableSql.TableType;
            var r = new ParmsSqlResult();
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
        /// Gets the table count.
        /// </summary>
        /// <param name="countTableSql">The count table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildTableCount(CountTableSql countTableSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{countTableSql.TableName}'";
            return r;
        }

        /// <summary>
        /// Updates the table name.
        /// </summary>
        /// <param name="updateTableNameSql">The update table name sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildUpdateTableName(UpdateTableNameSql updateTableNameSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"alter table {updateTableNameSql.OldName} rename {updateTableNameSql.NewName}";
            return r;
        }

        /// <summary>
        /// Deletes the table.
        /// </summary>
        /// <param name="deleteTableSql">The delete table sql.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildDeleteTable(DeleteTableSql deleteTableSql)
        {
            var r = new ParmsSqlResult();
            r.SQL = $"drop table {deleteTableSql.TableName}";
            return r;
        }
    }
}