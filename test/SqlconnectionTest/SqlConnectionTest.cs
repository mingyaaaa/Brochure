using System;
using Brochure.Core;
using Brochure.Core.Extends;
using Brochure.Core.implement;
using Brochure.Core.Query;
using ConnectionDal;
using Microsoft.Extensions.Configuration;
using test.Entity;
using Xunit;
using Xunit.Abstractions;

namespace test.SqlconnectionTest
{
    public class SqlConnectionTest : BaseTest
    {
        private readonly Setting _setting;
        public SqlConnectionTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _setting = Singleton.GetInstace<Setting>();
            var root = new ConfigurationBuilder().AddJsonFile("appSetting.json").Build();
            _setting.ConnectString = root.GetSection(ConstString.ConnectionString).Value;
        }

        [Fact]
        public void TestInsert()
        {
            var con = DbFactory.GetConnection(_setting.ConnectString);
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                InsertBuid query = new InsertBuid(new UserEntity()
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
            var con = DbFactory.GetConnection(_setting.ConnectString);
            var a = new UserEntity()
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
            var setting = Singleton.GetInstace<Setting>();
            var con = DbFactory.GetConnection(setting.ConnectString);
            var a = new UserEntity()
            {
                Id = new Guid("32D1CE3B-D841-4B71-84C3-324A2C969505"),
                Age = 46,
                Name = "cccccccccc"
            };
            var param = new UpdateParamBuild(a.UpdateParam(t => t.Age, t => t.Name));
            var where = new WhereBuild().And(a.Equal(t => t.Id));
            var query = new UpdateBuild(param, where);
            con.Update(query);
            con.Commit();
        }

        [Fact]
        public void TestSearch()
        {
            var con = DbFactory.GetConnection(_setting.ConnectString);
            var a = new UserEntity()
            {
                Id = new Guid("32D1CE3B-D841-4B71-84C3-324A2C969505"),
                Age = 46,
                Name = "cccccccccc"
            };
            var select = new SelectBuild(a, new SelectParamBuild(true));
            // var list = con.Search(select);

            var obj = con.GetInfoById<UserEntity>(a.Id);

        }
    }
}
