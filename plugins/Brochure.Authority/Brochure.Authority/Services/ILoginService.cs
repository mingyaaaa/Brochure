using System;
using System.Threading.Tasks;
using Brochure.Authority.Models;

namespace Brochure.Authority.Services
{
    public class LoginService : ILoginService
    {
        public ValueTask<bool> Login (LoginModel loginModel)
        {
            throw new NotImplementedException ();
        }

        public ValueTask<string> RefreshToken (string userName)
        {
            throw new NotImplementedException ();
        }

        public ValueTask<bool> VerifyToken (string token)
        {
            throw new NotImplementedException ();
        }

        public ValueTask<bool> VerifyUserName (string userName)
        {
            throw new NotImplementedException ();
        }
    }

    public interface ILoginService
    {
        ValueTask<bool> Login (LoginModel loginModel);

        ValueTask<bool> VerifyUserName (string userName);

        ValueTask<bool> VerifyToken (string token);

        ValueTask<string> RefreshToken (string userName);
    }

}