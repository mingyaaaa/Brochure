using System;
using System.ComponentModel.DataAnnotations;
using Brochure.User.Entrities;

namespace Brochure.User.Models
{
    public class UserModel
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
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 重复密码
        /// </summary>
        /// <value></value>
        public string RePassword { get; set; }

        public UserEntrity GetEntrity ()
        {
            var entity = new UserEntrity
            {
                Age = this.Age,
                Name = this.Name,
                IdCard = this.IdCard,
                Password = this.Password,
                UserId = this.UserId,
                CreateTime = DateTime.Now
            };
            return entity;
        }
    }
}