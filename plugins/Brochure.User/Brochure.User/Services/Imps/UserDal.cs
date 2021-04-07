﻿using Brochure.Abstract;
using Brochure.ORM.Querys;
using Brochure.User.Abstract.RequestModel;
using Brochure.User.Abstract.ResponseModel;
using Brochure.User.Entrities;
using Brochure.User.Repository;
using Brochure.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            Expression<Func<UserEntrity, bool>> fun = null;
            if (count == 1)
                fun = t => t.Id == idsList[0];
            else
                fun = t => ids.Contains(t.Id);
            var query = builder.Build<UserEntrity>().WhereAnd(fun);
            var entrity = await repository.List(query);
            return entrity;
        }

        /// <summary>
        /// Inserts the users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IEnumerable<UserEntrity>> InsertUsers(IEnumerable<ReqAddUserModel> users)
        {
            var userEntirys = users.Select(t => objectFactory.Create<ReqAddUserModel, UserEntrity>(t));
            var list = await repository.Insert(userEntirys);
            return userEntirys;
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