﻿using Brochure.Core;
#if Sql
using SqlConnectionDal;
#elif Oracle
using OracleConnectionDal;
#elif MySql
 using MySqlConnectionDal;
#endif

namespace ConnectionDal
{
    public class DbFactory
    {
        public static IConnection GetConnection(string connectString)
        {
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