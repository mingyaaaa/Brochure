using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brochure.Core.Abstracts;
using Brochure.Core.Atrributes;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Querys;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Server.Attributes;
using Brochure.Core.Server.Interfaces;
using Brochure.Core.System;
using Brochure.Server.MySql.Implements;
using MySql.Data.MySqlClient;

namespace Brochure.Server.MySql.Utils
{
    public static class DbUtil
    {
        public static string GetCreateTableSql<T> () where T : EntityBase
        {
            var type = typeof (T);
            var typeInfo = type.GetTypeInfo ();
            var props = typeInfo.GetRuntimeProperties ();
            var columSqls = new List<string> ();
            var typeMap = AbSingleton.GetInstance<AbTypeMap> ();
            var tableName = GetTableName<T> ();
            foreach (var item in props)
            {
                var pType = item.PropertyType.Name;
                var aType = typeMap.GetSqlType (pType);
                var columSql = $"{item.Name} {aType}";
                if (item.Name == "SequenceId")
                {
                    columSql = $"SequenceId int AUTO_INCREMENT";
                    columSqls.Add (columSql);
                    continue;
                }
                else if (item.Name == "Id")
                {
                    columSql = $"Id nvarchar(36) not null UNIQUE";
                    columSqls.Add (columSql);
                    continue;
                }
                var igAttribute = item.GetCustomAttribute (typeof (IngoreAttribute), true);
                if (igAttribute != null)
                    continue;
                var lengthAttribute = item.GetCustomAttribute (typeof (LengthAttribute), true) as LengthAttribute;
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

                var isNotNullAttribute = item.GetCustomAttribute (typeof (IsNotNullAttribut), true);
                if (isNotNullAttribute != null)
                    columSql = $"{columSql} not null";
                columSqls.Add (columSql);
            }
            columSqls.Add ($"PRIMARY KEY ( SequenceId )");
            if (string.IsNullOrWhiteSpace (tableName))
                throw new Exception ("当前TabelName的值为null 无法创建表");
            var sql = $@"create table {tableName}({string.Join(",",columSqls)})";
            return sql;
        }
        public static IDbParams GetInsertSql (string tableName, IRecord doc)
        {
            var sql = $"insert into {tableName}";
            var param = new MySqlDbParams ();
            param.Params = new Record ();
            var fields = doc.Keys.ToList ();
            foreach (var item in doc.Keys.ToList ())
            {
                var paramsKey = $"{param.ParamSymbol}{item}";
                param.Params[paramsKey] = doc[item];
            }
            param.Sql = $"{sql}{string.Join(",",fields)} set ({string.Join(",",param.Params.Keys.ToList())})";
            return param;
        }
        public static IDbParams GetUpdateSql (string tableName, IRecord doc)
        {
            var param = new MySqlDbParams ();
            param.Params = new Record ();
            var sql = $"update {tableName} set ";
            var fieldList = new List<string> ();
            foreach (var item in doc.Keys.ToList ())
            {
                var paramsKey = $"{param.ParamSymbol}{item}";
                var fieldStr = $"{item}=paramsKey";
                fieldList.Add (fieldStr);
                param.Params[paramsKey] = doc[item];
            }
            param.Sql = $"{sql}{string.Join(",",fieldList)}";
            return param;
        }

        public static MySqlParameter[] GetDbParams (IRecord param)
        {
            var result = new List<MySqlParameter> ();
            var sqlTypeMap = AbSingleton.GetInstance<AbTypeMap> ();
            foreach (var item in param.Keys.ToList ())
            {
                var mParams = new MySqlParameter (item, param[item]);
                result.Add (mParams);
            }
            return result.ToArray ();
        }

        public static MySqlParameter[] GetDbParams (Query query)
        {
            return null;
        }

        public static string GetTableName<T> () where T : EntityBase
        {
            var type = typeof (T);
            var tableAttribute = type.GetCustomAttribute (typeof (TableAttribute)) as TableAttribute;
            if (tableAttribute == null)
                throw new Exception ($"该{type.Name}没有使用TableAttribute，无法获取表数据");
            return tableAttribute.Name;
        }
    }
}