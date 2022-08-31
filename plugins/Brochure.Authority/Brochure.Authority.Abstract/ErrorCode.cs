using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public enum ErrorCode
    {
        [Description("密码错误")]
        PwdError,

        [Description("用户不存在")]
        UserNoExist,

        [Description("用户已锁")]
        UserLock
    }
}