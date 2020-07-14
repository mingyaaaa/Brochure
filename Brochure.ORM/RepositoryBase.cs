using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.ORM.Database;
using Brochure.ORM.Querys;

namespace Brochure.ORM
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : EntityBase
    {
        private readonly DbContext context;
        private readonly DbData dbData;
        public RepositoryBase (DbContext context)
        {
            this.context = context;
            dbData = context.GetDbData ();
        }
        public Task<bool> Delete (IQuery query)
        {
            var r = dbData.Delete<T> (query);
            return Task.FromResult (r > 0);
        }

        public Task<bool> Delete (Guid id)
        {
            var query = new Query<T> (context.GetDbProvider ());
            var q = query.WhereAnd (t => t.Id == id);
            var r = dbData.Delete<T> (query);
            return Task.FromResult (r > 0);
        }

        public async Task<T> Get (IQuery query)
        {
            var t = dbData.Query<T> (query);
            return t.FirstOrDefault ();
        }

        public Task<T> Get (Guid id)
        {
            throw new NotImplementedException ();
        }

        public Task<IEnumerable<T>> List (IQuery query)
        {
            throw new NotImplementedException ();
        }

        public Task<IEnumerable<T>> List (IEnumerable<Guid> ids)
        {
            throw new NotImplementedException ();
        }

        public Task<T> Update (IQuery query, T entity)
        {
            throw new NotImplementedException ();
        }

        public Task<T> Update (Guid id, T entity)
        {
            throw new NotImplementedException ();
        }
    }
}