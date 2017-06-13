using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Abstract;
using Brochure.Core.Extends;
using Brochure.Core.Helper;
using Brochure.Core.implement;

namespace ConnectionDal
{
    public class MySqlServerConnection : IConnection
    {
        private string _preString = Singleton.GetInstace<Setting>().ConnectString;
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        public MySqlServerConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IDocument Insert(IEntrity entrity)
        {
            try
            {
                OpenConnection();
                InsertBuid insertBuid = new InsertBuid(entrity);
                var dic = insertBuid.GetDocument();
                SqlCommand cmd = GetSqlCommand(dic, insertBuid.ToString());
                var result = cmd.ExecuteNonQuery();
                if (result != 1)
                    throw new Exception($"插入{insertBuid.GetTableName()}错误");
                return dic;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public long Delete(BaseBuild deletebuild)
        {
            try
            {
                if (!(deletebuild is DeleteBuild))
                    throw new ArgumentException("参数类型错误请传入DeleteBuild类型");
                var build = deletebuild.To<DeleteBuild>();
                OpenConnection();
                var sql = build.ToString();
                SqlCommand cmd = GetSqlCommand(build.GetDocument(), sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log(e);
                return -1;
            }
        }

        public long DeleteById<T>(Guid id) where T : IEntrity
        {
            var instance = ObjectHelper.CreateInstance<T>();
            DeleteBuild query = new DeleteBuild(new WhereBuild().And(instance.Equal(t => t.Id, id)));
            return Delete(query);
        }

        public long DeleteById(IEntrity entrity)
        {
            DeleteBuild query = new DeleteBuild(new WhereBuild().And(entrity.Equal(t => t.Id)));
            return Delete(query);
        }

        public long Update(BaseBuild build)
        {
            try
            {
                if (!(build is UpdateBuild))
                    throw new ArgumentException("参数类型错误请传入UpdateBuild类型");
                var updateBuild = build.To<UpdateBuild>();
                OpenConnection();
                var str = updateBuild.ToString();
                var cmd = GetSqlCommand(updateBuild.GetDocument(), str);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log(e);
                return -1;
            }
        }

        public T GetInfoById<T>(Guid id) where T : IEntrity
        {
            try
            {
                var entrity = ObjectHelper.CreateInstance<T>();
                OpenConnection(false);
                var cmd = new SqlCommand($"select * from {entrity.TableName} where id={_preString}id", _connection);
                cmd.Parameters.Add(new SqlParameter($"{_preString}id", id));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IDocument obj = new RecordDocument();
                        for (int i = 0; i < reader.FieldCount; i++)
                            obj.Add(reader.GetName(i), reader[i]);
                        return obj.ToEntrityObject<T>();
                    }
                }
                return default(T);
            }
            catch (Exception e)
            {
                Log(e);
                return default(T);
            }
        }

        public List<IDocument> Search(BaseBuild select)
        {
            try
            {
                if (!(select is SelectBuild))
                    throw new ArgumentException("参数类型错误请传入SelectBuild类型");
                var selectBuild = select.To<SelectBuild>();
                OpenConnection(false);
                var cmd = GetSqlCommand(selectBuild.GetDocument(), selectBuild.ToString());
                var reader = cmd.ExecuteReader();
                List<IDocument> result = new List<IDocument>();
                var paramList = selectBuild.GetParamList();
                while (reader.Read())
                {
                    IDocument obj = new RecordDocument();
                    foreach (var item in paramList)
                    {
                        obj.Add(item, reader[item]);
                    }
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public List<IDocument> SearchProcedure(string procedureName, IParameter outParameter = null, params IParameter[] inParameter)
        {
            try
            {
                List<IDocument> result = new List<IDocument>();
                OpenConnection();
                var cmd = new SqlCommand(procedureName, _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in inParameter)
                {
                    SqlParameter inpar = new SqlParameter(item.Parameter.ParameterName, item.Parameter.Value);
                    cmd.Parameters.Add(inpar).Direction = ParameterDirection.Input;
                }
                if (outParameter != null)
                {
                    SqlParameter outpar = new SqlParameter(outParameter.Parameter.ParameterName, outParameter.Parameter.Value);
                    cmd.Parameters.Add(outpar).Direction = ParameterDirection.Output;
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IDocument obj = new RecordDocument();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        obj.Add(reader.GetName(i), reader[i]);
                    }
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e)
            {
                Log(e);
                throw;
            }
        }

        public async Task<List<IDocument>> SearchProcedureAsync(string procedureName, IParameter outParameter = null, params IParameter[] inParameter)
        {
            try
            {
                List<IDocument> result = new List<IDocument>();
                OpenConnection();
                var cmd = new SqlCommand(procedureName, _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in inParameter)
                {
                    SqlParameter inpar = new SqlParameter(item.Parameter.ParameterName, item.Parameter.Value);
                    cmd.Parameters.Add(inpar).Direction = ParameterDirection.Input;
                }
                if (outParameter != null)
                {
                    SqlParameter outpar = new SqlParameter(outParameter.Parameter.ParameterName, outParameter.Parameter.Value);
                    cmd.Parameters.Add(outpar).Direction = ParameterDirection.Output;
                }
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    IDocument obj = new RecordDocument();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        obj.Add(reader.GetName(i), reader[i]);
                    }
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e)
            {
                Log(e);
                throw;
            }
        }

        public async Task<IDocument> InsertAsync(IEntrity entrity)
        {
            try
            {
                OpenConnection();
                InsertBuid insertBuid = new InsertBuid(entrity);
                var dic = insertBuid.GetDocument();
                SqlCommand cmd = GetSqlCommand(dic, insertBuid.ToString());
                var result = await cmd.ExecuteNonQueryAsync();
                if (result != 1)
                    throw new Exception($"插入{insertBuid.GetTableName()}错误");
                return dic;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public async Task<long> DeleteAsync(BaseBuild deletebuild)
        {
            try
            {
                if (!(deletebuild is DeleteBuild))
                    throw new ArgumentException("参数类型错误请传入DeleteBuild类型");
                var build = deletebuild.To<DeleteBuild>();
                OpenConnection();
                var sql = build.ToString();
                SqlCommand cmd = GetSqlCommand(build.GetDocument(), sql);
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<long> UpdateaAsync(BaseBuild build)
        {
            try
            {
                if (!(build is UpdateBuild))
                    throw new ArgumentException("参数类型错误请传入UpdateBuild类型");
                var updateBuild = build.To<UpdateBuild>();
                OpenConnection();
                var str = updateBuild.ToString();
                var cmd = GetSqlCommand(updateBuild.GetDocument(), str);
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Log(e);
                return -1;
            }
        }

        public async Task<T> GetInfoByIdAsync<T>(Guid id) where T : IEntrity
        {
            try
            {
                var entrity = ObjectHelper.CreateInstance<T>();
                OpenConnection(false);
                var cmd = new SqlCommand($"select * from {entrity.TableName} where id={_preString}id", _connection);
                cmd.Parameters.Add(new SqlParameter($"{_preString}id", id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        IDocument obj = new RecordDocument();
                        for (int i = 0; i < reader.FieldCount; i++)
                            obj.Add(reader.GetName(i), reader[i]);
                        return obj.ToEntrityObject<T>();
                    }
                }
                return default(T);
            }
            catch (Exception e)
            {
                Log(e);
                return default(T);
            }
        }

        public async Task<List<IDocument>> SearchAsync(BaseBuild select)
        {
            try
            {
                if (!(select is SelectBuild))
                    throw new ArgumentException("参数类型错误请传入SelectBuild类型");
                var selectBuild = select.To<SelectBuild>();
                OpenConnection(false);
                var cmd = GetSqlCommand(selectBuild.GetDocument(), selectBuild.ToString());
                var reader = await cmd.ExecuteReaderAsync();
                List<IDocument> result = new List<IDocument>();
                var paramList = selectBuild.GetParamList();
                while (reader.Read())
                {
                    IDocument obj = new RecordDocument();
                    foreach (var item in paramList)
                    {
                        obj.Add(item, reader[item]);
                    }
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e)
            {
                Log(e);
                return null;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Close();
        }

        public void Commit()
        {
            _transaction.Commit();
            Close();
        }

        private void OpenConnection(bool isBeginTransaction = true)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            if ((_transaction == null || _transaction.Connection == null) && isBeginTransaction)
                _transaction = _connection.BeginTransaction();
        }

        private SqlCommand GetSqlCommand(IDocument dictionary, string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _connection);
            cmd.Transaction = _transaction;
            foreach (var item in dictionary.Keys)
            {
                SqlParameter parameter = new SqlParameter(_preString + item, dictionary[item]);
                cmd.Parameters.Add(parameter);
            }
            return cmd;
        }

        private void Log(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
