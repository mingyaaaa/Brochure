using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Brochure.ORMTest.Querys
{
    /// <summary>
    /// The mysql db test.
    /// </summary>
    [TestClass]
    public class MysqlDbTest
    {
        private DbConnection dbConnection;
        private DbTransaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="MysqlDbTest"/> class.
        /// </summary>
        public MysqlDbTest()
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.Server = "192.168.0.6";
            builder.UserID = "test";
            builder.Password = "123456";
            builder.Database = "test";
            builder.ConnectionTimeout = 3;
            dbConnection = new MySqlConnection(builder.ToString());
        }

        /// <summary>
        /// Tests the connnect.
        /// </summary>
      //  [TestMethod]
        public void TestConnnect()
        {
            try
            {
                dbConnection.Open();
            }
            catch (MySqlException)
            {
                return;
            }

            //插入时同一个Command Connect可以复用
            var command = dbConnection.CreateCommand();
            command.CommandText = "insert testTable(Id) values('1')";
            int r = 0;
            for (int i = 4; i < 5; i++)
            {
                command.CommandText = $"insert testTable(Id) values('{i}')";
                r = command.ExecuteNonQuery();
            }
            for (int i = 1; i < 2; i++)
            {
                command.CommandText = $"update testTable set name='{i}' where id='{i}'";
                r = command.ExecuteNonQuery();
            }
            Assert.AreEqual(1, r);
        }

        /// <summary>
        /// Tests the connnect transction.
        /// </summary>
        //[TestMethod]
        public void TestConnnectTransction()
        {
            try
            {
                dbConnection.Open();
            }
            catch (MySqlException)
            {
                return;
            }
            transaction = dbConnection.BeginTransaction();
            //插入时同一个Command Connect可以复用
            var command = dbConnection.CreateCommand();
            command.CommandText = "insert testTable(Id) values('1')";
            command.Transaction = transaction;
            int r = 0;
            for (int i = 1; i < 2; i++)
            {
                command.CommandText = $"update testTable set name='{i + 1}' where id='{i}'";
                r = command.ExecuteNonQuery();
            }
            transaction.Commit();
            var command1 = dbConnection.CreateCommand();
            command1.Transaction = transaction;
            command1.CommandText = "insert testTable(Id) values('1')";
            for (int i = 2; i < 3; i++)
            {
                command1.CommandText = $"update testTable set name='{i + 2}' where id='{i}'";
                r = command1.ExecuteNonQuery();
            }
            transaction.Commit();
            Assert.AreEqual(1, r);
        }

        /// <summary>
        /// Tests the connect query.
        /// </summary>
       // [TestMethod]
        public void TestConnectQuery()
        {
            //   return;
            try
            {
                dbConnection.Open();
            }
            catch (MySqlException)
            {
                return;
            }

            var command = dbConnection.CreateCommand();
            command.CommandText = "select * from user";
            var reader = command.ExecuteReader();
            reader.Close();

            command = dbConnection.CreateCommand();
            command.CommandText = "select * from user";
            reader = command.ExecuteReader();
            reader.Close();

            command = dbConnection.CreateCommand();
            command.CommandText = "select * from user";
            reader = command.ExecuteReader();
            reader.Close();
        }

        [TestMethod]
        public void ExcuteIfSql()
        {
            var sql = @"if not exists (SELECT count(1) FROM information_schema.TABLES WHERE table_name ='Students' and TABLE_SCHEMA ='test') then create table Students(Id nvarchar(36),School nvarchar(255),ClassId nvarchar(255),PeopleId nvarchar(255),ClassCount decimal not null,No decimal not null,PRIMARY KEY ( Id ))";
            try
            {
                dbConnection.Open();
            }
            catch (MySqlException)
            {
            }
            var command = dbConnection.CreateCommand();
            command.CommandText = sql;
            try
            {
                var reader = command.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
            }
        }
    }
}