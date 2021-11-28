using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkTest.Models;
using Brochure.Core.Server;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchmarkTest
{
    public class InsertTest
    {
        /// <summary>
        /// Gets the fixture.
        /// </summary>
        public Fixture Fixture { get; }

        public static int Count = 100;

        public InsertTest()
        {
            Fixture = new Fixture();
            ServiceCollection service = new ServiceCollection();
            service.AddBrochureServer().GetAwaiter().GetResult();
            service.AddDbCore(t => t.AddMySql(t =>
            {
                var mysqlBuilder = new MySqlConnectionStringBuilder();
                mysqlBuilder.Database = "test";
                mysqlBuilder.UserID = "test";
                mysqlBuilder.Password = "123456";
                mysqlBuilder.Server = "192.168.0.6";
                t.ConnectionString = mysqlBuilder.ToString();
            }));

            service.AddDbContext<MysqlContent>(t => t.UseMySQL("server=192.168.0.6;database=test;uid=test;pwd=123456"));
        }

        [Benchmark]
        public async Task Insert()
        {
            await using var dbContex = new MySqlDbContext(true);
            for (int i = 0; i < Count; i++)
            {
                var a = new UserEntrity();
                a.Age = 12;
                a.CreateTime = 12;
                await dbContex.Datas.InsertAsync<UserEntrity>(a);
            }
        }

        [Benchmark]
        public async Task InsertEf()
        {
            using var context = new MysqlContent();
            for (int i = 0; i < Count; i++)
            {
                var a = new UserEntrity();
                context.UserEntrities.Add(a);
                context.SaveChanges();
                //  context.UserEntrities.FirstOrDefault(t => t.Id == a.Id);
            }
        }

        [Benchmark]
        public async Task InsertOr()
        {
            using var connect = new MySqlConnection("server=192.168.0.6;database=test;uid=test;pwd=123456");
            connect.Open();
            var tr = connect.BeginTransaction();
            for (int i = 0; i < Count; i++)
            {
                var id = Guid.NewGuid().ToString();
                var com = connect.CreateCommand();

                com.Transaction = tr;
                com.CommandText = $"insert user(id,age,createtime) values(@p0,@p1,@p2);select * from user where id=@p0";
                com.Parameters.Add(new MySqlParameter("@p0", id));
                com.Parameters.Add(new MySqlParameter("@p1", 11));
                com.Parameters.Add(new MySqlParameter("@p2", 111));
                //      com.Parameters.Add(new MySqlParameter("@p3", Guid.NewGuid().ToString()));
                //    var a = com.ExecuteNonQuery();
                //var com2 = connect.CreateCommand();
                //com2.CommandText = $"select * from user where id=@p0";
                //com2.Parameters.Add(new MySqlParameter("@p0", id));
                var r = com.ExecuteReader();
                while (r.Read())
                {
                };
                //r.Close();
            }
            tr.Commit();
            connect.Dispose();
        }
    }
}