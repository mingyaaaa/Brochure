using Brochure.ORM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenchmarkTest.Models
{
    /// <summary>
    /// The user entrity.
    /// </summary>
    [Table("user")]
    public class UserEntrity : EntityBase, IEntityKey<string>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        /// <value></value>
        public int Age { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        /// <value></value>
        public string IdCard { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        /// <value></value>
        public string UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}