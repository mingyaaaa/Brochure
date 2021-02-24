using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.User.Abstract
{
    public class UserServiceModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserServiceModel"/> class.
        /// </summary>
        public UserServiceModel() { 
        }

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
        /// 重复密码
        /// </summary>
        /// <value></value>
        public string RePassword { get; set; }
    }
}
