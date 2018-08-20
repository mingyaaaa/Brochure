using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Core.Abstracts;
using Brochure.Core.Enums;
using Brochure.Core.Extends;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Model;
using Brochure.Core.Querys;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Server.core;
using Brochure.Core.Server.Enums;
using Brochure.Core.Server.Interfaces;
using Brochure.Core.Server.Models;
using Brochure.Server.MySql.Utils;
using MySql.Data.MySqlClient;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlDatabase : IDbTableBase, IDbDatabase, IDbData
    {
        private MySqlCommand _command;
        private ISqlParse _parse;
        internal MySqlDatabase (string tableName, MySqlCommand command) : this ()
        {
            TableName = tableName;
            _command = command;
        }
        internal MySqlDatabase (MySqlCommand command) : this ()
        {
            _command = command;
        }
        protected MySqlDatabase ()
        {
            _parse = new MySqlParse ();
        }
        public string TableName { get; }

        #region Table
        public async Task<long> RegistTableAsync<T> () where T : EntityBase
        {
            var sql = DbUtil.GetCreateTableSql<T> ();
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        public async Task<long> CreateTable<T> () where T : EntityBase
        {
            var tableName = DbUtil.GetTableName<T> ();
            var r = await IsExistTableAsync (tableName);
            if (r)
                return 1;
            var sql = DbUtil.GetCreateTableSql<T> ();
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistTableAsync (string tableName)
        {
            var sql = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
            _command.CommandText = sql;
            var rr = await _command.ExecuteScalarAsync ();
            return rr.As<int> () == 1;
        }
        public async Task<long> DeleteTableAsync (string tableName)
        {
            var sql = $"drop table  {tableName}";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateTableName (string olderName, string tableName)
        {
            var r = await IsExistTableAsync (olderName);
            if (!r)
                return 0;
            var sql = $"alter table {olderName} rename {tableName};";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        #endregion

        #region Database
        public async Task<long> CreateDataBaseAsync (string databaseName)
        {
            var isExist = await IsExistDataBaseAsync (databaseName);
            if (isExist)
                return 1;
            var sql = $"create database {databaseName}";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr;
        }
        public async Task<long> DeleteDataBaseAsync (string databaseName)
        {
            var sql = $"drop database {databaseName}";
            _command.CommandText = sql;
            //执行删除方法  删除成功 默认返回了0  改变返回的值；
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistDataBaseAsync (string databaseName)
        {
            var sql = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
            _command.CommandText = sql;
            var rr = await _command.ExecuteScalarAsync ();
            return rr.As<int> () == 1;
        }
        #endregion

        #region Data
        public async Task<long> AddColumnsAsync (string ColumnName, string typeName, bool IsNotNull)
        {
            var sql = $"alter table {TableName} add column {ColumnName} {typeName}";
            if (IsNotNull)
            {
                sql = $"{sql} not null";
            }
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        public async Task<long> CreateIndexAsync (string[] ColumnNames, string indexName, string sqlIndex)
        {
            var sql = $"create {sqlIndex} {indexName} on {TableName}({string.Join(",",ColumnNames)})";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }
        public async Task<long> DeleteColumnAsync (string ColumnName)
        {
            var sql = $"alter table {TableName} drop column {ColumnName}";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteIndexAsync (string indexName)
        {
            var sql = $"drop index {indexName} on {TableName}";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteManyAsync (Guid[] ids)
        {
            var query = Query.In ("Id", ids.Select (t => t.ToString ()).ToArray ());
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var sql = $"delete from {TableName} where 1=1 and {param.Sql}";
            _command.CommandText = sql;
            var mysqlParams = DbUtil.GetDbParams (param.Params);
            _command.Parameters.AddRange (mysqlParams);
            var r = await _command.ExecuteNonQueryAsync ();
            return r;
        }

        public async Task<long> DeleteManyAsync (Query query)
        {
            var sql = $"delete from {TableName} where 1=1 and {query}";
            _command.CommandText = sql;
            var r = await _command.ExecuteNonQueryAsync ();
            return r;
        }

        public async Task<long> DeleteOneAsync (Guid id)
        {
            var sql = $"delete from {TableName} where 1=1 and Id={id}";
            _command.CommandText = sql;
            var r = await _command.ExecuteNonQueryAsync ();
            return r;
        }
        public async Task<long> GetCountAsync (Query query)
        {
            var sql = $"select count(*) from {TableName} where 1=1 and {query}";
            _command.CommandText = sql;
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var mysqlParams = DbUtil.GetDbParams (param.Params);
            _command.Parameters.AddRange (mysqlParams);
            var rr = await _command.ExecuteScalarAsync ();
            return rr.As<int> ();
        }

        public async Task<IRecord> GetInfoAsync (Guid guid)
        {
            IRecord doc = null;
            var sql = $"select * from {TableName} where 1=1 and Id={guid}";
            _command.CommandText = sql;
            var dr = await _command.ExecuteReaderAsync ();
            if (dr.NextResult ())
            {
                doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    var value = dr[i];
                    doc[name] = value;
                }
            }
            return doc;
        }

        public async Task<List<IRecord>> GetListAsync (SearchParams searchParams)
        {
            List<IRecord> result = null;
            var orderDoc = searchParams.OrderField;
            var orderStr = string.Empty;
            foreach (var item in orderDoc.Keys)
            {
                var value = orderDoc[item].As<OrderType> ();
                orderStr = $"{orderStr}";
                if (value == OrderType.Desc)
                    orderStr = $"{orderStr} desc";
                else
                    orderStr = $"{orderStr} asc";
            }
            //分页 排序
            var count = searchParams.EndIndex - searchParams.StarIndex + 1;
            var parse = new QueryParse (_parse);
            var param = parse.Parse (searchParams.Filter);
            var mysqlParams = DbUtil.GetDbParams (param.Params);
            var sql = $"select * from {TableName} where 1=1 and {param.Sql} order by {orderStr} limit {searchParams.StarIndex},{count}";
            _command.CommandText = sql;
            _command.Parameters.AddRange (mysqlParams);
            var dr = await _command.ExecuteReaderAsync ();
            while (dr.Read ())
            {
                var doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    var value = dr[i];
                    doc[name] = value;
                }
                result.Add (doc);
            }
            return result;
        }
        public async Task<List<IRecord>> GetListGroupByAsync (List<Aggregate> aggregates, SearchParams searchParams, params string[] groupFields)
        {
            List<IRecord> result = null;
            var orderDoc = searchParams.OrderField;
            var orderStr = string.Empty;
            foreach (var item in orderDoc.Keys)
            {
                var value = orderDoc[item].As<OrderType> ();
                orderStr = $"{orderStr}";
                if (value == OrderType.Desc)
                    orderStr = $"{orderStr} desc";
                else
                    orderStr = $"{orderStr} asc";
            }
            var count = searchParams.EndIndex - searchParams.StarIndex + 1;
            var gfield = string.Join (",", groupFields);
            var parse = new QueryParse (_parse);
            var param = parse.Parse (searchParams.Filter);
            var mysqlParams = DbUtil.GetDbParams (param.Params);
            var sql = $"select {string.Join(",",aggregates)},{gfield} from {TableName} where 1=1 and {param.Sql} group by {gfield} order by {orderStr} limit {searchParams.StarIndex},{count}";
            _command.CommandText = sql;
            _command.Parameters.AddRange (mysqlParams);
            var dr = await _command.ExecuteReaderAsync ();
            while (dr.Read ())
            {
                var doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    var value = dr[i];
                    doc[name] = value;
                }
                result.Add (doc);
            }
            return result;

        }
        public async Task<long> InserManyAsync (IEnumerable<EntityBase> entities)
        {
            var tasks = new List<Task<long>> ();
            foreach (var item in entities.ToList ())
                tasks.Add (InserOneAsync (item));
            var rr = await Task.WhenAll (tasks);
            return rr.Sum (t => t);
        }

        public async Task<long> InserOneAsync (EntityBase entity)
        {
            var dic = entity.AsDictionary ();
            var doc = new Record (dic);
            var sqlParams = DbUtil.GetInsertSql (TableName, doc);
            _command.CommandText = sqlParams.Sql;
            var paramList = DbUtil.GetDbParams (sqlParams.Params);
            _command.Parameters.AddRange (paramList);
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr;
        }

        public async Task<bool> IsExistColmnAsync (string ColumnName)
        {
            var sql = $"select COUNT(1) from information_schema.columns WHERE table_name = '{TableName}' and column_name = '{ColumnName}'";
            _command.CommandText = sql;
            var rr = await _command.ExecuteScalarAsync ();
            return rr.As<int> () == 1;
        }

        public async Task<long> RenameColumnAsync (string ColumnName, string newColumnName, string typeName)
        {
            var sql = $"alter table {TableName} change column {ColumnName} {newColumnName} {typeName}";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateAsync (Guid id, IRecord doc)
        {
            var paramObj = DbUtil.GetUpdateSql (TableName, doc);
            var idSymbol = $"{paramObj.ParamSymbol}id";
            var sql = $"{paramObj.Sql} where 1=1 and Id={idSymbol}";
            var paramDic = paramObj.Params;
            paramDic.Add (idSymbol, id);
            var sqlParams = DbUtil.GetDbParams (paramDic);
            _command.CommandText = sql;
            _command.Parameters.AddRange (sqlParams);
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr;
        }

        public async Task<long> UpdateAsync (Query query, IRecord doc)
        {
            var paramObj = DbUtil.GetUpdateSql (TableName, doc);
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var sql = $"{paramObj.Sql} where 1=1 and {param.Sql}";
            var paramDic = paramObj.Params;
            var sqlParams = DbUtil.GetDbParams (paramDic);
            var mysqlParams = DbUtil.GetDbParams (param.Params);
            _command.CommandText = sql;
            _command.Parameters.AddRange (sqlParams);
            _command.Parameters.AddRange (mysqlParams);
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr;
        }

        public async Task<long> UpdateColumnAsync (string ColumnName, string typeName, bool IsNotNull)
        {
            var sql = $"alter table {TableName} modify {ColumnName} {typeName}";
            if (IsNotNull)
                sql = $"{sql} not null";
            _command.CommandText = sql;
            var rr = await _command.ExecuteNonQueryAsync ();
            return rr == 0 ? 1 : -1;
        }

        #endregion

    }
}