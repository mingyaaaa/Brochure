using Brochure.Abstract.Models;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public interface ILoginService
    {
        ValueTask<IResult> Login(LoginModel loginModel);

        ValueTask<IResult> VerifyUserName(string userName);

        ValueTask<IResult> VerifyToken(string token);

        ValueTask<IResult> RefreshToken(string userName);
    }
}