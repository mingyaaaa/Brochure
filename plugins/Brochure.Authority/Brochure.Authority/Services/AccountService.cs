using Brochure.Abstract.Models;
using Brochure.Authority.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Services
{
    internal class AccountService : IAccountService
    {
        public ValueTask<Result<string>> VerifyAccount(string userName, string pwd)
        {
            throw new NotImplementedException();
        }
    }
}