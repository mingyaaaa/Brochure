using Brochure.Core.Server;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Brochure.Server.MySql
{
    public class MySqlDbFactory : DbFactory
    {
        public const string _connectStr = "Data Source={0};User Id={1};Password={2};pooling=false;CharSet=utf8;port={3};SslMode = none;";
        public MySqlDbFactory(string address, string userName, string password, string port) : base(address, userName, password, port, "")
        {
        }
        public MySqlDbFactory(string address, string userName, string password, string port, string database) : base(address, userName, password, port, database)
        {
        }
        public override DbConnection CreateDbConnect()
        {
            var connectString = string.Format(_connectStr, Address, UserName, Password, Port);
            if (!string.IsNullOrWhiteSpace(DatabaseName))
                connectString = $"{connectString} Database={DatabaseName};";
            return new MySqlConnection(connectString);
        }
    }
}
