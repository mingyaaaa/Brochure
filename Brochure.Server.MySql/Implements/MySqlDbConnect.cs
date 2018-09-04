using Brochure.Core.Server;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Brochure.Server.MySql
{
    public class MySqlDbConnect : IDbConnect, IDisposable
    {
        private MySqlConnection _connection;
        private IDictionary<string, IDbData> _baseDic;
        private MySqlTransaction _transaction;

        internal MySqlDbConnect(string connectString)
        {
            //注册表映射类型
            _connection = new MySqlConnection(connectString);
            _baseDic = new Dictionary<string, IDbData>();
            Open();
        }
        internal MySqlDbConnect(string connectString, bool isbeginTransation) : this(connectString)
        {
            if (isbeginTransation)
                _transaction = _connection.BeginTransaction();
        }
        public IDbDatabase GetBatabaseHub()
        {
            var commond = new MySqlCommand();
            commond.Connection = _connection;
            return new MySqlDatabase(commond);
        }

        public IDbTableBase GetTableHub()
        {
            var commond = new MySqlCommand();
            commond.Connection = _connection;
            return new MySqlDatabase(commond);
        }

        public IDbData GetDataHub(string tableName)
        {
            var key = tableName;
            if (_baseDic.ContainsKey(key))
                return _baseDic[key];
            var commond = new MySqlCommand();
            commond.Connection = _connection;
            commond.Transaction = _transaction;
            var table = new MySqlDatabase(key, commond);
            _baseDic.Add(key, table);
            return table;
        }
        public void Close()
        {
            _connection?.Close();
        }
        public void Open()
        {
            if (_connection != null && _connection.State == ConnectionState.Closed)
                _connection.Open();
        }
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }
        public void Dispose()
        {
            Close();
            _connection?.Dispose();
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
