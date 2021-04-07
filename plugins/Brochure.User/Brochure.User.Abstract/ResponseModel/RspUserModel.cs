using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.User.Abstract.ResponseModel
{
    /// <summary>
    /// The rsp user model.
    /// </summary>
    public class RspUserModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        /// <value></value>
        public int? Age { get; set; }
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
    }
}
