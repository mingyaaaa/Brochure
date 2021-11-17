using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkTest.BenchmarkTest;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Server;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var insert = new InsertTest();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await insert.Insert();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            await insert.InsertEf();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}