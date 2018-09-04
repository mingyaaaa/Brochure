namespace Brochure.Core.Server
{
    public abstract class DbFactoryAbstract
    {

        protected DbFactoryAbstract(string address, string userName,
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
        public static TypeMap TypeMap { get; set; }
        public abstract IDbConnect GetDbConnect();
    }
}
