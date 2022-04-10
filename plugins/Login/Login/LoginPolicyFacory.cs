using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    /// <summary>
    /// The login policy facory.
    /// </summary>
    internal class LoginPolicyFacory
    {
        private readonly IEnumerable<ILoginPolicy> _loginPolicies;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPolicyFacory"/> class.
        /// </summary>
        /// <param name="loginPolicies">The login policies.</param>
        public LoginPolicyFacory(IEnumerable<ILoginPolicy> loginPolicies)
        {
            _loginPolicies = loginPolicies;
        }

        /// <summary>
        /// Creates the policy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An ILoginPolicy? .</returns>
        public virtual ILoginPolicy? CreatePolicy(string type)
        {
            return _loginPolicies.FirstOrDefault(t => t.Type == type);
        }
    }
}