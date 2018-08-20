using Brochure.Core.Abstracts;
using Brochure.Core.Server.Interfaces;

namespace Brochure.Core.Server.Abstracts
{
    public abstract class DbFactoryAbstract : Singleton
    {

        protected DbFactoryAbstract (string address, string userName, string password, string port, string databaseName)
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
        public abstract IDbConnect GetDbConnect ();
    }
}