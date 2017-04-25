using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Brochure.Core.Extends;
using Brochure.Core.Helper;
using Brochure.Core.Query;

namespace Brochure.Core.implement
{
    public class SqlServerConnection : IConnection
    {
        private string _preString = ConstString.SqlServerPre;
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        public SqlServerConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            Transaction = new SqlServerTransaction(_transaction);
        }

        public ITransaction Transaction { get; }

        public IDocument Insert(IEntrity entrity)
        {
            try
            {
                OpenConnection();
                var dic = entrity.AsDictionary();
                var arr = dic.Keys.AddPreString(_preString);
                var tableName = dic[ConstString.T];
                dic.Remove(ConstString.T);
                if (dic[ConstString.Id].To<Guid>() == Guid.Empty)
                    dic[ConstString.Id] = Guid.NewGuid();
                string sql = $"Insert into {tableName}({dic.Keys.ToString(",")}) values({arr.ToString(",")})";
                SqlCommand cmd = GetSqlCommand(dic, sql);
                var result = cmd.ExecuteNonQuery();
                if (result != 1)
                    throw new Exception($"插入{tableName}错误");
                return dic.AsDocument();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public long Delete<T>(QueryBuild build, IEntrity entrity = null) where T : IEntrity
        {
            try
            {
                OpenConnection();
                if (entrity == null)
                    entrity = ObjectHelper.CreateInstance<T>();
                var sql = $"delete from {entrity.TableName} where " + build;
                SqlCommand cmd = GetSqlCommand(build.GetDictionary(), sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public long DeleteById(IEntrity entrity)
        {
            if (entrity == null)
                throw new Exception("参数错误");
            var tuple = entrity.GetPropertyValueTuple<IEntrity>(t => t.Id);
            return Delete<IEntrity>(QueryBuild.Ins.And(tuple), entrity);
        }

        public long DeleteById<T>(Guid id) where T : IEntrity
        {
            var entrity = ObjectHelper.CreateInstance<T>();
            return Delete<T>(QueryBuild.Ins.And(ConstString.Id, id), entrity);
        }

        public long Update(IEntrity obj)
        {
            throw new NotImplementedException();
        }

        public T GetInfoById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public T Search<T>(string str)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument> InsertAsync(IEntrity entrity)
        {
            throw new NotImplementedException();
        }

        public Task<long> DeleteAsync(IEntrity entrity)
        {
            throw new NotImplementedException();
        }

        public Task<long> UpdateaAsync(IEntrity entrity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetInfoByIdAsync<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<T> SearchAsync<T>(string str)
        {
            throw new NotImplementedException();
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

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            if (_transaction == null || _transaction.Connection == null)
                _transaction = _connection.BeginTransaction();
        }

        private SqlCommand GetSqlCommand(IDictionary<string, object> dictionary, string sql)
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
    }
}
