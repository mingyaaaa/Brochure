using Brochure.Abstract;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using Brochure.User.Repository;
using Brochure.User.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brochure.User.Services.Imps
{
    /// <summary>
    /// The user dal.
    /// </summary>
    public class UserDal : IUserDal
    {
        private readonly IUserRepository repository;
        private readonly IQueryBuilder builder;
        private readonly IObjectFactory objectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="objectFactory">The object factory.</param>
        public UserDal(IUserRepository repository, IQueryBuilder builder, IObjectFactory objectFactory)
        {
            this.repository = repository;
            this.builder = builder;
            this.objectFactory = objectFactory;
        }

        /// <summary>
        /// Deletes the user return error ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<string>> DeleteUserReturnErrorIds(IEnumerable<string> ids)
        {
            var userIds = await repository.DeleteManyReturnError(ids);
            return userIds;
        }

        /// <summary>
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> DeleteUsers(IEnumerable<string> ids)
        {
            var r = await repository.DeleteMany(ids);
            return r;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<UserEntrity>> GetUsers(IEnumerable<string> ids)
        {
            var idsList = ids.ToList();
            var count = idsList.Count;
            if (count == 0)
                return new List<UserEntrity>();
            var entrity = await repository.List(ids);
            return entrity;
        }

        public async ValueTask<UserEntrity> InsertAndGet(UserEntrity userEntrity)
        {
            var r = await repository.InsertAndGet(userEntrity);
            return r;
        }

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> InsertUsers(IEnumerable<UserEntrity> users)
        {
            var r = await repository.Insert(users);
            return r;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> UpdateUser(string id, IRecord record)
        {
            var useEntiry = objectFactory.Create<UserEntrity>(record);
            var r = await repository.Update(id, useEntiry);
            return r;
        }
    }
}