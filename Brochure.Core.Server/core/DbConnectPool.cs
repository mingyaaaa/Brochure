using Brochure.Core.Server.Abstracts.sql;
using Brochure.Core.Server.Enums.sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.Core.Server.core
{
    public static class DbConnectPool
    {

        //todo 地处没有处理未使用DbConnection的驱动
        private static IDictionary<DatabaseType, DbFactory> _factoryDic = new Dictionary<DatabaseType, DbFactory>();
        public static void RegistFactory(DbFactory factory, DatabaseType type)
        {
            if (!_factoryDic.ContainsKey(type))
                _factoryDic.Add(type, factory);
        }
        public static DbConnection GetDbConnection(DatabaseType databaseType, string databaseName = null)
        {
            if (!_factoryDic.ContainsKey(databaseType))
                throw new Exception("未注册DbFactory工厂类");
            var factory = _factoryDic[databaseType];
            factory.DatabaseName = databaseName;
            var connect = factory.CreateDbConnect();
            var proxy = new DbConnectionProxy(connect, databaseType);
            return proxy;
        }

    }

}
