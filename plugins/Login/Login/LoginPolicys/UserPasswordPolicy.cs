using Brochure.Abstract.Models;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.LoginPolicys
{
    /// <summary>
    /// The user password policy.
    /// </summary>
    internal class UserPasswordPolicy : ILoginPolicy
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type => "password";

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="reqLoginModel">The req login model.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<RspLoginModel> Login(ReqLoginModel reqLoginModel)
        {
            return ValueTask.FromResult(new RspLoginModel());
        }
    }
}