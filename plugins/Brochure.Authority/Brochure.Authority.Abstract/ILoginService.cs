using Brochure.Abstract.Models;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public interface ILoginService
    {
        ValueTask<Result<LoginUserModel>> Login(LoginModel loginModel);

        ValueTask<Result> VerifyUserName(string userName);

        ValueTask<Result> VerifyToken(string token);

        ValueTask<Result> RefreshToken(string userName);
    }
}