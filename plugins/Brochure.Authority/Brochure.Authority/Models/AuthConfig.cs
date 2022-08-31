using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Models
{
    public class AuthConfig
    {
        /// <summary>
        /// Gets or sets the login expire time. 单位分钟
        /// </summary>
        public int LoginExpireTime { get; set; } = 5;

        /// <summary>
        /// Gets or sets the login lock time.
        /// </summary>
        public int LoginLockTime { get; set; } = 5;

        /// <summary>
        /// 密码输入错误次数
        /// </summary>
        public int LoginErrorCount { get; set; } = 5;
    }
}