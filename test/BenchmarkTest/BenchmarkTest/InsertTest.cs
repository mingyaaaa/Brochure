using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkTest.Models;
using Brochure.Core.Server;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.ORM.Querys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchmarkTest
{
    public class InsertTest
    {
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
            for (int i = 0; i < 100; i++)
            {
                using var dbcontext = new MySqlDbContext();
                var a = new UserEntrity();
                dbcontext.Datas.Insert<UserEntrity>(a);
                var tQuery = Query.From<UserEntrity>(Query.Where<UserEntrity>(t => t.Id == a.Id)).Take(1);
                var tt = dbcontext.Datas.Query(tQuery);
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
                context.UserEntrities.FirstOrDefault(t => t.Id == a.Id);
            }
        }
    }
}