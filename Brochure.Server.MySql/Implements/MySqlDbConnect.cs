using System;
using System.Collections.Generic;
using System.Data;
using Brochure.Core;
using Brochure.Core.Server;
using MySql.Data.MySqlClient;

namespace Brochure.Server.MySql
{
    public class MySqlDbConnect : IDbConnect, IDisposable
    {
        private MySqlConnection _connection;
        private IDictionary<string, IDbData> _baseDic;
        internal MySqlDbConnect (string connectString)
        {
            //注册表映射类型
            _connection = new MySqlConnection (connectString);
            _baseDic = new Dictionary<string, IDbData> ();
            Open ();
        }
        #region Name

        public IDbDatabase GetBatabaseHub ()
        {
            return new MySqlDatabase (_connection);
        }

        public IDbTableBase GetTableHub ()
        {
            var commond = new MySqlCommand ();
            commond.Connection = _connection;
            return new MySqlDatabase (_connection);
        }

        public IDbColumns GetColumnsHub (string tableName)
        {
            Open ();
            var key = tableName;
            var commond = new MySqlCommand ();
            commond.Connection = _connection;
            var database = new MySqlDatabase (key, _connection);
            return database;
        }

        public IDbColumns GetColumnsHub<T> () where T : EntityBase
        {
            var tableName = DbUtil.GetTableName<T> ();
            return GetColumnsHub (tableName);
        }
        public IDbData GetDataHub (string tableName, bool isBeginTransaction = false)
        {
            Open ();
            var key = tableName;
            if (_baseDic.ContainsKey (key))
                return _baseDic[key];
            var database = new MySqlDatabase (key, _connection);
            if (!isBeginTransaction)
                database.BeginTransaction ();
            _baseDic.Add (key, database);
            return database;
        }

        public IDbData GetDataHub<T> (bool isBeginTransaction = false) where T : EntityBase
        {
            var tableName = DbUtil.GetTableName<T> ();
            return GetDataHub (tableName, isBeginTransaction);
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
        #endregion
        public void Dispose ()
        {
            throw new NotImplementedException ();
        }
    }
}