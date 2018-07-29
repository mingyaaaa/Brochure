using System;
using System.Collections.Generic;
using System.Data;
using Brochure.Core.Abstracts;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Server.Interfaces;
using Brochure.Core.Server.SQLMap;
using Brochure.Server.MySql.Implements;
using MySql.Data.MySqlClient;
namespace Brochure.Server.MySql
{
    public class MySqlDbConnect : IDbConnect, IDisposable
    {
        private MySqlConnection _connection;
        private IDictionary<string, IDbData> _baseDic;
        private MySqlTransaction _transaction;

        private ISqlParse _parse;
        internal MySqlDbConnect (string connectString, ISqlParse sqlParse)
        {
            //注册表映射类型
            AbSingleton.Regist<MySqlTypeMap> ();
            _connection = new MySqlConnection (connectString);
            _baseDic = new Dictionary<string, IDbData> ();
            _parse = sqlParse;
            Open ();
        }
        internal MySqlDbConnect (string connectString, ISqlParse sqlParse, bool isbeginTransation) : this (connectString, sqlParse)
        {
            if (isbeginTransation)
                _transaction = _connection.BeginTransaction ();
        }
        public IDbDatabase GetBatabaseHub ()
        {
            var commond = new MySqlCommand ();
            commond.Connection = _connection;
            return new MySqlDatabase (commond, _parse);
        }

        public IDbTableBase GetTableHub ()
        {
            var commond = new MySqlCommand ();
            commond.Connection = _connection;
            return new MySqlDatabase (commond, _parse);
        }

        public IDbData GetDataHub (string tableName)
        {
            var key = tableName;
            if (_baseDic.ContainsKey (key))
                return _baseDic[key];
            var commond = new MySqlCommand ();
            commond.Connection = _connection;
            commond.Transaction = _transaction;
            var table = new MySqlDatabase (key, commond, _parse);
            _baseDic.Add (key, table);
            return table;
        }
        public void Close ()
        {
            _connection?.Close ();
        }
        public void Open ()
        {
            if (_connection != null && _connection.State == ConnectionState.Closed)
                _connection.Open ();
        }
        public void BeginTransaction ()
        {
            _transaction = _connection.BeginTransaction ();
        }
        public void Dispose ()
        {
            Close ();
            _connection?.Dispose ();
        }

        public void Commit ()
        {
            _transaction?.Commit ();
        }

        public void Rollback ()
        {
            _transaction?.Rollback ();
        }

    }
}