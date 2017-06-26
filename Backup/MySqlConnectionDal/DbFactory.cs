﻿using Brochure.Core;
using Brochure.Core.implement;

namespace ConnectionDal
{
    public class DbFactory
    {
        public static IConnection GetConnection()
        {
            var connectString = Singleton.GetInstace<Setting>().ConnectString;
#if Sql
            return new SqlServerConnection(connectString);
#elif Oracle
            return new OracleServerConnection(connectString);
#elif MySql
            return new MySqlServerConnection(connectString);

#endif
        }
    }
}