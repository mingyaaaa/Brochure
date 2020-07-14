using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Core.Server.Interfaces;
using Brochure.ORM;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    public interface IUserRepository : IRepository
    {
        UserEntrity GetUser (IQuery query);

        IEnumerable<UserEntrity> GetUsers (IQuery query);

        int DeleteUser (IQuery query);

        int UpdateUser (object obj, IQuery query);

        UserEntrity Add (UserEntrity entrity);

        Task<UserEntrity> GetUserAsync (IQuery query);

        Task<IEnumerable<UserEntrity>> GetUsersAsync (IQuery query);

        Task<int> DeleteUserAsync (IQuery query);

        Task<int> UpdateUserAsync (object obj, IQuery query);

        Task<UserEntrity> AddAsync (UserEntrity entrity);

        Task<int> DeleteUserByUserIdAsync (string[] userIds);
    }
}