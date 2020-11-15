using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Brochure.Abstract.Models;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using Brochure.User.Repository;
using UserModel = User.Rpc;
namespace Brochure.User.Services
{
    public interface IUserDal
    {
        ValueTask<IEnumerable<UserModel.User>> GetUsers (IEnumerable<string> ids);

        ValueTask<IEnumerable<UserModel.User>> UpdateUser (string id, Record record);

        ValueTask<IEnumerable<UserModel.User>> DeleteUsers (IEnumerable<string> ids);

        ValueTask<IEnumerable<UserModel.User>> InsertUsers (IEnumerable<UserModel.User> users);
    }

    public class UserDal : IUserDal
    {
        private readonly IUserRepository repository;
        private readonly IQueryBuilder builder;

        public UserDal (IUserRepository repository, IQueryBuilder builder)
        {
            this.repository = repository;
            this.builder = builder;
        }

        public ValueTask<IEnumerable<UserModel.User>> DeleteUsers (IEnumerable<string> ids)
        {
            throw new System.NotImplementedException ();
        }

        public async ValueTask<IEnumerable<UserModel.User>> GetUsers (IEnumerable<string> ids)
        {
            var idsList = ids.ToList ();
            var count = idsList.Count;
            if (count == 0)
                return new List<UserModel.User> ();
            Expression<Func<UserEntrity, bool>> fun = null;
            if (count == 1)
                fun = t => t.Id == idsList[0];
            else
                fun = t => ids.Contains (t.Id);
            var query = builder.From<UserEntrity> ().WhereAnd (fun);
            var entrity = await repository.Get (query);
            return new List<UserModel.User> ();
        }

        public ValueTask<IEnumerable<UserModel.User>> InsertUsers (IEnumerable<UserModel.User> users)
        {
            throw new System.NotImplementedException ();
        }

        public ValueTask<IEnumerable<UserModel.User>> UpdateUser (string id, Record record)
        {
            throw new System.NotImplementedException ();
        }
    }
}