﻿using Brochure.Core.Server;

namespace Brochure.Server.MySql
{
    public class MySqlDbFactory : DbFactoryAbstract
    {
        public const string _connectStr = "Data Source={0};User Id={1};Password={2};pooling=false;CharSet=utf8;port={3};SslMode = none;";
        public MySqlDbFactory (string address, string userName, string password, string port) : base (address, userName, password, port, "")
        {
            TypeMap = new MySqlTypeMap ();
        }
        public MySqlDbFactory (string address, string userName, string password, string port, string database) : base (address, userName, password, port, database)
        {
            TypeMap = new MySqlTypeMap ();
        }
        public override IDbConnect GetDbConnect ()
        {
            var connectString = string.Format (_connectStr, Address, UserName, Password, Port);
            if (!string.IsNullOrWhiteSpace (DatabaseName))
                connectString = $"{connectString} Database={DatabaseName};";
            return new MySqlDbConnect (connectString);
        }
    }
}