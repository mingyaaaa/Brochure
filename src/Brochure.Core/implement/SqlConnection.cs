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

        public IDocument Insert(InsertBuid insertBuid)
        {
            try
            {
                OpenConnection();
                var dic = insertBuid.GetDocument();
                SqlCommand cmd = GetSqlCommand(dic, insertBuid.ToString());
                var result = cmd.ExecuteNonQuery();
                if (result != 1)
                    throw new Exception($"插入{insertBuid.GetTableName()}错误");
                return dic;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public long Delete(DeleteBuild build)
        {
            try
            {
                OpenConnection();
                var sql = build.ToString();
                SqlCommand cmd = GetSqlCommand(build.GetDocument(), sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

        public long Update(IEntrity obj)
        {
            throw new NotImplementedException();
        }

        public T GetInfoById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public IDocument GetInfoById(string id)
        {
            throw new NotImplementedException();
        }

        public IDocument Search(string str)
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

        public Task<IDocument> GetInfoByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument> SearchAsync(string str)
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
    }
}
