using Brochure.Core;
using SqlConnectionDal;

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
