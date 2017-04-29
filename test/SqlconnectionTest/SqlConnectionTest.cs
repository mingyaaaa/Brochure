using System;
using Brochure.Core;
using Brochure.Core.Extends;
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
            for (int i = 0; i < 10; i++)
            {
                InsertBuid query = new InsertBuid(new UserDatabase()
                {
                    Age = random.Next(10, 99),
                    Name = "aaaa" + random.Next(10, 99),
                });
                con.Insert(query);
            }

            con.Commit();
        }
        [Fact]
        public void TestDelete()
        {
            var con = new SqlServerConnection(_connectionString);
            var a = new UserDatabase()
            {
                Age = 62
            };
            var query = new DeleteBuild(new WhereBuild().And(a.Equal(t => t.Age)));
            con.Delete(query);
            con.Commit();
            query = new DeleteBuild(new WhereBuild().And(a.Between(t => t.Age, 50, 100)));
            con.Delete(query);
            con.Commit();
        }

        [Fact]
        public void TestUpdate()
        {
            var con = new SqlServerConnection(_connectionString);
            var a = new UserDatabase()
            {
                Id = new Guid("32D1CE3B-D841-4B71-84C3-324A2C969505"),
                Age = 46,
                Name = "bbb0000"
            };
            var param = new ParamBuild(new RecordDocument(new
            {
                a.Age,
                a.Name
            }));
            var where = new WhereBuild().And(a.Equal(t => t.Id));
            var query = new UpdateBuild(param, where);
            con.Update(query);
            con.Commit();
        }
    }
}
