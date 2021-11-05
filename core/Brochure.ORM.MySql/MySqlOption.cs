using System;
using Brochure.ORM;
using Brochure.ORM.Database;
using MySql.Data.MySqlClient;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql option.
    /// </summary>
    public class MySqlOption : DbOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlOption"/> class.
        /// </summary>
        public MySqlOption() { }

        /// <summary>
        /// Uses the mysq.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="user">The user.</param>
        /// <param name="pwd">The pwd.</param>
        /// <param name="database">The database.</param>
        /// <param name="connectTimeOut">The connect time out.</param>
        public void UseMysq(string server, string user, string pwd, string database = "", int connectTimeOut = 30)
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.UserID = user;
            builder.Password = pwd;
            builder.Database = database;
            builder.ConnectionTimeout = (uint)connectTimeOut;
            ConnectionString = builder.ToString();
            Timeout = connectTimeOut;
        }
    }
}