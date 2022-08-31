using Brochure.Abstract.Models;
using System.Threading.Tasks;

namespace Brochure.Authority.Abstract
{
    public interface ILoginService
    {
        ValueTask<Result<LoginUserModel>> Login(LoginModel loginModel);

        ValueTask<Result<LoginInfo>> GetFailLoginInfo(string userName);
    }
}