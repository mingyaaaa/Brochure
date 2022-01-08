using BenchmarkTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    /// <summary>
    /// The mysql content.
    /// </summary>
    public class MysqlContent : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MysqlContent"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MysqlContent(DbContextOptions<MysqlContent> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MysqlContent"/> class.
        /// </summary>
        public MysqlContent()
        {
        }

        /// <summary>
        /// Ons the configuring.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.0.6;database=test;uid=test;pwd=123456");
        }

        /// <summary>
        /// Gets or sets the user entrities.
        /// </summary>
        public virtual DbSet<UserEntrity> UserEntrities { get; set; }
    }
}