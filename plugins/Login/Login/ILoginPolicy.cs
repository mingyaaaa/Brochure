using Brochure.Abstract.Models;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    /// <summary>
    /// The login policy.
    /// </summary>
    internal interface ILoginPolicy
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="reqLoginModel">The req login model.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<RspLoginModel> Login(ReqLoginModel reqLoginModel);
    }
}