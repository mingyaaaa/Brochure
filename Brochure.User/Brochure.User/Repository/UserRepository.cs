using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM;
using Brochure.ORM.Extensions;
using Brochure.User.Entrities;

namespace Brochure.User.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly DbContext dbContext;

        internal UserRepository (DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public UserEntrity Add (UserEntrity entrity)
        {
            var r = this.dbContext.Insert<UserEntrity> (entrity);
            if (r > 0)
                return null;
            return entrity;
        }

        public async Task<UserEntrity> AddAsync (UserEntrity entrity)
        {
            return await Task.Run (() => Add (entrity));
        }

        public int DeleteUser (IQuery query)
        {
            return this.dbContext.Delete<UserEntrity> (query);
        }

        public async Task<int> DeleteUserByUserIdAsync (string[] userIds)
        {
            var query = dbContext.Query<UserEntrity> ().WhereAnd (t => userIds.Any (p => p == t.Id));
            return await DeleteUserAsync (query);
        }

        public async Task<int> DeleteUserAsync (IQuery query)
        {
            return await Task.Run (() => DeleteUser (query));
        }

        public UserEntrity GetUser (IQuery query)
        {
            return this.dbContext.Query<UserEntrity> (query).FirstOrDefault ();
        }

        public async Task<UserEntrity> GetUserAsync (IQuery query)
        {
            return await Task.Run (() => GetUser (query));
        }

        public IEnumerable<UserEntrity> GetUsers (IQuery query)
        {
            return this.dbContext.Query<UserEntrity> (query);
        }

        public async Task<IEnumerable<UserEntrity>> GetUsersAsync (IQuery query)
        {
            return await Task.Run (() => GetUsers (query));
        }

        public int UpdateUser (object obj, IQuery query)
        {
            return this.dbContext.Update<UserEntrity> (obj, query);
        }

        public async Task<int> UpdateUserAsync (object obj, IQuery query)
        {
            return await Task.Run (() => UpdateUser (obj, query));
        }
    }
}