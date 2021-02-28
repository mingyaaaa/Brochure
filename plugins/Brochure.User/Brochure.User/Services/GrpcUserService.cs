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
using System.Collections.Generic;
using Brochure.Abstract;
using Brochure.User.Models;
using Google.Protobuf.Collections;
using Brochure.User.Abstract;
using Brochure.Abstract.Models;
using Brochure.Extensions;

namespace Brochure.User.Services
{
    /// <summary>
    /// The grpc user service.
    /// </summary>
    public class GrpcUserService : UserServiceBase
    {
        private readonly IUserDal _userDal;
        private readonly IObjectFactory _objectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userDal">The user dal.</param>
        /// <param name="objectFactory"></param>
        public GrpcUserService(IUserDal userDal, IObjectFactory objectFactory)
        {
            _userDal = userDal;
            _objectFactory = objectFactory;
        }
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override async Task<UserResponse> GetUsers(UserRequest request, ServerCallContext context)
        {
            var user = await _userDal.GetUsers(request.Ids);
            var userResponse = new UserResponse();
            foreach (var item in user)
            {
                var obj = _objectFactory.Create<UserModel, UseModel.User>(item);
                userResponse.Users.Add(obj);
            }
            return userResponse;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override async Task<FailIdsResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var user = request.Data;
            var id = user.Id;
            var updateRecord = user.As<IRecord>();
            var i = await _userDal.UpdateUser(id, updateRecord);
            var failIdsResponse = new FailIdsResponse();
            if (i <= 0)
            {
                failIdsResponse.Ids.Add(id);
            }
            return failIdsResponse;
        }

        /// <summary>
        /// Inserts the.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override async Task<UserResponse> Insert(MutiUserRequest request, ServerCallContext context)
        {
            var users = request.Users.ToList();
            var inserUsers = users.Select(t => _objectFactory.Create<UserModel>(t));
            var r = await _userDal.InsertUsers(inserUsers);
            var rsp = new UserResponse();
            foreach (var item in r)
            {
                var obj = _objectFactory.Create<UseModel.User>(item);
                rsp.Users.Add(obj);
            }
            return rsp;
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        public override async Task<FailIdsResponse> DeleteUser(UserRequest request, ServerCallContext context)
        {
            var deleteUserIds = request.Ids;
            var r = await _userDal.DeleteUsers(deleteUserIds);
            var rsp = new FailIdsResponse();

            if (r > 0)
            {
                rsp.Ids.AddRange(deleteUserIds);
            }
            return rsp;
        }
    }
}