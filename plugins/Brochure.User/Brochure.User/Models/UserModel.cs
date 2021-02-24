using System;
using System.ComponentModel.DataAnnotations;
using Brochure.Abstract;
using Brochure.Extensions;
using Brochure.ORM;
using Brochure.User.Entrities;
namespace Brochure.User.Models
{

    /// <summary>
    /// The user model.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserModel"/> class.
        /// </summary>
        public UserModel () : base () { }

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
    }
}