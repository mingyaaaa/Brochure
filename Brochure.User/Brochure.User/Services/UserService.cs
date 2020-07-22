using System;
using System.Linq;
using System.Threading.Tasks;
using Brochure.User.Repository;
using Grpc.Core;
using User.Rpc;
using static User.Rpc.UserService;
using UseModel = User.Rpc;
using Brochure.ORM;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;

namespace Brochure.User.Services
{
    public class UserService : UserServiceBase
    {
        private readonly IUserRepository repository;
        private readonly IDbProvider dbProvider;

        public UserService (IUserRepository repository, IDbProvider dbProvider)
        {
            this.repository = repository;
            this.dbProvider = dbProvider;
        }
        public override async Task<UserResponse> GetUser (UserRequest request, ServerCallContext context)
        {
            var query = new Query<UserEntrity> (dbProvider);
            var entrity = await repository.Get (query);
            var userResponse = new UserResponse ();
            userResponse.Users.Add (new UseModel.User ()
            {
                Id = entrity.Id,
                    Name = entrity.Name,
                    IdCard = entrity.IdCard,
                    Age = entrity.Age,
            });
            return userResponse;
        }

        public override Task<FailIdsResponse> UpdateUser (UserRequest request, ServerCallContext context)
        {
            return Task.FromResult (new FailIdsResponse ());
        }

        public override Task<UserResponse> Insert (UserRequest request, ServerCallContext context)
        {
            return Task.FromResult (new UserResponse ());
        }

        public override Task<UserResponse> InsertMuti (MutiUserRequest request, ServerCallContext context)
        {
            return Task.FromResult (new UserResponse ());
        }

        public override Task<FailIdsResponse> DeleteUser (UserRequest request, ServerCallContext context)
        {
            return Task.FromResult (new FailIdsResponse ());
        }
    }
}