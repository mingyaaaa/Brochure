using System;
using Brochure.ORM;
using Brochure.ORM.Database;
using MySql.Data.MySqlClient;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlOption : DbOption
    {
        public MySqlOption() { }

        public void UseMysq(string server, string user, string pwd, string database = "", int connectTimeOut = 30)
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.UserID = user;
            builder.Password = pwd;
            builder.Database = database;
            builder.ConnectionTimeout = (uint)connectTimeOut;
            this.ConnectionString = builder.ToString();
            this.Timeout = connectTimeOut;
        }
    }
}