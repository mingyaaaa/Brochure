using Brochure.Core;
using Brochure.Core.Server;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.Server.MySql
{
    public class MySqlDatabase : IDbTableBase, IDbDatabase, IDbColumns, IDbData, IDbTransaction, IDisposable
    {
        private ISqlParse _parse;
        private MySqlConnection _connection;
        private bool _isBeginTransaction;
        private MySqlTransaction _transaction;

        public MySqlDatabase(string tableName, MySqlConnection connection) : this()
        {
            this.TableName = tableName;
            _connection = connection;
        }
        public MySqlDatabase(MySqlConnection connection) : this()
        {
            _connection = connection;
        }
        protected MySqlDatabase()
        {
            _parse = new MySqlParse();
        }
        public string TableName { get; }
        public bool IsBeginTransaction { get; set; }
        #region Database
        public async Task<long> CreateDataBaseAsync(string databaseName)
        {
            var command = new MySqlCommand() { Connection = _connection };
            var isExist = await IsExistDataBaseAsync(databaseName);
            if (isExist)
                return 1;
            command.CommandText = $"create database {databaseName}";
            return await command.ExecuteNonQueryAsync();
        }
        public async Task<long> DeleteDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"drop database {databaseName}";
            //执行删除方法  删除成功 默认返回了0  改变返回的值；
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistDataBaseAsync(string databaseName)
        {
            var command = GetCommand();
            command.CommandText = $"SELECT count(1) FROM information_schema.SCHEMATA where SCHEMA_NAME='{databaseName}'";
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>() == 1;
        }
        #endregion

        #region Table
        public async Task<long> RegistTableAsync<T>() where T : EntityBase
        {
            return await CreateTableAsync<T>();
        }
        public async Task<long> CreateTableAsync<T>() where T : EntityBase
        {
            var command = GetCommand();
            var tableName = DbUtil.GetTableName<T>();
            var r = await IsExistTableAsync(tableName);
            if (r)
                return 1;
            command.CommandText = DbUtil.GetCreateTableSql<T>();
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        public async Task<bool> IsExistTableAsync(string tableName)
        {
            var command = GetCommand();
            command.CommandText = $"SELECT count(1) FROM information_schema.TABLES WHERE table_name ='{tableName}'";
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>() == 1;
        }
        public async Task<long> DeleteTableAsync(string tableName)
        {
            var command = GetCommand();
            command.CommandText = $"drop table  {tableName}";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateTableNameAsync(string olderName, string tableName)
        {
            var command = GetCommand();
            var r = await IsExistTableAsync(olderName);
            if (!r)
                return 0;
            command.CommandText = $"alter table {olderName} rename {tableName};";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        #endregion

        #region Columns
        public async Task<bool> IsExistColumnAsync(string columnName)
        {
            var command = GetCommand();
            command.CommandText = $"select COUNT(1) from information_schema.columns WHERE table_name = '{TableName}' and column_name = '{columnName}'";
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>() == 1;
        }

        public async Task<long> RenameColumnAsync(string columnName, string newcolumnName, string typeName)
        {
            var command = GetCommand();
            command.CommandText = $"alter table {TableName} change column {columnName} {newcolumnName} {typeName}";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateColumnAsync(string columnName, string typeName, bool isNotNull)
        {
            var command = GetCommand();
            var sql = $"alter table {TableName} modify {columnName} {typeName}";
            if (isNotNull)
                sql = $"{sql} not null";
            command.CommandText = sql;
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteColumnAsync(string columnName)
        {
            var command = GetCommand();
            command.CommandText = $"alter table {TableName} drop column {columnName}";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }
        public async Task<long> AddColumnsAsync(string columnName, string typeName, bool isNotNull)
        {
            var command = GetCommand();
            var sql = $"alter table {TableName} add column {columnName} {typeName}";
            if (isNotNull)
            {
                sql = $"{sql} not null";
            }
            command.CommandText = sql;
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        #endregion

        #region Data
        public async Task<long> CreateIndexAsync(string[] columnNames, string indexName, string sqlIndex)
        {
            var command = GetCommand();
            command.CommandText = $"create {sqlIndex} {indexName} on {TableName}({string.Join(",", columnNames)})";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteIndexAsync(string indexName)
        {
            var command = GetCommand();
            command.CommandText = $"drop index {indexName} on {TableName}";
            var rr = await command.ExecuteNonQueryAsync();
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteAsync(IEnumerable<Guid> ids)
        {
            var command = GetCommand();
            var query = Query.In("Id", ids.Select(t => t.ToString()).ToArray());
            var parse = new QueryParse(_parse);
            var param = parse.Parse(query);
            command.CommandText = $"delete from {TableName} where 1=1 and {param.Sql}";
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.Parameters.AddRange(mysqlParams);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<long> DeleteAsync(Query query)
        {
            var command = GetCommand();
            var parse = new QueryParse(_parse);
            var param = parse.Parse(query);
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.CommandText = $"delete from {TableName} where 1=1 and {param.Sql}";
            command.Parameters.AddRange(mysqlParams);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<long> DeleteAsync(Guid id)
        {
            return await DeleteAsync(new Guid[] { id });
        }
        public async Task<long> GetCountAsync(Query query)
        {
            var command = GetCommand();
            command.CommandText = $"select count(*) from {TableName} where 1=1 and {query}";
            var parse = new QueryParse(_parse);
            var param = parse.Parse(query);
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.Parameters.AddRange(mysqlParams);
            var rr = await command.ExecuteScalarAsync();
            return rr.As<int>();
        }

        public async Task<IRecord> GetInfoAsync(Guid guid)
        {
            var command = GetCommand();
            var query = Query.Eq("Id", guid.ToString());
            var parse = new QueryParse(_parse);
            var param = parse.Parse(query);
            IRecord doc = null;
            command.CommandText = $"select * from {TableName} where 1=1 and {param.Sql}";
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.Parameters.AddRange(mysqlParams);
            var dr = await command.ExecuteReaderAsync();
            if (dr.Read())
            {
                doc = new Record();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName(i);
                    var value = dr[i];
                    doc[name] = value;
                }
            }
            dr.Close();
            return doc;
        }

        public async Task<List<IRecord>> GetListAsync(SearchParams searchParams)
        {
            var command = GetCommand();
            List<IRecord> result = new List<IRecord>();
            var orderDoc = searchParams.OrderField;
            var orderStr = string.Empty;
            if (orderDoc != null)
            {
                foreach (var item in orderDoc.Keys)
                {
                    var value = orderDoc[item].As<OrderType>();
                    orderStr = $"{orderStr}";
                    if (value == OrderType.Desc)
                        orderStr = $"{orderStr} desc";
                    else
                        orderStr = $"{orderStr} asc";
                }
                orderStr = $" order by {orderStr} ";
            }
            var pageStr = string.Empty;
            if (searchParams.EndIndex < searchParams.StarIndex)
                throw new Exception("分页参数不正确");
            if (searchParams.StarIndex != 0 && searchParams.EndIndex != 0)
            {
                var count = searchParams.EndIndex - searchParams.StarIndex + 1;
                pageStr = $" limit {searchParams.StarIndex},{count}";
            }
            //分页 排序
            var parse = new QueryParse(_parse);
            var param = parse.Parse(searchParams.Filter);
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.CommandText = $"select * from {TableName} where 1=1 and {param.Sql} {orderStr} {pageStr}";
            command.Parameters.AddRange(mysqlParams);
            var dr = await command.ExecuteReaderAsync();
            while (dr.Read())
            {
                var doc = new Record();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName(i);
                    doc[name] = dr[i];
                }
                result.Add(doc);
            }
            dr.Close();
            return result;
        }
        public async Task<List<IRecord>> GetListGroupByAsync(List<Aggregate> aggregates, SearchParams searchParams, params string[] groupFields)
        {
            var command = GetCommand();
            List<IRecord> result = null;
            var orderDoc = searchParams.OrderField;
            var orderStr = string.Empty;
            foreach (var item in orderDoc.Keys)
            {
                var value = orderDoc[item].As<OrderType>();
                orderStr = $"{orderStr}";
                if (value == OrderType.Desc)
                    orderStr = $"{orderStr} desc";
                else
                    orderStr = $"{orderStr} asc";
            }
            var count = searchParams.EndIndex - searchParams.StarIndex + 1;
            var gfield = string.Join(",", groupFields);
            var parse = new QueryParse(_parse);
            var param = parse.Parse(searchParams.Filter);
            var mysqlParams = DbUtil.GetMySqlParams(param.Params);
            command.CommandText = $"select {string.Join(",", aggregates)},{gfield} from {TableName} where 1=1 and {param.Sql} group by {gfield} order by {orderStr} limit {searchParams.StarIndex},{count}";
            command.Parameters.AddRange(mysqlParams);
            var dr = await command.ExecuteReaderAsync();
            while (dr.Read())
            {
                var doc = new Record();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName(i);
                    doc[name] = dr[i];
                }
                result.Add(doc);
            }
            return result;

        }
        public async Task<long> InserManyAsync(IEnumerable<EntityBase> entities)
        {
            var tasks = new List<Task<long>>();
            foreach (var item in entities.ToList())
                tasks.Add(InserOneAsync(item));
            var rr = await Task.WhenAll(tasks);
            return rr.Sum(t => t);
        }

        public async Task<long> InserOneAsync(EntityBase entity)
        {
            var command = GetCommand();
            var dic = entity.AsDictionary();
            var doc = new Record(dic);
            var sqlParams = DbUtil.GetInsertSql(TableName, doc);
            command.CommandText = sqlParams.Sql;
            var paramList = DbUtil.GetMySqlParams(sqlParams.Params);
            command.Parameters.AddRange(paramList);
            return await command.ExecuteNonQueryAsync();
        }
        public async Task<long> UpdateAsync(Guid id, IRecord doc)
        {
            var command = GetCommand();
            var paramObj = DbUtil.GetUpdateSql(TableName, doc);
            var idSymbol = $"{paramObj.ParamSymbol}Id0";
            idSymbol = Sequence.GetRecordSequence(paramObj.Params, idSymbol);
            var sqlParams = DbUtil.GetMySqlParams(paramObj.Params, new Record()
            {
                [idSymbol] = id
            });
            command.CommandText = $"{paramObj.Sql} where 1=1 and Id={idSymbol}";
            command.Parameters.AddRange(sqlParams);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<long> UpdateAsync(Query query, IRecord doc)
        {
            var command = GetCommand();
            var paramObj = DbUtil.GetUpdateSql(TableName, doc);
            var parse = new QueryParse(_parse);
            var param = parse.Parse(query);
            var sql = $"{paramObj.Sql} where 1=1 and {param.Sql}";
            var paramDic = paramObj.Params;
            var sqlParams = DbUtil.GetMySqlParams(paramDic, param.Params);
            command.CommandText = sql;
            command.Parameters.AddRange(sqlParams);
            return await command.ExecuteNonQueryAsync();
        }

        #endregion

        private MySqlCommand GetCommand()
        {
            var command = new MySqlCommand()
            {
                Connection = _connection
            };
            command.Transaction = _transaction;
            return command;
        }
        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            _transaction?.Dispose();
        }
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }
        public void Commit()
        {
            _transaction?.Commit();
        }
        public void Rollback()
        {
            _transaction?.Rollback();
        }
    }
}
