using Brochure.Abstract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public interface IAccountService
    {
        public ValueTask<Result> VerifyAccount(string userName, string pwd);
    }
}