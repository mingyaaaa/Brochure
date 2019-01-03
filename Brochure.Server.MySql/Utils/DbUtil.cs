using Brochure.Core;
using Brochure.Core.Server;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brochure.Server.MySql
{
    public static class DbUtil
    {
        public static string GetCreateTableSql<T>(TypeMap typeMap) where T : EntityBase
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            var columSqls = new List<string>();
            var tableName = GetTableName<T>();
            foreach (var item in props)
            {
                var pType = item.PropertyType.Name;
                var aType = typeMap.GetSqlType(pType);
                var columSql = $"{item.Name} {aType}";
                if (item.Name == "SequenceId")
                {
                    columSql = $"SequenceId int AUTO_INCREMENT";
                    columSqls.Add(columSql);
                    continue;
                }
                else if (item.Name == "Id")
                {
                    columSql = $"Id nvarchar(36) not null UNIQUE";
                    columSqls.Add(columSql);
                    continue;
                }
                var igAttribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (igAttribute != null)
                    continue;
                var lengthAttribute = item.GetCustomAttribute(typeof(LengthAttribute), true) as LengthAttribute;
                if (lengthAttribute != null)
                {
                    if (pType == BaseType.Double || pType == BaseType.Float)
                        columSql = $"{columSql}({lengthAttribute.Length},6)";
                    else if (pType == BaseType.DateTime || pType == BaseType.Byte)
                        columSql = $"{columSql}";
                    else
                        columSql = $"{columSql}({lengthAttribute.Length})";
                }
                else
                {
                    if (pType == BaseType.Double || pType == BaseType.Float)
                        columSql = $"{columSql}(15,6)";
                    else if (pType == BaseType.DateTime || pType == BaseType.Byte || pType == BaseType.Int)
                        columSql = $"{columSql}";
                    else
                        columSql = $"{columSql}({255})";
                }

                var isNotNullAttribute = item.GetCustomAttribute(typeof(NotNullAttribute), true);
                if (isNotNullAttribute != null)
                    columSql = $"{columSql} not null";
                columSqls.Add(columSql);
            }
            columSqls.Add($"PRIMARY KEY ( SequenceId )");
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception("当前TabelName的值为null 无法创建表");
            var sql = $@"create table {tableName}({string.Join(",", columSqls)})";
            return sql;
        }
        public static IDbParams GetInsertSql(string tableName, IRecord doc)
        {
            var sql = $"insert into {tableName} ";
            var param = new MySqlDbParams();
            param.Params = new Record();
            var fields = doc.Keys.ToList();
            foreach (var item in doc.Keys.ToList())
            {
                var paramsKey = $"{param.ParamSymbol}{item}";
                param.Params[paramsKey] = doc[item];
            }
            param.Sql = $"{sql}({string.Join(",", fields)}) values({string.Join(",", param.Params.Keys.ToList())})";
            return param;
        }
        public static IDbParams GetInsertManySql(string tableName, IRecord[] docs)
        {
            var sql = $"insert into {tableName} ";
            var param = new MySqlDbParams();
            param.Params = new Record();
            //需要验证传入得IRecord 字段是否一至
            if (docs == null || docs.Length == 0)
                throw new ArgumentException("参数错误");
            var fields = docs.First();
            var valuesSql = string.Empty;
            var listSql = new List<string>();
            foreach (var item in docs)
            {
                if (item.Keys.Except(fields.Keys).Count() != 0)
                    throw new ArgumentException("参数中字段不一致");
                var index = 0;
                var fieldlist = new List<string>();
                foreach (var field in item.Keys)
                {
                    var paramsKey = $"{param.ParamSymbol}{field}{index}";
                    param.Params[paramsKey] = item[field];
                    fieldlist.Add(paramsKey);
                    index++;
                    valuesSql = $"({string.Join(",", fieldlist)})";
                }
            }
            valuesSql = string.Join(",", listSql);
            param.Sql = $"{sql}({string.Join(",", fields)}) values {valuesSql}";
            return param;
        }
        public static IDbParams GetUpdateSql(string tableName, IRecord doc)
        {
            var param = new MySqlDbParams();
            param.Params = new Record();
            var sql = $"update {tableName} set ";
            var fieldList = new List<string>();
            foreach (var item in doc.Keys.ToList())
            {
                var paramsKey = $"{param.ParamSymbol}{item}0";
                var fieldStr = $"{item}={paramsKey}";
                fieldList.Add(fieldStr);
                param.Params[paramsKey] = doc[item];
            }
            param.Sql = $"{sql}{string.Join(",", fieldList)}";
            return param;
        }

        public static MySqlParameter[] GetMySqlParams(TypeMap typeMap, params IRecord[] paramArray)
        {
            var result = new List<MySqlParameter>();
            var sqlTypeMap = typeMap;
            foreach (var param in paramArray)
            {
                foreach (var item in param.Keys.ToList())
                {
                    var mParams = new MySqlParameter(item, param[item]);
                    result.Add(mParams);
                }
            }
            return result.ToArray();
        }

        public static string GetTableName<T>() where T : EntityBase
        {
            var type = typeof(T);
            var tableAttribute = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (tableAttribute == null)
                throw new Exception($"该{type.Name}没有使用TableAttribute，无法获取表数据");
            return tableAttribute.Name;
        }
    }
}
