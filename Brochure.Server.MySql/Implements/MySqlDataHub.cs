using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Server;

namespace Brochure.Server.MySql.Implements
{
    public class MySqlDataHub : IDataHub
    {
        private ISqlParse _parse;
        private DbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        public MySqlDataHub (IClient client, string tableName, IDbTransaction dbTransaction)
        {
            Client = client;
            _parse = client.SqlParse;
            TableName = tableName;
            _dbTransaction = dbTransaction;
        }

        public string TableName { get; }

        public IClient Client { get; }
        #region Columns
        public async Task<bool> IsExistColumnAsync (string columnName)
        {
            var command = GetCommand ();
            command.CommandText = $"select COUNT(1) from information_schema.columns WHERE table_name = '{TableName}' and column_name = '{columnName}'";
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr.As<int> () == 1;
        }

        public async Task<long> RenameColumnAsync (string columnName, string newcolumnName, string typeName)
        {
            var command = GetCommand ();
            command.CommandText = $"alter table {TableName} change column {columnName} {newcolumnName} {typeName}";
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> UpdateColumnAsync (string columnName, string typeName, bool isNotNull)
        {
            var command = GetCommand ();
            var sql = $"alter table {TableName} modify {columnName} {typeName}";
            if (isNotNull)
                sql = $"{sql} not null";
            command.CommandText = sql;
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteColumnAsync (string columnName)
        {
            var command = GetCommand ();
            command.CommandText = $"alter table {TableName} drop column {columnName}";
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }
        public async Task<long> AddColumnsAsync (string columnName, string typeName, bool isNotNull)
        {
            var command = GetCommand ();
            var sql = $"alter table {TableName} add column {columnName} {typeName}";
            if (isNotNull)
            {
                sql = $"{sql} not null";
            }
            command.CommandText = sql;
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }

        #endregion

        #region Data
        public async Task<long> CreateIndexAsync (string[] columnNames, string indexName, string sqlIndex)
        {
            var command = GetCommand ();
            command.CommandText = $"create {sqlIndex} {indexName} on {TableName}({string.Join(",", columnNames)})";
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteIndexAsync (string indexName)
        {
            var command = GetCommand ();
            command.CommandText = $"drop index {indexName} on {TableName}";
            var rr = await Excute<int> (command.ExecuteNonQueryAsync, -1);
            return rr == 0 ? 1 : -1;
        }

        public async Task<long> DeleteAsync (IEnumerable<Guid> ids)
        {
            var command = GetCommand ();
            var query = Query.In ("Id", ids.Select (t => t.ToString ()).ToArray ());
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            command.CommandText = $"delete from {TableName} where 1=1 and {param.Sql}";
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.Parameters.AddRange (mysqlParams);
            return await Excute<int> (command.ExecuteNonQueryAsync, -1);
        }

        public async Task<long> DeleteAsync (Query query)
        {
            var command = GetCommand ();
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.CommandText = $"delete from {TableName} where 1=1 and {param.Sql}";
            command.Parameters.AddRange (mysqlParams);
            return await Excute<int> (command.ExecuteNonQueryAsync, -1);
        }

        public async Task<long> DeleteAsync (Guid id)
        {
            return await DeleteAsync (new Guid[] { id });
        }
        public async Task<long> GetCountAsync (Query query)
        {
            //查询无法共用连接
            var connecttion = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
            connecttion.Open ();
            var command = connecttion.CreateCommand ();
            command.CommandText = $"select count(*) from {TableName} where 1=1 and {query}";
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.Parameters.AddRange (mysqlParams);
            var rr = await command.ExecuteScalarAsync ();
            connecttion.Close ();
            return rr.As<int> ();
        }

        public async Task<IRecord> GetInfoAsync (Guid guid)
        {
            //查询无法共用连接
            var connecttion = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
            connecttion.Open ();
            var command = connecttion.CreateCommand ();
            var query = Query.Eq ("Id", guid.ToString ());
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            IRecord doc = null;
            command.CommandText = $"select * from {TableName} where 1=1 and {param.Sql}";
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.Parameters.AddRange (mysqlParams);
            var dr = await command.ExecuteReaderAsync ();
            if (dr.Read ())
            {
                doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    doc[name] = dr[i];
                }
            }
            dr.Close ();
            connecttion.Close ();
            return doc;
        }

        public async Task<List<IRecord>> GetListAsync (SearchParams searchParams)
        {
            //查询无法共用连接
            var connecttion = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
            connecttion.Open ();
            var command = connecttion.CreateCommand ();
            List<IRecord> result = new List<IRecord> ();
            var orderDoc = searchParams.OrderField;
            var orderStr = string.Empty;
            if (orderDoc != null)
            {
                foreach (var item in orderDoc.Keys)
                {
                    var value = orderDoc[item].As<OrderType> ();
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
                throw new Exception ("分页参数不正确");
            if (searchParams.StarIndex != 0 && searchParams.EndIndex != 0)
            {
                var count = searchParams.EndIndex - searchParams.StarIndex + 1;
                pageStr = $" limit {searchParams.StarIndex},{count}";
            }
            //分页 排序
            var parse = new QueryParse (_parse);
            var param = parse.Parse (searchParams.Filter);
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.CommandText = $"select * from {TableName} where 1=1 and {param.Sql} {orderStr} {pageStr}";
            command.Parameters.AddRange (mysqlParams);
            var dr = await command.ExecuteReaderAsync ();
            while (dr.Read ())
            {
                var doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    doc[name] = dr[i];
                }
                result.Add (doc);
            }
            dr.Close ();
            connecttion.Close ();
            return result;
        }
        public async Task<List<IRecord>> GetListGroupByAsync (List<Aggregate> aggregates, SearchParams searchParams, params string[] groupFields)
        {
            //查询无法共用连接
            var connecttion = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
            connecttion.Open ();
            var command = connecttion.CreateCommand ();
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
            var mysqlParams = DbUtil.GetMySqlParams (Client.TypeMap, param.Params);
            command.CommandText = $"select {string.Join(",", aggregates)},{gfield} from {TableName} where 1=1 and {param.Sql} group by {gfield} order by {orderStr} limit {searchParams.StarIndex},{count}";
            command.Parameters.AddRange (mysqlParams);
            var dr = await command.ExecuteReaderAsync ();
            while (dr.Read ())
            {
                var doc = new Record ();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    var name = dr.GetName (i);
                    doc[name] = dr[i];
                }
                result.Add (doc);
            }
            dr.Close ();
            connecttion.Close ();
            return result;

        }
        public async Task<long> InserManyAsync (IEnumerable<EntityBase> entities)
        {
            var command = GetCommand ();
            var list = entities.ToList ();
            var docs = new List<IRecord> ();
            IRecord doc;
            foreach (var item in list)
            {
                doc = new Record (item);
                docs.Add (doc);
            }
            var sqlParams = DbUtil.GetInsertManySql (TableName, docs.ToArray ());
            command.CommandText = sqlParams.Sql;
            var paramList = DbUtil.GetMySqlParams (Client.TypeMap, sqlParams.Params);
            command.Parameters.AddRange (paramList);
            return await command.ExecuteNonQueryAsync ();
        }

        public async Task<long> InserOneAsync (EntityBase entity)
        {
            return await Excute<long> (() => Insert (entity), -1);
        }

        private async Task<long> Insert (EntityBase entity)
        {
            var command = GetCommand ();
            var doc = new Record (entity);
            var sqlParams = DbUtil.GetInsertSql (TableName, doc);
            command.CommandText = sqlParams.Sql;
            var paramList = DbUtil.GetMySqlParams (Client.TypeMap, sqlParams.Params);
            command.Parameters.AddRange (paramList);
            return await command.ExecuteNonQueryAsync ();
        }

        public async Task<long> UpdateAsync (Guid id, IRecord doc)
        {
            var command = GetCommand ();
            var paramObj = DbUtil.GetUpdateSql (TableName, doc);
            var idSymbol = $"{paramObj.ParamSymbol}Id0";
            idSymbol = Sequence.GetRecordSequence (paramObj.Params, idSymbol);
            var sqlParams = DbUtil.GetMySqlParams (Client.TypeMap, paramObj.Params, new Record ()
            {
                [idSymbol] = id
            });
            command.CommandText = $"{paramObj.Sql} where 1=1 and Id={idSymbol}";
            command.Parameters.AddRange (sqlParams);
            return await Excute<int> (command.ExecuteNonQueryAsync, -1);
        }

        public async Task<long> UpdateAsync (Query query, IRecord doc)
        {
            var command = GetCommand ();
            var paramObj = DbUtil.GetUpdateSql (TableName, doc);
            var parse = new QueryParse (_parse);
            var param = parse.Parse (query);
            var sql = $"{paramObj.Sql} where 1=1 and {param.Sql}";
            var paramDic = paramObj.Params;
            var sqlParams = DbUtil.GetMySqlParams (Client.TypeMap, paramDic, param.Params);
            command.CommandText = sql;
            command.Parameters.AddRange (sqlParams);
            return await Excute<int> (command.ExecuteNonQueryAsync, -1);
        }
        private DbCommand GetCommand ()
        {
            if (_dbTransaction != null)
                _dbConnection = _dbTransaction.Transaction.Connection;
            if (_dbConnection == null)
                _dbConnection = DbConnectPool.GetDbConnection (DatabaseType.MySql, Client.DatabaseName);
            var dbCommand = _dbConnection.CreateCommand ();
            if (_dbTransaction != null)
                dbCommand.Transaction = _dbTransaction.Transaction;
            _dbConnection.Open ();
            return dbCommand;
        }

        private async Task<T> Excute<T> (Func<Task<T>> func, T errorValue)
        {
            var r = default (T);
            try
            {
                r = await func?.Invoke ();
            }
            catch (Exception ex)
            {
                r = errorValue;
            }
            finally
            {
                if (_dbTransaction == null)
                    _dbConnection?.Close ();
            }
            return r;
        }
        public void Dispose ()
        {
            _dbConnection?.Close ();
        }

        ~MySqlDataHub ()
        {
            _dbConnection?.Close ();
        }
        #endregion
    }
}