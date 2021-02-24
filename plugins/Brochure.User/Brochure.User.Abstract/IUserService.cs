using Brochure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.User.Abstract
{
    public interface IUserService
    {
        ValueTask<IEnumerable<UserServiceModel>> GetUsers();

        ValueTask<IEnumerable<UserServiceModel>> GetUsers(IEnumerable<string> ids);

        ValueTask<int> UpdateUser(string id, IRecord record);

        ValueTask<int> DeleteUsers(IEnumerable<string> ids);

        ValueTask<IEnumerable<UserServiceModel>> InsertUsers(IEnumerable<UserServiceModel> users);
    }
}
