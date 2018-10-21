using System.Data.Common;

namespace Brochure.Core.Server
{
    public abstract class DbFactory
    {

        protected DbFactory(string address, string userName,
                                   string password, string port,
                                   string databaseName)
        {
            Address = address;
            UserName = userName;
            Password = password;
            Port = port;
            DatabaseName = databaseName;
        }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }

        public abstract DbConnection CreateDbConnect();

    }
}
