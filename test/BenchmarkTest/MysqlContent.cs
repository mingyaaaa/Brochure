using BenchmarkTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    public class MysqlContent : DbContext
    {
        public MysqlContent(DbContextOptions<MysqlContent> options) : base(options)
        {
        }

        public MysqlContent()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.0.6;database=test;uid=test;pwd=123456");
        }

        public virtual DbSet<UserEntrity> UserEntrities { get; set; }
    }
}