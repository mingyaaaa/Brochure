using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Brochure.Test
{
    [TestClass]
    public class DbTest
    {
        private IDbConnection dbConnection;
        private IDbTransaction transaction;
        public DbTest ()
        {
            var builder = new MySqlConnectionStringBuilder ();
            builder.Server = "192.168.56.101";
            builder.UserID = "root";
            builder.Password = "123";
            builder.Database = "test";
            dbConnection = new MySqlConnection (builder.ToString ());

        }

        [TestMethod]
        public void TestConnnect ()
        {
            dbConnection.Open ();
            //插入时同一个Command Connect可以复用
            var command = dbConnection.CreateCommand ();
            command.CommandText = "insert testTable(Id) values('1')";
            int r = 0;
            for (int i = 4; i < 5; i++)
            {
                command.CommandText = $"insert testTable(Id) values('{i}')";
                r = command.ExecuteNonQuery ();
            }
            for (int i = 1; i < 2; i++)
            {
                command.CommandText = $"update testTable set name='{i}' where id='{i}'";
                r = command.ExecuteNonQuery ();
            }
            Assert.AreEqual (1, r);
        }

        [TestMethod]
        public void TestConnnectTransction ()
        {
            dbConnection.Open ();
            transaction = dbConnection.BeginTransaction ();
            //插入时同一个Command Connect可以复用
            var command = dbConnection.CreateCommand ();
            command.CommandText = "insert testTable(Id) values('1')";
            command.Transaction = transaction;
            int r = 0;
            for (int i = 1; i < 2; i++)
            {
                command.CommandText = $"update testTable set name='{i+1}' where id='{i}'";
                r = command.ExecuteNonQuery ();
            }
            transaction.Commit ();
            var command1 = dbConnection.CreateCommand ();
            command1.Transaction = transaction;
            command1.CommandText = "insert testTable(Id) values('1')";
            for (int i = 2; i < 3; i++)
            {
                command1.CommandText = $"update testTable set name='{i+2}' where id='{i}'";
                r = command1.ExecuteNonQuery ();
            }
            transaction.Commit ();
            Assert.AreEqual (1, r);
        }

    }
}