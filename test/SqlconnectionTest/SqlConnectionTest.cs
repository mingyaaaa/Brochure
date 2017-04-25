using System;
using System.Data.SqlClient;
using Brochure.Core;
using Brochure.Core.Extends;
using Brochure.Core.Helper;
using Brochure.Core.implement;
using Brochure.Core.Query;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace test.SqlconnectionTest
{
    public class SqlConnectionTest : BaseTest
    {
        private readonly string _connectionString;
        public SqlConnectionTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            var root = new ConfigurationBuilder().AddJsonFile("appSetting.json").Build();
            _connectionString = root.GetSection(ConstString.ConnectionString).Value;
        }

        [Fact]
        public void TestInsert()
        {
            var con = new SqlServerConnection(_connectionString);
            Random random = new Random();
            con.Insert(new UserDatabase()
            {
                Age = random.Next(10, 99),
                Name = "aaaa" + random.Next(10, 99),
            });
            con.Commit();
        }
        [Fact]
        public void TestDelete()
        {
            var con = new SqlServerConnection(_connectionString);
            var a = new UserDatabase()
            {
                Age = 12
            };
            con.Delete<UserDatabase>(QueryBuild.Ins.And(t => t.Age, a));
            con.Commit();
            con.Delete<UserDatabase>(QueryBuild.Ins.AndBetweeen<UserDatabase>(t => t.Age, 40, 50));
            con.Commit();
        }

        [Fact]
        public void TestDelete1()
        {
            var con = new SqlServerConnection(_connectionString);
            var a = new UserDatabase()
            {
                Age = 12
            };
            var rr = a.GetPropertyValueTuple<UserDatabase>(t => t.Age);
            con.Delete<UserDatabase>(QueryBuild.Ins.And(rr), a);
        }
    }
}
