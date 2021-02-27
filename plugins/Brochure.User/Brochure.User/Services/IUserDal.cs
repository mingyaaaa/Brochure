using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using Brochure.User.Models;
using Brochure.User.Repository;
namespace Brochure.User.Services
{
    /// <summary>
    /// The user dal.
    /// </summary>
    public interface IUserDal
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserModel>> GetUsers(IEnumerable<string> ids);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="record">The record.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> UpdateUser(string id, IRecord record);

        /// <summary>
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<int> DeleteUsers(IEnumerable<string> ids);

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        ValueTask<IEnumerable<UserModel>> InsertUsers(IEnumerable<UserModel> users);
    }

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
        /// Deletes the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<int> DeleteUsers(IEnumerable<string> ids)
        {
            var r = await repository.DeleteMany(ids);
            return r ? ids.Count() : -1;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<UserModel>> GetUsers(IEnumerable<string> ids)
        {
            var list = new List<UserModel>();
            var idsList = ids.ToList();
            var count = idsList.Count;
            if (count == 0)
                return list;
            Expression<Func<UserEntrity, bool>> fun = null;
            if (count == 1)
                fun = t => t.Id == idsList[0];
            else
                fun = t => ids.Contains(t.Id);
            var query = builder.From<UserEntrity>().WhereAnd(fun);
            var entrity = await repository.List(query);
            foreach (var item in entrity)
            {
                var model = objectFactory.Create<UserEntrity, UserModel>(item);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<UserModel>> InsertUsers(IEnumerable<UserModel> users)
        {
            var userEntirys = users.Select(t => objectFactory.Create<UserModel, UserEntrity>(t));
            var list = await repository.Insert(userEntirys);
            return users;
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