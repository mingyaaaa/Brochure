using Brochure.Core.Server.Abstracts.sql;
using Brochure.Core.Server.Enums.sql;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Brochure.Core.Server.core
{
    public static class DbConnectPool
    {
        private static object lockObj = new object();
        //todo 地处没有处理未使用DbConnection的驱动
        private static IDictionary<DatabaseType, Queue<DbConnection>> closeQueueDic = new Dictionary<DatabaseType, Queue<DbConnection>>();
        private static IDictionary<DatabaseType, DbFactory> _factoryDic => new Dictionary<DatabaseType, DbFactory>();
        public static void RegistFactory(DbFactory factory, DatabaseType type)
        {
            if (!_factoryDic.ContainsKey(type))
            {
                _factoryDic[type] = factory;
            }

            closeQueueDic[type] = closeQueueDic[type] ?? new Queue<DbConnection>();
        }
        public static DbConnection GetDbConnection(DatabaseType databaseType, string databaseName)
        {
            var closeQueue = closeQueueDic[databaseType];
            if (closeQueue.Count > 0)
            {
                lock (lockObj)
                {
                    var orgConnection = closeQueue.Dequeue();
                    orgConnection.Open();
                    return orgConnection;
                }
            }
            if (!_factoryDic.ContainsKey(databaseType))
                throw new Exception("未注册DbFactory工厂类");
            var factory = _factoryDic[databaseType];
            var connect = factory.CreateDbConnect();
            var proxy = new DbConnectionProxy(connect, databaseType);
            if (databaseName != proxy.Database && !string.IsNullOrWhiteSpace(databaseName))
                proxy.ChangeDatabase(databaseName);
            proxy.ClosingHander += OnClosingHander;
            proxy.Open();
            return proxy;
        }
        private static void OnClosingHander(DbConnection connection)
        {
            if (connection is DbConnectionProxy)
            {
                var proxy = (DbConnectionProxy)connection;
                var databaseType = proxy.DatabaseType;
                var closeQueue = closeQueueDic[databaseType];
                closeQueue.Enqueue(proxy);
            }
        }

    }

}
