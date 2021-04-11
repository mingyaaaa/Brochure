using System;
using System.Collections.Generic;
using System.Reflection;
using Brochure.Abstract;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Visitors;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbSql : DbSql
    {
        public MySqlDbSql(IDbProvider dbProvider, DbOption dbOption, IVisitProvider visitProvider) : base(dbProvider, dbOption, visitProvider) { }

        public override string GetCreateTableSql<T>()
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
                    else if (pType == nameof(TypeCode.DateTime) || pType == nameof(TypeCode.Byte) || pType == nameof(TypeCode.Int32)
                        || pType == nameof(TypeCode.Decimal))
                        columSql = $"{columSql}";
                    else
                        columSql = $"{columSql}({64})";
                }
                if (item.GetCustomAttribute(typeof(KeyAttribute), true) is KeyAttribute _)
                {
                    keys.Add(item.Name);
                }
                var isNotNullAttribute = item.GetCustomAttribute(typeof(NotNullAttribute), true);
                if (isNotNullAttribute != null)
                    columSql = $"{columSql} not null";
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
    }
}